import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComplianceComponent } from './components/admin-compliance/admin-compliance.component';
import { EmployeesComponent } from './components/employees/employees.component';
import { ProcedureComponent } from './components/procedure/procedure.component';
import { AdminEmployeeComplianceComponent } from './components/admin-employee-compliance/admin-employee-compliance.component';

const routes: Routes = [{ path: 'admin-compliance', component: AdminComplianceComponent },
  { path: 'employees', component: EmployeesComponent }, { path: '', component: AdminComplianceComponent, pathMatch: 'full' },
  { path: 'procedures', component: ProcedureComponent }, { path: 'admin-employee', component: AdminEmployeeComplianceComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
