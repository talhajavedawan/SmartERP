import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmVendorComponent } from './frm-vendor.component';

describe('FrmVendorComponent', () => {
  let component: FrmVendorComponent;
  let fixture: ComponentFixture<FrmVendorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmVendorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
//ts