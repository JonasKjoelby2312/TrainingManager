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
  isCreateRoleActive: boolean = false;

  // For procedure input and training selections for create procedure
  procedureInputValue: string = '';
  trainingSelectionsCreateProcedure: { [roleId: number]: number } = {};

  // For procedure input and training selections for create role
  roleInputValue: string = '';
  trainingSelectionsCreateRole: { [procedureName: string]: number } = {};

   isEditModalActive: boolean = false;
  selectedRole: RolesRequiredTraining | null = null;
  selectedProcedure: string | null = null;
  editValue: number = 0;

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

  openCreateRole() {
    this.isCreateRoleActive = true;
    console.log("open, ", this.isCreateRoleActive);
  }

  closeCreateRole() {
    this.isCreateRoleActive = false;
    console.log("close, ", this.isCreateRoleActive);
    this.trainingSelectionsCreateRole = {};
    this.roleInputValue = '';
  }

  submitCreateRole(roleName: string) {
    console.log('Creating Role:', roleName);
    console.log('Selected Procedure Required Types:', this.trainingSelectionsCreateRole);

    const requestBody = {
      id: -1,
      name: roleName,
      isActive: true,
      proceduresRequiredTypes: this.trainingSelectionsCreateRole
    };

    this.http.post('https://localhost:7227/api/Roles', requestBody)
      .subscribe(
        response => {
          console.log('Role created successfully', response);
          // Optionally refresh or update UI
        },
        error => {
          console.error('Error creating Role', error);
        }
      );

    // Reset UI
    this.isCreateProcedureActive = false;
    this.trainingSelectionsCreateProcedure = {};
    this.procedureInputValue = '';
    this.closeCreateRole();
    this.ngOnInit();
  }

  // Show modal
  openCreateProcedure() {
    this.isCreateProcedureActive = true;
    console.log('openCreateProcedure called');
    console.log(this.isCreateProcedureActive);
  }

  // Cancel modal
  onCancelProcedure() {
    console.log("Just got cancelled!");
    this.isCreateProcedureActive = false;
    this.trainingSelectionsCreateProcedure = {};
    this.procedureInputValue = '';
  }

  // Handle radio button change on create procedure
  setRoleSelection(roleId: number, value: number) {
    this.trainingSelectionsCreateProcedure[roleId] = value;
  }

  // Handle radio button change on create role
  setProcedureSelection(procedureName: string, value: number) {
    this.trainingSelectionsCreateRole[procedureName] = value;
  }

  // Handle create
  onCreateProcedure(procedureName: string) {
    console.log('Creating Procedure:', procedureName);
    console.log('Selected Role Trainings:', this.trainingSelectionsCreateProcedure);

    const requestBody = {
      procedureName: procedureName,
      revisionNumber: 1.0,
      isActive: true, // or false depending on your app logic
      historyText: 'Initial creation', // You can replace or prompt for this
      rolesRequiredTrainingList: Object.entries(this.trainingSelectionsCreateProcedure).map(
        ([roleId, requiredType]) => ({
          roleId: Number(roleId),
          requiredType: requiredType
        })
      )
    };

    this.http.post('https://localhost:7227/api/ProcedureOverview', requestBody)
      .subscribe(
        response => {
          console.log('Procedure created successfully', response);
          // Optionally refresh or update UI
        },
        error => {
          console.error('Error creating procedure', error);
        }
      );

    // Reset UI
    this.isCreateProcedureActive = false;
    this.trainingSelectionsCreateProcedure = {};
    this.procedureInputValue = '';

    
  }

  //EDIT MODAL
  openEditModal(role: RolesRequiredTraining, procedureName: string) {
    this.selectedRole = role;
    this.selectedProcedure = procedureName;
    this.editValue = role.trainingRequiredTypes[procedureName] ?? 0;
    this.isEditModalActive = true;
  }

  closeEditModal() {
    this.selectedRole = null;
    this.selectedProcedure = null;
    this.editValue = 0;
    this.isEditModalActive = false;
  }

  confirmEdit() {
    if (this.selectedRole && this.selectedProcedure) {
      const updatedValue = this.editValue;

     
      this.selectedRole.trainingRequiredTypes[this.selectedProcedure] = updatedValue;

      
      const requestBody = {
        roleId: this.selectedRole.roleId,
        procedureName: this.selectedProcedure,
        requiredType: updatedValue
      };

      
      this.http.put('https://localhost:7227/api/RolesRequiredTraining', requestBody)
        .subscribe({
          next: () => {
            console.log('Training requirement updated');
          },
          error: (err) => {
            console.error('Failed to update requirement:', err);
          }
        });

      
      this.closeEditModal();
    }
  }

}
