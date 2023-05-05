using Application.IntegrationTests;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<CustomerApiFactory>
{
}
