import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { ComplianceComponent } from './pages/compliance/compliance.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'compliance', component: ComplianceComponent },
  { path: '**', redirectTo: '' }
];
