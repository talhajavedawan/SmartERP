import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmGroupComponent } from './frm-group.component';

describe('FrmGroupComponent', () => {
  let component: FrmGroupComponent;
  let fixture: ComponentFixture<FrmGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmGroupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
