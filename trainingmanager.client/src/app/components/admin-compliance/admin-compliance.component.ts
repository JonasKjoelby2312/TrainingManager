
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-admin-compliance',
  templateUrl: './admin-compliance.component.html',
  styleUrls: ['./admin-compliance.component.css'],
  standalone: false
})
export class AdminComplianceComponent implements OnInit {
  employees: Employee[] = [];
  tableRows: any[] = [];
  employeeInitials: string[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {

    this.http.get<Employee[]>('https://localhost:7227/api/AdminComplienceOverview').subscribe(data => {
      console.log('API response: ', data)
      this.employees = data;
      this.employeeInitials = data.map(e => e.initials);
      console.log('Initials:', this.employeeInitials);

      const procedureMap: { [procedure: string]: any } = {};

      //Laver en række for hver procedure hvis den ikke eksistere endnu.
      //Sætter værdien for emplyee's column i det row
      for (const employee of data) { 
        for (const [procedure, status] of Object.entries(employee.employeeTrainingStatuses)) {
          if (!procedureMap[procedure]) {
            procedureMap[procedure] = { procedure };
          }
          procedureMap[procedure][employee.initials] = status;
        }
      }

      this.tableRows = Object.values(procedureMap);
      console.log('Table rows:', this.tableRows);
    });
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

}
