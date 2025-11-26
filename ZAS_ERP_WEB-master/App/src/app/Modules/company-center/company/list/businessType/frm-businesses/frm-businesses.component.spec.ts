import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmBusinessesComponent } from './frm-businesses.component';

describe('FrmBusinessesComponent', () => {
  let component: FrmBusinessesComponent;
  let fixture: ComponentFixture<FrmBusinessesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmBusinessesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmBusinessesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
