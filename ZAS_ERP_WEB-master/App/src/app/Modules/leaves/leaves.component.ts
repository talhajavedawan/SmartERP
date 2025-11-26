// import { Component, OnInit } from "@angular/core";
// import { Router, NavigationEnd } from "@angular/router";
// import { CommonModule } from "@angular/common";
// import { FormsModule } from "@angular/forms";
// import { RouterOutlet, RouterLink } from "@angular/router";
// import { filter } from "rxjs/operators";
// import { LeaveRequestService } from "../../shared/services/leaves/leave-request.service";
// import { LeaveRequest } from "../location/model/leaveapp.model";

// @Component({
//   selector: "app-leaves",
//   standalone: true,
//   templateUrl: "./leaves.component.html",
//   styleUrls: ["./leaves.component.css"],
//   imports: [CommonModule, FormsModule, RouterOutlet],
// })
// export class LeavesComponent implements OnInit {
//   /** 🔹 UI States */
//   isMainPage = true;
//   pageTitle = "Leave Applications";
//   isLoading = false;
//   searchText = "";

//   /** 🔹 Data */
//   leaves: LeaveRequest[] = [];
//   filteredLeaves: LeaveRequest[] = [];

//   constructor(
//     private router: Router,
//     private leaveService: LeaveRequestService
//   ) {}

//   // ----------------------------------------------------------
//   // ✅ Lifecycle Hook
//   // ----------------------------------------------------------
//   ngOnInit(): void {
//     this.loadLeaves();

//     // 🔹 Detect route changes (updates header + layout mode)
//     this.router.events
//       .pipe(
//         filter(
//           (event): event is NavigationEnd => event instanceof NavigationEnd
//         )
//       )
//       .subscribe((event) => this.updatePageMode(event.urlAfterRedirects));
//   }

//   // ----------------------------------------------------------
//   // ✅ ROUTING + NAVIGATION HANDLERS
//   // ----------------------------------------------------------

//   /** 🔹 Handles layout mode depending on current route */
//   private updatePageMode(url: string): void {
//     if (url === "/leaves" || url === "/leaves/") {
//       this.isMainPage = true;
//       this.pageTitle = "Leave Applications";
//     } else if (url.includes("/leaves/apply")) {
//       this.isMainPage = false;
//       this.pageTitle = "Apply for Leave";
//     } else if (url.includes("/leaves/balance")) {
//       this.isMainPage = false;
//       this.pageTitle = "Track Balance";
//     } else {
//       this.isMainPage = false;
//       this.pageTitle = "Leave Management";
//     }
//   }

//   /** 🔹 Go to child route (apply / balance) */
//   navigateTo(route: string): void {
//     console.log(`Navigating to /leaves/${route}`);
//     this.router.navigate(["/leaves", route]);
//   }

//   /** 🔹 Return to main list view */
//   goBack(): void {
//     console.log("Navigating back to /leaves");
//     this.router.navigate(["/leaves"]);
//   }

//   // ----------------------------------------------------------
//   // ✅ DATA OPERATIONS
//   // ----------------------------------------------------------

//   /** 🔹 Fetch all leaves for logged-in employee */
//   loadLeaves(): void {
//     this.isLoading = true;
//     const employeeId = 1; // TODO: Replace with dynamic logged-in user ID

//     this.leaveService.getByEmployee(employeeId).subscribe({
//       next: (data) => {
//         this.leaves = data || [];
//         this.filteredLeaves = [...this.leaves];
//         this.isLoading = false;
//         console.log("✅ Leaves loaded:", this.leaves);
//       },
//       error: (err) => {
//         console.error("❌ Error fetching leaves:", err);
//         this.isLoading = false;
//       },
//     });
//   }

//   /** 🔹 Search filter for table */
//   onSearch(): void {
//     const text = this.searchText.toLowerCase().trim();
//     if (!text) {
//       this.filteredLeaves = [...this.leaves];
//       return;
//     }

//     this.filteredLeaves = this.leaves.filter(
//       (l) =>
//         l.reason?.toLowerCase().includes(text) ||
//         l.leaveType?.name?.toLowerCase().includes(text) ||
//         l.status?.toLowerCase().includes(text)
//     );
//   }

//   /** 🔹 Reload data */
//   refreshList(): void {
//     console.log("🔄 Refreshing leave list...");
//     this.loadLeaves();
//   }
// }

import { Component, OnInit } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterOutlet } from "@angular/router";
import { filter } from "rxjs/operators";
import { LeaveRequestService } from "../../shared/services/leaves/leave-request.service";
import { LeaveRequest } from "../../shared/services/leaves/leave-request.service";

@Component({
  selector: "app-leaves",
  standalone: true,
  templateUrl: "./leaves.component.html",
  styleUrls: ["./leaves.component.css"],
  imports: [CommonModule, FormsModule, RouterOutlet],
})
export class LeavesComponent implements OnInit {
  isMainPage = true;
  pageTitle = "Leave Applications";
  isLoading = false;
  searchText = "";

  leaves: LeaveRequest[] = [];
  filteredLeaves: LeaveRequest[] = [];

  constructor(
    private router: Router,
    private leaveService: LeaveRequestService
  ) {}

  ngOnInit(): void {
    this.loadLeaves();
    this.router.events
      .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd))
      .subscribe((e) => this.updatePageMode(e.urlAfterRedirects));
  }
  getLeaveDays(
    start: string | Date,
    end: string | Date,
    isHalfDay: boolean
  ): number {
    if (isHalfDay) return 0.5;
    if (!start || !end) return 0;

    const startDate = new Date(start);
    const endDate = new Date(end);

    // include both start and end date → +1 day difference
    const diffMs = endDate.getTime() - startDate.getTime();
    const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24)) + 1;

    return diffDays;
  }

  private updatePageMode(url: string): void {
    if (url === "/leaves" || url === "/leaves/") {
      this.isMainPage = true;
      this.pageTitle = "Leave Applications";
    } else if (url.includes("/leaves/apply")) {
      this.isMainPage = false;
      this.pageTitle = "Apply for Leave";
    } else if (url.includes("/leaves/balance")) {
      this.isMainPage = false;
      this.pageTitle = "Track Balance";
    } else {
      this.isMainPage = false;
      this.pageTitle = "Leave Management";
    }
  }

  navigateTo(route: string): void {
    this.router.navigate(["/leaves", route]);
  }

  goBack(): void {
    this.router.navigate(["/leaves"]);
  }

  loadLeaves(): void {
    this.isLoading = true;
    const employeeId = 1; // TODO: dynamically get from auth

    this.leaveService.getByEmployee(employeeId).subscribe({
      next: (data) => {
        this.leaves = data || [];
        this.filteredLeaves = [...this.leaves];
        this.isLoading = false;
        console.log("✅ Leaves loaded:", this.leaves);
      },
      error: (err) => {
        console.error("❌ Error fetching leaves:", err);
        this.isLoading = false;
      },
    });
  }

  onSearch(): void {
    const text = this.searchText.toLowerCase().trim();
    this.filteredLeaves = this.leaves.filter(
      (l) =>
        l.leaveDescription?.toLowerCase().includes(text) ||
        l.leaveTypeName?.toLowerCase().includes(text) ||
        l.status?.toLowerCase().includes(text)
    );
  }

  refreshList(): void {
    this.loadLeaves();
  }
}
