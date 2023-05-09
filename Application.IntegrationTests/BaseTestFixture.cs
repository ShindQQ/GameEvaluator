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


    public static IEnumerable<object[]> SetNameDescriptionData()
    {
        yield return new object[] { "", "" };
        yield return new object[] { "Name", "Description<20" };
        yield return new object[] { "<3", "Description<20" };
        yield return new object[] { "<3", "Description bigger then 20 characters" };
    }

    public static IEnumerable<object[]> SetUsernameEmailPasswordData()
    {
        yield return new object[] { "", "", "" };
        yield return new object[] { "Name", "email<10", "Pass_1" };
        yield return new object[] { "<3", "email<10", "incorrect" };
        yield return new object[] { "Username", "somemail@gmail.com", "TooLongPass" };
    }
}
