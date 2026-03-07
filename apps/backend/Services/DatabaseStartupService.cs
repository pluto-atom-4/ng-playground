using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class DatabaseStartupService : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<DatabaseStartupService> _logger;

    public DatabaseStartupService(IServiceProvider services, ILogger<DatabaseStartupService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("--- Azure SQL Database Startup Check ---");

        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Extract server/db from the connection string for informational logging (no credentials)
        var connStr = db.Database.GetConnectionString() ?? string.Empty;
        LogConnectionTarget(connStr);

        try
        {
            var canConnect = await db.Database.CanConnectAsync(cancellationToken);

            if (canConnect)
            {
                _logger.LogInformation("✓ Connection successful.");

                var aircraftCount  = await db.Aircraft.CountAsync(cancellationToken);
                var logCount       = await db.ComplianceLogs.CountAsync(cancellationToken);

                _logger.LogInformation("✓ Aircraft table       : {Count} record(s)", aircraftCount);
                _logger.LogInformation("✓ ComplianceLogs table : {Count} record(s)", logCount);
            }
            else
            {
                _logger.LogWarning("✗ CanConnectAsync returned false — database unreachable.");
                _logger.LogWarning("  API endpoints that query the database will return HTTP 500.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "✗ Database connection failed: {Message}", ex.Message);
            _logger.LogWarning("  API endpoints that query the database will return HTTP 500.");
        }

        _logger.LogInformation("--- Startup Check Complete ---");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    // Parses "Server=tcp:..." and "Initial Catalog=..." without exposing credentials
    private void LogConnectionTarget(string connStr)
    {
        var server   = ExtractSegment(connStr, "Server=tcp:", ",");
        var database = ExtractSegment(connStr, "Initial Catalog=", ";");

        if (!string.IsNullOrEmpty(server) || !string.IsNullOrEmpty(database))
            _logger.LogInformation("  Target → Server: {Server} | Database: {Database}", server, database);
    }

    private static string ExtractSegment(string source, string prefix, string suffix)
    {
        var start = source.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
        if (start < 0) return string.Empty;
        start += prefix.Length;
        var end = source.IndexOf(suffix, start, StringComparison.OrdinalIgnoreCase);
        return end < 0 ? source[start..] : source[start..end];
    }
}
