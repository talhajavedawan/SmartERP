import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmCityComponent } from './frm-city.component';

describe('FrmCityComponent', () => {
  let component: FrmCityComponent;
  let fixture: ComponentFixture<FrmCityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmCityComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmCityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
