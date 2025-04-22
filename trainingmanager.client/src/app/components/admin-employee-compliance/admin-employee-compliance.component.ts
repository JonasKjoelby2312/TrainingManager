import { Component, EmbeddedViewRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface MatrixParent {
  matrix: string[][];
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

  searchForCompliance(searchString: string) {
    //const searchInput = <HTMLInputElement>document.getElementById("searchInput");
    let apiConnectionString = 'https://localhost:7227/api/EmployeeCompliance/' + searchString; //searchInput.value.toString();
    this.http.get<MatrixParent>(apiConnectionString).subscribe(data => {
      console.log('EmployeeCompliance', data)
      this.tableRows = data.matrix;
    });
  }
}
