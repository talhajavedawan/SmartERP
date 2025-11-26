import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LstVendorContactComponent } from './lst-vendor-contact.component';

describe('LstVendorContactComponent', () => {
  let component: LstVendorContactComponent;
  let fixture: ComponentFixture<LstVendorContactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LstVendorContactComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LstVendorContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
