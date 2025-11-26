import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VendorService } from '../../../../shared/services/vendor/vendor.service';
import { VendorContactService } from '../../../../shared/services/vendor/vendor-contact.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-vendor-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './dtl-vendor.component.html',
  styleUrls: ['./dtl-vendor.component.css'],
})
export class DtlVendorComponent implements OnInit {
  vendorId!: number;
  vendor: any = null;
  contacts: any[] = [];
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private vendorService: VendorService,
    private vendorContactService: VendorContactService,
        private router: Router
  ) {}

  ngOnInit(): void {
    this.vendorId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadVendorData();
  }

  loadVendorData(): void {
  this.loading = true;

this.vendorService.getVendorById(this.vendorId).subscribe({
  next: (res) => {
    console.log("Vendor API Response:", res);
    this.vendor = res?.data ? res.data : res;
  },
  error: (err) => console.error("Vendor Load Error:", err)
});




  this.vendorContactService.getByVendor(this.vendorId).subscribe({
    next: (res: any) => {
      if (res?.data) {
        this.contacts = res.data;
      } else if (Array.isArray(res)) {
        this.contacts = res;
      } else {
        this.contacts = [];
      }

      this.loading = false;
    },
    error: (err) => {
      console.error("Contact Load Error:", err);
      this.contacts = [];
      this.loading = false;
    },
  });
}
onClose() {
  this.router.navigate(['/lst-vendor']); 
}


}

