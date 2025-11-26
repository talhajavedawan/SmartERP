import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DtlVendorComponent } from './dtl-vendor.component';

describe('VendorDetailComponent', () => {
  let component: DtlVendorComponent;
  let fixture: ComponentFixture<DtlVendorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DtlVendorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DtlVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
