import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmEmployeeStatusComponent } from './frm-employee-status.component';

describe('FrmEmployeeStatusComponent', () => {
  let component: FrmEmployeeStatusComponent;
  let fixture: ComponentFixture<FrmEmployeeStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmEmployeeStatusComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmEmployeeStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
