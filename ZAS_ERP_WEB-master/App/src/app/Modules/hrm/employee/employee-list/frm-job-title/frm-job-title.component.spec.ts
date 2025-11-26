import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrmJobTitleComponent } from './frm-job-title.component';

describe('FrmJobTitleComponent', () => {
  let component: FrmJobTitleComponent;
  let fixture: ComponentFixture<FrmJobTitleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FrmJobTitleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FrmJobTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
