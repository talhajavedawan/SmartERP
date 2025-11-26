import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmCustomerComponent } from './frm-customer.component';

describe('FrmCustomerComponent', () => {
  let component: FrmCustomerComponent;
  let fixture: ComponentFixture<FrmCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmCustomerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
