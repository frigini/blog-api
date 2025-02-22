using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using Serilog;
using System.Net.Sockets;

namespace BlogApi.Infra.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task MigrateDatabase<TContext>(
        this IApplicationBuilder app,
        Serilog.ILogger? logger = null) where TContext : DbContext
    {
        var policy = Policy
            .Handle<NpgsqlException>()
            .Or<SocketException>()
            .WaitAndRetryAsync(
                retryCount: 10,
                sleepDurationProvider: retryAttempt => 
                    TimeSpan.FromSeconds(Math.Min(Math.Pow(2, retryAttempt), 30)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Log.Warning(
                        exception,
                        "Database connection attempt {RetryCount} failed. Retrying in {RetryIn:n1}s",
                        retryCount,
                        timeSpan.TotalSeconds
                    );
                }
            );

        await policy.ExecuteAsync(async () =>
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            
            Log.Information("Applying database migrations...");
            await context.Database.MigrateAsync();
            Log.Information("Database migrated successfully");
        });
    }
}