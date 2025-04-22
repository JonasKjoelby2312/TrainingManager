import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface RolesRequiredTraining {
  roleId: number;
  roleName: string;
  trainingRequiredTypes: {
    [procedureName: string]: number;
  }
}

@Component({
  selector: 'app-roles-required-training',
  standalone: false,
  templateUrl: './roles-required-training.component.html',
  styleUrl: './roles-required-training.component.css'
})
export class RolesRequiredTrainingComponent {
  rolesRequiredTraining: RolesRequiredTraining[] = [];
  //roleNames: string[] = [];
  //requiredTypes: number[] = [];
  procedures: string[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<RolesRequiredTraining[]>('https://localhost:7227/api/RolesRequiredTraining').subscribe(data => {
      this.rolesRequiredTraining = data; console.log(data)

      const procedureSet = new Set<string>();
      data.forEach(role => {
        Object.keys(role.trainingRequiredTypes).forEach(proc => procedureSet.add(proc));
      });

      this.procedures = Array.from(procedureSet).sort();

    }, error => console.log(error));
  }
}
