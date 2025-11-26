import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmIndustriesComponent } from './frm-industries.component';

describe('FrmIndustriesComponent', () => {
  let component: FrmIndustriesComponent;
  let fixture: ComponentFixture<FrmIndustriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmIndustriesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmIndustriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
