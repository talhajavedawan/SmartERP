import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LstVendorNatureComponent } from './lst-vendor-nature.component';

describe('LstVendorNatureComponent', () => {
  let component: LstVendorNatureComponent;
  let fixture: ComponentFixture<LstVendorNatureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LstVendorNatureComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LstVendorNatureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
