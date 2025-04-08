import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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
  selector: 'app-employees',
  standalone: false,
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css'
})
export class EmployeesComponent {
  employees: Employee[] = [];
  tableRows: any[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<Employee[]>('https://localhost:7227/api/AdminComplienceOverview').subscribe(data => {
      this.tableRows = data; console.log(data) }, error => console.log(error));
  }
}
