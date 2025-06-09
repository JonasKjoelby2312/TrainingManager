
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
//import { Employees, EmployeeService } from '../services/Employee.Services';

interface Employee {
  initials: string;
  email: string;
  isActive: boolean;
  roles: string[];
  employeeTrainingStatuses: {
    [procedureName: string]: string;
  };
}

interface AdminComplianceMatrix {
  matrix: string[][];
}

@Component({
  selector: 'app-admin-compliance',
  templateUrl: './admin-compliance.component.html',
  styleUrls: ['./admin-compliance.component.css'],
  standalone: false
})
export class AdminComplianceComponent implements OnInit {
  employees: Employee[] = [];
  matrixObject: any;
  matrixData: string[][] = [];
  tableRows: any[] = [];

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {

    this.http.get<AdminComplianceComponent>('https://localhost:7227/api/AdminCompliances').subscribe(data => {
      this.matrixObject = data;
      this.matrixData = this.matrixObject.matrix;
    }, error => {
      console.log(error);
    });
  }

  getTransposedMatrix(): string[][] {
    if (!this.matrixData.length) return [];

    const numRows = this.matrixData[0].length;
    const numCols = this.matrixData.length;

    const transposed: string[][] = [];

    for (let row = 0; row < numRows; row++) {
      const newRow: string[] = [];
      for (let col = 0; col < numCols; col++) {
        newRow.push(this.matrixData[col][row]);
      }
      transposed.push(newRow);
    }

    return transposed;
  }

  getCssClass(status: string | undefined): string {
    if (!status) return '';
    switch (status.toLowerCase()) {
      case 'completed':
        return 'completed';
      case 'missing':
        return 'missing';
      case 'optional':
        return 'optional';
      case 'if performing':
        return 'if-performing';
      default:
        return '';
    }
  }

  //navigateToAdminEmployeeCompliance(initialsClicked: string) {

  //  const searchString = initialsClicked;

  //  this.router.navigate(['/components/admin-employee-compliance'], { queryParams: { search: searchString } });
  //}

}
