import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Employee {
  initials: string;
  email: string;
  isActive: boolean;
  roles: string[];
  employeeTrainingStatuses: null;
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
  isCreateModalActive: boolean = false;
  newEmployee: Employee = {
    initials: "", email: "", isActive: true, roles: [], employeeTrainingStatuses: {}
  };
  rolesList: string[] = ["Tester", "Application/Data Architect", "Bookkeeper", "Chief Executive Officer", "Chief Operation Officer", "Developer", "Human Resources", "Internal IT Support", "Lead Developer", "Project Manager", "PRRC", "QARA Associate", "QARA Manager", "R&D Manager", "Research Associate", "Sales/Key Account Manager", "Software Team Manager", "Specification Engineer", "Specification Team Manager", "Support Manager", "Supporter", "System Architect", "Test Manager", "Transfer/ Delivery Manager"];
  selectedRoles: string[] = [];
  selectedEmployee: any = null;
  isEditModalActive = false;
  tempEditRoles: string[] = [];
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<Employee[]>('https://localhost:7227/api/EmployeeOverview').subscribe(data => {
      this.tableRows = data; console.log(data) }, error => console.log(error));
  }

  

  openCreateModal() {
    this.isCreateModalActive = true;
  }

  closeCreateModal() {
    this.isCreateModalActive = false;
  }

  openEditModal(employee: any) {
    this.selectedEmployee = { ...employee };
    this.tempEditRoles = [...employee.roles];
    this.isEditModalActive = true;
  }

  closeEditModal() {
    this.isEditModalActive = false;
    this.selectedEmployee = null;
  }

  createNewEmployee() {
    this.openCreateModal();
    
  }

  onRoleChange(event: any) {
    const role = event.target.value;
    if (event.target.checked) {
      this.selectedRoles.push(role);
    } else {
      this.selectedRoles = this.selectedRoles.filter(r => r !== role);
    }
  }

  saveEditChanges(initials: string, email: string) {
    const index = this.tableRows.findIndex(emp => emp.email === this.selectedEmployee.email);
    if (index > -1) {
      this.tableRows[index] = {
        ...this.selectedEmployee,
        initials,
        email,
        roles: [...this.tempEditRoles]
      };
    }
    this.closeEditModal();
    this.ngOnInit();
  }

  onTempEditRoleChange(event: any) {
    const role = event.target.value;
    if (event.target.checked) {
      if (!this.tempEditRoles.includes(role)) this.tempEditRoles.push(role);
    } else {
      this.tempEditRoles = this.tempEditRoles.filter(r => r !== role);
    }
  }
}
