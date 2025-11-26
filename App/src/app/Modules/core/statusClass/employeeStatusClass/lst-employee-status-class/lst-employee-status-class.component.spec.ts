import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LstEmployeeStatusClassComponent } from './lst-employee-status-class.component';

describe('LstEmployeeStatusClassComponent', () => {
  let component: LstEmployeeStatusClassComponent;
  let fixture: ComponentFixture<LstEmployeeStatusClassComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LstEmployeeStatusClassComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LstEmployeeStatusClassComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
