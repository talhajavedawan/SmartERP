import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmDepartmentComponent } from './frm-department.component';

describe('FrmDepartmentComponent', () => {
  let component: FrmDepartmentComponent;
  let fixture: ComponentFixture<FrmDepartmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmDepartmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
