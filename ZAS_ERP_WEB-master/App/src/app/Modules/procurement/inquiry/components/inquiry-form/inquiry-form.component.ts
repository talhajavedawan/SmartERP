import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { InquiryGridComponent } from '../inquiry-grid/inquiry-grid.component';

@Component({
  selector: 'app-inquiry-form',
  templateUrl: './inquiry-form.component.html',
  styleUrls: ['./inquiry-form.component.scss', '../../../../../../scss/forms.css'],
  imports: [CommonModule, NzTabsModule, InquiryGridComponent],
  standalone: true,
})
export class InquiryFormComponent implements OnInit {
  // Component properties for UI display only (no functionality)
  selectedTabIndex = 0;
  isLoading = false;

  // Dummy data arrays for dropdowns (design only)
  inquiryTemplates: any[] = [];
  companies: any[] = [];
  departments: any[] = [];
  employees: any[] = [];
  transactionHolders: any[] = [];
  customers: any[] = [];
  inquiryStatuses: any[] = [];
  items: any[] = [];
  unitOfMeasures: any[] = [];

  // Grid data placeholder
  inquiryItems: any[] = [];

  constructor() {}

  ngOnInit(): void {
    // No initialization logic - design only
  }

  // Placeholder methods (non-functional)
  closeModal(): void {
    // Design only
  }

  saveInquiry(): void {
    // Design only
  }
}
