using Application.IntegrationTests;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests;

public class BaseTestFixture : IAsyncLifetime
{
    protected readonly CustomerApiFactory _apiFactory;

    public BaseTestFixture(
        CustomerApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }

    public virtual async Task DisposeAsync() => await _apiFactory.ResetDatabaseAsync();

    public virtual Task InitializeAsync() => Task.CompletedTask;
}
