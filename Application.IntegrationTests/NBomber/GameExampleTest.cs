﻿using Application.IntegrationTests;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.NBomber;

[Collection("Test collection")]
public sealed class GameExampleTest : BaseTestFixture
{
    public GameExampleTest(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public void GetGamesTest()
    {
        using var httpClient = new HttpClient();

        var scenario = Scenario.Create("games_scenario", async context =>
        {
            var step1 = await Step.Run("step_1", context, async () =>
            {
                var request =
                    Http.CreateRequest("GET", "http://localhost:5283/api/Games/1/5")
                        .WithHeader("Accept", "application/json");

                var response = await Http.Send(httpClient, request);

                return response;
            });

            return Response.Ok();
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(10))
        .WithLoadSimulations(Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)));

        var result = NBomberRunner
          .RegisterScenarios(scenario)
          .Run();

        var scnStats = result.GetScenarioStats("games_scenario");
        var step1Stats = scnStats.GetStepStats("step_1");

        Assert.True(result.AllBytes > 0);
        Assert.True(result.AllRequestCount > 0);
        Assert.True(result.AllOkCount > 0);
        Assert.True(result.AllFailCount == 0);

        Assert.True(scnStats.Ok.Request.RPS > 0);
        Assert.True(scnStats.Ok.Request.Count > 0);
        Assert.True(scnStats.Ok.Latency.MinMs > 0);
        Assert.True(scnStats.Ok.Latency.MaxMs > 0);
        Assert.True(scnStats.Fail.Request.Count == 0);
        Assert.True(scnStats.Fail.Request.Count == 0);
        Assert.True(scnStats.Fail.Latency.MinMs == 0);

        Assert.True(step1Stats.Ok.Latency.Percent50 > 0);
        Assert.True(step1Stats.Ok.Latency.Percent75 > 0);
        Assert.True(step1Stats.Ok.Latency.Percent99 > 0);
    }
}
