import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmCountryComponent } from './frm-country.component';

describe('FrmCountryComponent', () => {
  let component: FrmCountryComponent;
  let fixture: ComponentFixture<FrmCountryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmCountryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmCountryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
