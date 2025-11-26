import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmVendorNatureComponent } from './frm-vendor-nature.component';

describe('FrmVendorNatureComponent', () => {
  let component: FrmVendorNatureComponent;
  let fixture: ComponentFixture<FrmVendorNatureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmVendorNatureComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmVendorNatureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
