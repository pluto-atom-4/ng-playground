import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ComplianceService } from '../../services/compliance.service';
import { ComplianceDto } from '../../models/compliance.model';

@Component({
  selector: 'app-compliance',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './compliance.component.html',
  styleUrl: './compliance.component.scss'
})
export class ComplianceComponent implements OnInit {
  aircraft: ComplianceDto[] = [];
  modelFilter = '';
  loading = false;
  error = '';

  constructor(private complianceService: ComplianceService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = '';

    this.complianceService.getOverdueAircraft(this.modelFilter || undefined)
      .subscribe({
        next: (data) => {
          this.aircraft = data;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load compliance data';
          this.loading = false;
          console.error(err);
        }
      });
  }

  getRowClass(daysUntilDue: number): string {
    if (daysUntilDue < 0) return 'overdue';
    if (daysUntilDue <= 7) return 'due-soon';
    return '';
  }
}
