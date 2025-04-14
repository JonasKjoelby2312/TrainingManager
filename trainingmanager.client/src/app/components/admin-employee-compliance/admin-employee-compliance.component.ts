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
  tableRows: any[] = [];

  constructor(private http: HttpClient) { }

  searchForCompliance() {
    const searchInput = <HTMLInputElement>document.getElementById("searchInput");
    let apiConnectionString = 'https://localhost:7227/api/EmployeeCompliance/' + searchInput.value.toString();
    this.http.get<EmployeeCompliance>(apiConnectionString).subscribe(data => {
      this.tableRows = data.rows;
    });
  }
}
