using Application;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Infrastructre;
using Presentration.API;
using Presentration.API.BackgroundJobs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddAPI(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.AddSuperAdminRole();
}

app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

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

await app.RunAsync();

public partial class Program { }