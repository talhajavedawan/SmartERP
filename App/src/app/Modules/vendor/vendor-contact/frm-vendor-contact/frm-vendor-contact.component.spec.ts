import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmVendorContactComponent } from './frm-vendor-contact.component';

describe('FrmVendorContactComponent', () => {
  let component: FrmVendorContactComponent;
  let fixture: ComponentFixture<FrmVendorContactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmVendorContactComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmVendorContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
