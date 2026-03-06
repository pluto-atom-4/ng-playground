import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ComplianceDto } from '../models/compliance.model';

@Injectable({
  providedIn: 'root'
})
export class ComplianceService {
  private readonly baseUrl = 'http://localhost:5000/api/compliance';

  constructor(private http: HttpClient) {}

  getOverdueAircraft(modelFilter?: string): Observable<ComplianceDto[]> {
    let params = new HttpParams();
    if (modelFilter) {
      params = params.set('modelFilter', modelFilter);
    }
    return this.http.get<ComplianceDto[]>(`${this.baseUrl}/overdue`, { params });
  }
}
