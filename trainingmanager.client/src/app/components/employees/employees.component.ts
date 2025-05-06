import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface Employee {
  employeeId: number;
  initials: string;
  email: string;
  isActive: boolean;
  roles: string[];
  employeeTrainingStatuses: {[procedureName:string]: string};
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
  inactivesOpened: boolean = false;
  isCreateModalActive: boolean = false;
  newEmployee: Employee = {
    employeeId: -1, initials: "", email: "", isActive: true, roles: [], employeeTrainingStatuses: {},
  };
  listOfAllRoleNames: string[] = ["Error: Could not load list of roles"];
  selectedRoles: string[] = [];
  selectedEmployee: any = null;
  isEditModalActive = false;
  tempEditRoles: string[] = [];
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.loadListOfRoles().subscribe(roles => {
      this.listOfAllRoleNames = roles });
    this.loadAllEmployees();
  }

  toggleInactives() {
    this.inactivesOpened = !this.inactivesOpened;
  }

  loadListOfRoles(): Observable<string[]> {
    return this.http.get<string[]>('https://localhost:7227/api/Roles');
  }

  openCreateModal() {
    this.isCreateModalActive = true;
    document.addEventListener('keydown', this.handleEscapeKeyCreate);
  }

  closeCreateModal() {
    this.isCreateModalActive = false;
    document.removeEventListener('keydown', this.handleEscapeKeyCreate);
  }

  handleEscapeKeyCreate = (event: KeyboardEvent) => {
    if (event.key === 'Escape') {
      this.closeCreateModal();
    }
  };

  openEditModal(employee: any) {
    this.selectedEmployee = { ...employee };
    this.tempEditRoles = [...employee.roles];
    this.isEditModalActive = true;
    document.addEventListener('keydown', this.handleEscapeKeyEdit);
  }

  closeEditModal() {
    this.isEditModalActive = false;
    this.selectedEmployee = null;
    document.removeEventListener('keydown', this.handleEscapeKeyEdit);
  }

  handleEscapeKeyEdit = (event: KeyboardEvent) => {
    if (event.key === 'Escape') {
      this.closeEditModal();
    }
  };

  loadAllEmployees() {
    this.tableRows = [];
    this.http.get<Employee[]>('https://localhost:7227/api/EmployeeOverview').subscribe(data => {
      this.tableRows = data;
    }, error => console.log(error));
  }

  createNewEmployee(initialsInput: string, emailInput: string, isActive: boolean) {
    this.newEmployee = {
      employeeId: -1, //dummy id, correct id is set in db
      initials: initialsInput,
      email: emailInput,
      isActive: isActive,
      roles: [...this.selectedRoles],
      employeeTrainingStatuses: {}
    };
    this.http.post<number>('https://localhost:7227/api/EmployeeOverview', this.newEmployee)
      .subscribe(() => {
        this.closeCreateModal();
        this.loadAllEmployees();
        this.selectedRoles = [];
      }); 
  }

  onRoleChange(event: any) {
    const role = event.target.value;
    if (event.target.checked) {
      this.selectedRoles.push(role);
    } else {
      this.selectedRoles = this.selectedRoles.filter(r => r !== role);
    }
  }

  saveEditChanges(initials: string, email: string, isActive: boolean) {
    const index = this.tableRows.findIndex(emp => emp.email === this.selectedEmployee.email);
    console.log(this.tableRows[index]);
    if (index > -1) {
      this.tableRows[index] = {
        ...this.selectedEmployee,
        initials,
        email,
        isActive,
        roles: [...this.tempEditRoles]
      };
      console.log(this.tableRows[index]);
      this.updateEmployee(this.tableRows[index]).subscribe(success => {
        console.log(success);
        if (success) {
          //write out success
          console.log(this.tableRows[index]);
          console.log("success");
          this.loadAllEmployees();
        }
        else {
          console.log("fail");
          //write it failed
        }
      });
    }
    this.closeEditModal();
  }

  updateEmployee(employee: Employee): Observable<boolean> {
    return this.http.put<boolean>('https://localhost:7227/api/EmployeeOverview/' + employee.employeeId, employee);
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
