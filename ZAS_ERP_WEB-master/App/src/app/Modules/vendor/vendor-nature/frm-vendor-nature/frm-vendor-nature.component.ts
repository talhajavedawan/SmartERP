import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { VendorNatureService } from '../../../../shared/services/vendor/vendor-nature.service';
import { VendorNature } from '../vendor-nature.model';
import { ActivatedRoute, Router } from '@angular/router'; // Import ActivatedRoute to access route params
import { CommonModule } from '@angular/common';
import { NzMessageService } from 'ng-zorro-antd/message'; // Add for error messaging

@Component({
  selector: 'app-frm-vendor-nature',
  templateUrl: './frm-vendor-nature.component.html',
  styleUrls: ['./frm-vendor-nature.component.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class FrmVendorNatureComponent implements OnInit {
  vendorNatureForm: FormGroup;
  isEditMode: boolean = false;
  vendorNatureId: number | null = null;
 isLoading: boolean = false;
  constructor(
    private fb: FormBuilder,
    private vendorNatureService: VendorNatureService,
    private route: ActivatedRoute, // To fetch query parameters
    private router: Router, // For navigation
    private message: NzMessageService // For displaying error messages
  ) {
    this.vendorNatureForm = this.fb.group({
      id: [null],
      name: ['', [Validators.required, Validators.maxLength(150)]],
      isActive: [true],
    });
  }

  ngOnInit(): void {
    // Read the vendorNatureId from the URL parameters (if it's in edit mode)
    this.route.queryParams.subscribe(params => {
      this.vendorNatureId = params['id'] ? +params['id'] : null; // Parse to number
      if (this.vendorNatureId) {
        this.isEditMode = true;
        this.getVendorNatureById(this.vendorNatureId);
      }
    });
  }

  // Fetch existing Vendor Nature by ID (for editing)
  getVendorNatureById(id: number): void {
    this.vendorNatureService.getVendorNatureById(id).subscribe(
      (data: VendorNature) => {
        this.vendorNatureForm.patchValue(data); // Fill the form with existing data
      },
      (error) => {
        console.error('Error fetching vendor nature', error);
        this.message.error('Failed to fetch vendor nature details.');
      }
    );
  }

// Handle form submission (create or update)
onSubmit(): void {
  if (this.vendorNatureForm.valid) {
    const formData = this.vendorNatureForm.value;

    // Show the loading spinner
    this.isLoading = true;

    if (this.isEditMode) {
      if (!formData.id) {
        // Handle case where ID is missing
        console.error('ID is missing');
        return;
      }
      // Update the Vendor Nature
      this.vendorNatureService.updateVendorNature(formData.id, formData).subscribe(
        (response) => {
          console.log('Vendor Nature updated successfully', response);
          this.isLoading = false;
          // Show success message
          this.message.success('Vendor Nature updated successfully!');
          // Redirect to list page
          this.router.navigate(['/lst-vendor-nature']);
        },
        (error) => {
          this.isLoading = false;
          console.error('Error updating vendor nature', error);
          this.message.error('Error updating Vendor Nature!');
        }
      );
    } else {
      // Ensure the 'id' is not included in the create request
      delete formData.id; // Avoid sending id during create

      // Create new Vendor Nature
      this.vendorNatureService.createVendorNature(formData).subscribe(
        (response) => {
          console.log('Vendor Nature created successfully', response);
          this.isLoading = false;
          // Show success message
          this.message.success('Vendor Nature created successfully!');
          // Redirect to list page
          this.router.navigate(['/lst-vendor-nature']);
        },
        (error) => {
          this.isLoading = false;
          console.error('Error creating vendor nature', error);
          this.message.error('Error creating Vendor Nature!');
        }
      );
    }
  }
}


  // Reset form for next entry
  onReset(): void {
    this.vendorNatureForm.reset();
    this.isEditMode = false;
    // Optionally set default values if needed
    this.vendorNatureForm.patchValue({
      isActive: true // Default active value
    });
  }
    onCancel(): void {
    this.router.navigate(["/lst-vendor-nature"])
  }
}
