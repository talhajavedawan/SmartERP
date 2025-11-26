import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LstCustomerComponent } from './lst-customer.component';

describe('LstCustomerComponent', () => {
  let component: LstCustomerComponent;
  let fixture: ComponentFixture<LstCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LstCustomerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LstCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
