using backend.Data;
using backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class AircraftComplianceService
{
    private readonly AppDbContext _context;

    public AircraftComplianceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ComplianceDto>> GetOverdueAircraftAsync(string? modelFilter = null)
    {
        var now = DateTime.UtcNow;
        var oneYearAgo = now.AddYears(-1);
        var oneMonthFromNow = now.AddMonths(1);

        var query = _context.Aircraft
            .AsNoTracking()
            .Where(a => a.NextDueDate < oneMonthFromNow);

        if (!string.IsNullOrEmpty(modelFilter))
        {
            query = query.Where(a => EF.Functions.Like(a.Model, $"%{modelFilter}%"));
        }

        var results = await query
            .Select(a => new ComplianceDto(
                a.AircraftId,
                a.TailNumber,
                a.Model,
                a.Manufacturer,
                a.NextDueDate,
                (int)EF.Functions.DateDiffDay(now, a.NextDueDate),
                (a.ComplianceLogs ?? new List<ComplianceLog>())
                    .Where(cl => cl.PerformedDate >= oneYearAgo)
                    .Count()
            ))
            .ToListAsync();

        return results ?? new List<ComplianceDto>();
    }
}
