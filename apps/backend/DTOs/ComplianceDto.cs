namespace backend.DTOs;

public record ComplianceDto(
    int AircraftId,
    string TailNumber,
    string Model,
    string Manufacturer,
    DateTime NextDueDate,
    int DaysUntilDue,
    int RecentChecks
);
