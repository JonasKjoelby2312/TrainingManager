import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

export interface Employees {
  initials: string;
  email: string;
  isActive: boolean;
  roles: string[];
  employeeTrainingStatuses: {
    [procedureName: string]: string;
  };
}

// employee.service.ts
@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private apiUrl = 'https://localhost:7139/api/AdminComplienceOverview'; //Christians
  //private apiUrl = 'https://localhost:7227/api/AdminComplienceOverview';

  constructor(private http: HttpClient) { }

  getAllEmployees(): Observable<Employees[]> {
    return this.http.get<Employees[]>(this.apiUrl);
  }
}
