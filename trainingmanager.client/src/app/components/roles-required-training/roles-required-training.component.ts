import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface RolesRequiredTraining {
  roleId: number;
  roleName: string;
  trainingRequiredTypes: {
    [procedureName: string]: number;
  };
}

@Component({
  selector: 'app-roles-required-training',
  standalone: false,
  templateUrl: './roles-required-training.component.html',
  styleUrl: './roles-required-training.component.css'
})
export class RolesRequiredTrainingComponent {
  rolesRequiredTraining: RolesRequiredTraining[] = [];
  procedures: string[] = [];
  isCreateProcedureActive: boolean = false;

  // For procedure input and training selections
  procedureInputValue: string = '';
  trainingSelections: { [roleId: number]: number } = {};

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<RolesRequiredTraining[]>('https://localhost:7227/api/RolesRequiredTraining').subscribe(
      data => {
        this.rolesRequiredTraining = data;
        console.log(data);

        const procedureSet = new Set<string>();
        data.forEach(role => {
          Object.keys(role.trainingRequiredTypes).forEach(proc => procedureSet.add(proc));
        });

        this.procedures = Array.from(procedureSet).sort();
      },
      error => console.log(error)
    );
  }

  // Show modal
  openCreateProcedure() {
    this.isCreateProcedureActive = true;
    console.log('openCreateProcedure called');
    console.log(this.isCreateProcedureActive);
  }

  // Cancel modal
  onCancelProcedure() {
    this.isCreateProcedureActive = false;
    this.trainingSelections = {};
    this.procedureInputValue = '';
  }

  // Handle radio button change
  setRoleSelection(roleId: number, value: number) {
    this.trainingSelections[roleId] = value;
  }

  // Handle create
  onCreateProcedure(procedureName: string) {
    console.log('Creating Procedure:', procedureName);
    console.log('Selected Role Trainings:', this.trainingSelections);

    // Add POST logic here to send data to the API if needed

    this.isCreateProcedureActive = false;
    this.trainingSelections = {};
    this.procedureInputValue = '';
  }
}
