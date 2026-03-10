using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace backend.Services;

public class DatabaseStartupService : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseStartupService> _logger;

    public DatabaseStartupService(IConfiguration configuration, ILogger<DatabaseStartupService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("--- Azure SQL Database Startup Check ---");

        var connectionString = _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        LogConnectionTarget(connectionString);

        try
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            _logger.LogInformation("✓ Connection successful.");

            // Query Aircraft table record count
            var aircraftCount = await connection.QuerySingleAsync<int>(
                "SELECT COUNT(*) FROM Aircraft",
                commandTimeout: 10
            );

            // Query ComplianceLogs table record count
            var logCount = await connection.QuerySingleAsync<int>(
                "SELECT COUNT(*) FROM ComplianceLogs",
                commandTimeout: 10
            );

            _logger.LogInformation("✓ Aircraft table       : {Count} record(s)", aircraftCount);
            _logger.LogInformation("✓ ComplianceLogs table : {Count} record(s)", logCount);
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
        var server = ExtractSegment(connStr, "Server=tcp:", ",");
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
