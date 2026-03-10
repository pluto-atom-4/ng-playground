using backend.DTOs;
using backend.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace backend.Services;

public class AircraftComplianceService
{
    private readonly IConfiguration _configuration;

    public AircraftComplianceService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<ComplianceDto>> GetOverdueAircraftAsync(string? modelFilter = null)
    {
        var now = DateTime.UtcNow;
        var oneYearAgo = now.AddYears(-1);
        var oneMonthFromNow = now.AddMonths(1);

        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Get aircraft with upcoming/overdue compliance dates
        var aircraftQuery = @"
            SELECT 
                a.AircraftId,
                a.TailNumber,
                a.Model,
                a.Manufacturer,
                a.NextDueDate,
                DATEDIFF(DAY, @Now, a.NextDueDate) AS DaysUntilDue
            FROM Aircraft a
            WHERE a.NextDueDate < @OneMonthFromNow
        ";

        if (!string.IsNullOrEmpty(modelFilter))
        {
            aircraftQuery += " AND a.Model LIKE @ModelFilter";
        }

        aircraftQuery += " ORDER BY a.NextDueDate ASC";

        var aircraftData = await connection.QueryAsync<dynamic>(
            aircraftQuery,
            new { Now = now, OneMonthFromNow = oneMonthFromNow, ModelFilter = $"%{modelFilter}%" }
        );

        var results = new List<ComplianceDto>();

        foreach (var aircraft in aircraftData)
        {
            // Get recent compliance checks for this aircraft
            var checksQuery = @"
                SELECT COUNT(*) 
                FROM ComplianceLogs 
                WHERE AircraftId = @AircraftId 
                AND PerformedDate >= @OneYearAgo
            ";

            var recentChecks = await connection.QuerySingleAsync<int>(
                checksQuery,
                new { AircraftId = (int)aircraft.AircraftId, OneYearAgo = oneYearAgo }
            );

            results.Add(new ComplianceDto(
                aircraft.AircraftId,
                aircraft.TailNumber,
                aircraft.Model,
                aircraft.Manufacturer,
                aircraft.NextDueDate,
                aircraft.DaysUntilDue,
                recentChecks
            ));
        }

        return results;
    }
}
