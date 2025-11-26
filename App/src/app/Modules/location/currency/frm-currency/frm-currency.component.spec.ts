import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmCurrencyComponent } from './frm-currency.component';

describe('FrmCurrencyComponent', () => {
  let component: FrmCurrencyComponent;
  let fixture: ComponentFixture<FrmCurrencyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmCurrencyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmCurrencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
