import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  apiMessage = '';
  loading = false;
  error = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchMessage();
  }

  fetchMessage(): void {
    this.loading = true;
    this.error = '';

    this.http.get<{ message: string }>('http://localhost:5000/api/health')
      .subscribe({
        next: (response) => {
          this.apiMessage = response.message;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to fetch from backend';
          this.loading = false;
          console.error(err);
        }
      });
  }
}
