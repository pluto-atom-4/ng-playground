namespace backend.Models;

public class Aircraft
{
    public int AircraftId { get; set; }
    public string TailNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public DateTime NextDueDate { get; set; }
    
    public ICollection<ComplianceLog> ComplianceLogs { get; set; } = [];
}
