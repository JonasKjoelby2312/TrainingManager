import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminComplianceComponent } from './components/admin-compliance/admin-compliance.component';
import { EmployeesComponent } from './components/employees/employees.component';
import { ProcedureComponent } from './components/procedure/procedure.component';
import { AdminEmployeeComplianceComponent } from './components/admin-employee-compliance/admin-employee-compliance.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminComplianceComponent,
    EmployeesComponent,
    ProcedureComponent,
    AdminEmployeeComplianceComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
