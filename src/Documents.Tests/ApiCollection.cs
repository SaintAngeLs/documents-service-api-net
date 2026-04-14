using Documents.Tests.Fixtures;
using Xunit;

namespace Documents.Tests;

[CollectionDefinition("api")]
public class ApiCollection : ICollectionFixture<PostgresFixture>
{
}