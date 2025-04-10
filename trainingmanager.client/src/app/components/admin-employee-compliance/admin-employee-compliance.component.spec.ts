import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminEmployeeComplianceComponent } from './admin-employee-compliance.component';

describe('AdminEmployeeComplianceComponent', () => {
  let component: AdminEmployeeComplianceComponent;
  let fixture: ComponentFixture<AdminEmployeeComplianceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminEmployeeComplianceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminEmployeeComplianceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
