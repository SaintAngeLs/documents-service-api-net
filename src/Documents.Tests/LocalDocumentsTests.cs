using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Documents.Tests.Contracts;
using Documents.Tests.Fixtures;

namespace Documents.Tests;

public class LocalDocumentsTests : IntegrationTestBase
{
    public LocalDocumentsTests(PostgresFixture postgresFixture) : base(postgresFixture)
    {
    }

    [Fact]
    public async Task CreateDocument_ShouldSucceed_ForValidInvoice()
    {
        var request = new
        {
            descriptiveNo = "FV/001/04/2026",
            kind = "Invoice",
            items = new[]
            {
                new
                {
                    articleName = "Laptop",
                    taxRate = 23.0,
                    netValue = 1000.0
                }
            }
        };

        var response = await LocalClient.PostAsJsonAsync("/api/local/documents", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var listResponse = await LocalClient.GetAsync("/api/local/documents");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await listResponse.Content.ReadAsStringAsync();
        body.Should().Contain("FV/001/04/2026");
    }

    [Fact]
    public async Task CreateDocument_ShouldFail_WhenNoItems()
    {
        var request = new
        {
            descriptiveNo = "FV/EMPTY/04/2026",
            kind = "Invoice",
            items = Array.Empty<object>()
        };

        var response = await LocalClient.PostAsJsonAsync("/api/local/documents", request);

        response.IsSuccessStatusCode.Should().BeFalse();
    }

    [Fact]
    public async Task CreateReturn_ShouldFail_WhenNetValueIsPositive()
    {
        var request = new
        {
            descriptiveNo = "RET/001/04/2026",
            kind = "Return",
            items = new[]
            {
                new
                {
                    articleName = "Returned item",
                    taxRate = 23.0,
                    netValue = 100.0
                }
            }
        };

        var response = await LocalClient.PostAsJsonAsync("/api/local/documents", request);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Theory]
    [InlineData("Invoice")]
    [InlineData("Receipt")]
    public async Task CreatePositiveDocumentKinds_ShouldFail_WhenNetValueIsNegative(string kind)
    {
        var request = new
        {
            descriptiveNo = $"DOC-{kind}",
            kind,
            items = new[]
            {
                new
                {
                    articleName = "Item",
                    taxRate = 23.0,
                    netValue = -50.0
                }
            }
        };

        var response = await LocalClient.PostAsJsonAsync("/api/local/documents", request);

        response.IsSuccessStatusCode.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateDocument_ShouldFail_OnRevisionConflict()
    {
        var id = await CreateSampleInvoiceAsync("FV-CONC-1", 23, 100);
        var details = await GetDocumentAsync(id);

        var firstUpdate = new
        {
            descriptiveNo = details.DescriptiveNo,
            kind = details.Kind,
            revision = details.Revision,
            items = details.Items.Select(x => new
            {
                id = x.Id,
                articleName = x.ArticleName,
                taxRate = x.TaxRate,
                netValue = x.NetValue
            }).ToArray()
        };

        var ok = await LocalClient.PutAsJsonAsync($"/api/local/documents/{id}", firstUpdate);
        ok.EnsureSuccessStatusCode();

        var staleUpdate = await LocalClient.PutAsJsonAsync($"/api/local/documents/{id}", firstUpdate);

        staleUpdate.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UpdateDocument_ShouldFail_WhenDocumentWasIntegrated()
    {
        var id = await CreateSampleInvoiceAsync();

        var integrateResponse = await IntegrationClient.PostAsync($"/api/integration/documents/{id}/integrate", null);
        integrateResponse.EnsureSuccessStatusCode();

        var details = await GetDocumentAsync(id);

        var updateRequest = new
        {
            descriptiveNo = details.DescriptiveNo,
            kind = details.Kind,
            revision = details.Revision,
            items = details.Items.Select(x => new
            {
                id = x.Id,
                articleName = x.ArticleName + " updated",
                taxRate = x.TaxRate,
                netValue = x.NetValue
            }).ToArray()
        };

        var response = await LocalClient.PutAsJsonAsync($"/api/local/documents/{id}", updateRequest);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Integrator_ShouldReturnOnlyDocumentsSinceLastCheckpoint()
    {
        await CreateInvoiceAsync("INV-1", 23, 100);
        await CreateInvoiceAsync("INV-2", 8, 200);

        var first = await IntegrationClient.GetAsync("/api/integration/documents/aggregates");
        first.EnsureSuccessStatusCode();

        var firstBody = await first.Content.ReadAsStringAsync();
        firstBody.Should().Contain("Invoice");

        await IntegrationClient.PostAsync("/api/integration/documents/integrate-all", null);

        await CreateInvoiceAsync("INV-3", 23, 300);

        var second = await IntegrationClient.GetAsync("/api/integration/documents/aggregates");
        second.EnsureSuccessStatusCode();

        var secondBody = await second.Content.ReadAsStringAsync();

        secondBody.Should().Contain("300");
        secondBody.Should().NotContain("100");
        secondBody.Should().NotContain("200");
    }

    [Fact]
    public async Task LocalClient_ShouldNotAccess_IntegrationEndpoints()
    {
        var response = await LocalClient.GetAsync("/api/integration/documents/aggregates");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Integrator_ShouldNotAccess_LocalEndpoints()
    {
        var response = await IntegrationClient.GetAsync("/api/local/documents");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}