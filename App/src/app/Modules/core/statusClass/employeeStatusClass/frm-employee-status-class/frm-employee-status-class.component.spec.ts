import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmEmployeeStatusClassComponent } from './frm-employee-status-class.component';

describe('FrmEmployeeStatusClassComponent', () => {
  let component: FrmEmployeeStatusClassComponent;
  let fixture: ComponentFixture<FrmEmployeeStatusClassComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmEmployeeStatusClassComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmEmployeeStatusClassComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
