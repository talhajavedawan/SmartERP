import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LstJobTitleComponent } from './lst-job-title.component';

describe('LstJobTitleComponent', () => {
  let component: LstJobTitleComponent;
  let fixture: ComponentFixture<LstJobTitleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LstJobTitleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LstJobTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
