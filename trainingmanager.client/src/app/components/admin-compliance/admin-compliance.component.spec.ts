import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminComplianceComponent } from './admin-compliance.component';

describe('AdminComplianceComponent', () => {
  let component: AdminComplianceComponent;
  let fixture: ComponentFixture<AdminComplianceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminComplianceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminComplianceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
