// admin-compliance.component.ts

import { Component, OnInit } from '@angular/core';
import { Employees, EmployeeService } from '../services/Employee.Services';

@Component({
  selector: 'app-admin-compliance',
  standalone: false,
  templateUrl: './admin-compliance.component.html',
  styleUrls: ['./admin-compliance.component.css'] // note: plural 'styleUrls'
})
export class AdminComplianceComponent implements OnInit {
  employees: Employees[] = [];
 

  constructor(private employeeService: EmployeeService) { }

  ngOnInit() {
    this.loadEmployeeCompliance();
  }

  loadEmployeeCompliance() {
    this.employeeService.getAllEmployees().subscribe({
      next: (data) => (this.employees = data)
    })
  }
}
