export interface ComplianceDto {
  aircraftId: number;
  tailNumber: string;
  model: string;
  manufacturer: string;
  nextDueDate: string;
  daysUntilDue: number;
  recentChecks: number;
}
