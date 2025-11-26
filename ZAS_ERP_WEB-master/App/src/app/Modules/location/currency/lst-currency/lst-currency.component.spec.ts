import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LstCurrencyComponent } from './lst-currency.component';

describe('LstCurrencyComponent', () => {
  let component: LstCurrencyComponent;
  let fixture: ComponentFixture<LstCurrencyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LstCurrencyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LstCurrencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
