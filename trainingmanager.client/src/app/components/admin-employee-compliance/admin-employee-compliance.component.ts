import { Component, EmbeddedViewRef, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface MatrixParent {
  matrix: string[][];
}

interface Employee {
  initials: string;
  email: string;
  isActive: boolean;
  roles: string[];
  employeeTrainingStatuses: { [procedureName: string]: string };
}

@Component({
  selector: 'app-admin-employee-compliance',
  standalone: false,
  templateUrl: './admin-employee-compliance.component.html',
  styleUrl: './admin-employee-compliance.component.css'
})
export class AdminEmployeeComplianceComponent {
  tableRows: any[] = [];
  allEmployees: Employee[] = [];
  employeesInitials: string[] = [];
  filteredInitials: string[] = [];
  showDropDown: boolean = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<Employee[]>('https://localhost:7227/api/EmployeeOverview/').subscribe(data => {
      console.log('EmployeeCompliance', data)
      this.allEmployees = data;
      for (var i = 0; i < data.length; i++) {
        this.employeesInitials.push(data[i].initials);
        console.log(data[i].initials);
      }
    });
  }

  toggleDropDown() {
    this.showDropDown = !this.showDropDown;
    console.log('drop down: toggled');
  }

  onInputChange(searchString: string) {
    this.filteredInitials = this.employeesInitials.filter(inits => inits.toLowerCase().includes(searchString.toLowerCase()));
  }

  searchForCompliance(searchString: string) {
    //const searchInput = <HTMLInputElement>document.getElementById("searchInput");
    let apiConnectionString = 'https://localhost:7227/api/EmployeeCompliance/' + searchString; //searchInput.value.toString();
    this.http.get<MatrixParent>(apiConnectionString).subscribe(data => {
      this.tableRows = data.matrix;
    });
  }

  getCssClass(status: string | undefined): string {
    console.log('getcss running');
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
