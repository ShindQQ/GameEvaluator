using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Presentration.API.Health;

public sealed class DatabaseHealthCheck : IHealthCheck
{
    private string ConnectionString { get; init; }

    public DatabaseHealthCheck(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = new())
    {
        using var connection = new SqlConnection(ConnectionString);

        try
        {
            await connection.OpenAsync(cancellationToken);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(exception: exception);
        }

        return HealthCheckResult.Healthy();
    }
}
