using Application.IntegrationTests;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests;

[CollectionDefinition("Test collection")]
public sealed class SharedTestCollection : ICollectionFixture<CustomerApiFactory>
{
}
