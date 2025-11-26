import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LstEmployeeStatusComponent } from './lst-employee-status.component';

describe('LstEmployeeStatusComponent', () => {
  let component: LstEmployeeStatusComponent;
  let fixture: ComponentFixture<LstEmployeeStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LstEmployeeStatusComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LstEmployeeStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
