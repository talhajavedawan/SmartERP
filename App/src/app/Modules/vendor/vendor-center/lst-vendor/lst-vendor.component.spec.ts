import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LstVendorComponent } from './lst-vendor.component';

describe('LstVendorComponent', () => {
  let component: LstVendorComponent;
  let fixture: ComponentFixture<LstVendorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LstVendorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LstVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
// ts