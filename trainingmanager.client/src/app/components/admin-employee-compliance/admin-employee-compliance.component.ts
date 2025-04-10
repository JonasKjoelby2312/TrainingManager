import { Component, EmbeddedViewRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface EmployeeCompliance {
  rows: string[][];
}

@Component({
  selector: 'app-admin-employee-compliance',
  standalone: false,
  templateUrl: './admin-employee-compliance.component.html',
  styleUrl: './admin-employee-compliance.component.css'
})
export class AdminEmployeeComplianceComponent {
  tableRows: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<EmployeeCompliance>('https://localhost:7227/api/EmployeeCompliance/lw').subscribe(data => {
      this.tableRows = data;
    });
  }
}
