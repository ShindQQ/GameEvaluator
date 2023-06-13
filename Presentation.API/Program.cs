using Application;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Infrastructre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Presentation.API.Middlewares;
using Presentration.API;
using Presentration.API.BackgroundJobs;
using Presentration.API.Health;
using Presentration.API.Hubs;
using Presentration.API.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddAPI(builder.Configuration)
    .AddOpenTelemetry(builder.Configuration);

var app = builder.Build();

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).RequireAuthorization(new AuthorizeAttribute() { Roles = "SuperAdmin", });

app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    await scope.AddSuperAdminRole();
}

app.UseOutputCache();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<RequestsMiddleware>();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    DashboardTitle = "Game Evaluator",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            Pass = builder.Configuration.GetSection("SuperAdmin:Password").Value,
            User = builder.Configuration.GetSection("SuperAdmin:Password").Value
        }
    }
});

app.MapHangfireDashboard();

//RecurringJob.AddOrUpdate<Scheduler>("recomended games",
//        x => x.SendRecomendedGamesAsync(new CancellationToken()), Cron.Daily);

RecurringJob.AddOrUpdate<Scheduler>("unban users",
        x => x.UnbanUsersAsync(new CancellationToken()), Cron.Hourly);

app.MapHub<CommentsHub>("/commentsHub");
app.UseWebSockets();

await app.RunAsync();

public partial class Program { }