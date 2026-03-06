using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class ComplianceLog
{
    [Key]
    public int LogId { get; set; }
    public int AircraftId { get; set; }
    public DateTime PerformedDate { get; set; }
    public string CheckType { get; set; } = string.Empty;
    
    public Aircraft Aircraft { get; set; } = null!;
}
