using System.Net.Http.Json;
using Documents.Infrastructure.Authorization;
using Documents.Tests.Contracts;
using Documents.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Documents.Tests;

[Collection("api")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly PostgresFixture PostgresFixture;
    protected CustomWebApplicationFactory Factory = default!;
    protected HttpClient LocalClient = default!;
    protected HttpClient IntegrationClient = default!;

    protected IntegrationTestBase(PostgresFixture postgresFixture)
    {
        PostgresFixture = postgresFixture;
    }

    public virtual Task InitializeAsync()
    {
        Factory = new CustomWebApplicationFactory(PostgresFixture.ConnectionString);

        LocalClient = Factory.CreateClient();
        LocalClient.DefaultRequestHeaders.Add(ApiRoleConstants.HeaderName, ApiRoleConstants.Local);

        IntegrationClient = Factory.CreateClient();
        IntegrationClient.DefaultRequestHeaders.Add(ApiRoleConstants.HeaderName, ApiRoleConstants.Integrator);

        return Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        LocalClient.Dispose();
        IntegrationClient.Dispose();
        await Factory.DisposeAsync();
    }

    protected async Task<Guid> CreateSampleInvoiceAsync(
        string descriptiveNo = "FV-SAMPLE-1",
        decimal taxRate = 23,
        decimal netValue = 100)
    {
        var request = new
        {
            descriptiveNo,
            kind = "Invoice",
            items = new[]
            {
                new
                {
                    articleName = "Item",
                    taxRate,
                    netValue
                }
            }
        };

        var response = await LocalClient.PostAsJsonAsync("/api/local/documents", request);
        response.EnsureSuccessStatusCode();

        var documents = await LocalClient.GetFromJsonAsync<List<DocumentListItemResponse>>("/api/local/documents");
        documents.Should().NotBeNull();

        var document = documents!.Single(x => x.DescriptiveNo == descriptiveNo);
        return document.Id;
    }

    protected Task<Guid> CreateInvoiceAsync(string descriptiveNo, decimal taxRate, decimal netValue)
        => CreateSampleInvoiceAsync(descriptiveNo, taxRate, netValue);

    protected async Task<DocumentDetailsResponse> GetDocumentAsync(Guid id)
    {
        var response = await LocalClient.GetAsync($"/api/local/documents/{id}");
        response.EnsureSuccessStatusCode();

        var document = await response.Content.ReadFromJsonAsync<DocumentDetailsResponse>();
        document.Should().NotBeNull();

        return document!;
    }
}