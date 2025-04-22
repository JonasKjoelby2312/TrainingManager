import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolesRequiredTrainingComponent } from './roles-required-training.component';

describe('RolesRequiredTrainingComponent', () => {
  let component: RolesRequiredTrainingComponent;
  let fixture: ComponentFixture<RolesRequiredTrainingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RolesRequiredTrainingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolesRequiredTrainingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
