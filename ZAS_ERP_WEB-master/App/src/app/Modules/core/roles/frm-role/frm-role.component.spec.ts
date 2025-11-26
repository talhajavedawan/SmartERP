import { ComponentFixture, TestBed } from '@angular/core/testing';

<<<<<<<< HEAD:App/src/app/Modules/core/roles/frm-role/frm-role.component.spec.ts
import { FrmRoleComponent } from './frm-role.component';

describe('FrmRoleComponent', () => {
  let component: FrmRoleComponent;
  let fixture: ComponentFixture<FrmRoleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmRoleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmRoleComponent);
========
import { FrmUserComponent } from './frm-user.component';

describe('FrmUserComponent', () => {
  let component: FrmUserComponent;
  let fixture: ComponentFixture<FrmUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmUserComponent);
>>>>>>>> remotes/origin/Amna:App/src/app/Modules/core/Users/frm-user/frm-user.component.spec.ts
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
