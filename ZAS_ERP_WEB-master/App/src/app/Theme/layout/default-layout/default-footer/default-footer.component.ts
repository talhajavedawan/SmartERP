import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { FooterComponent } from "@coreui/angular";
import {
  WorkspaceService,
  WorkspaceTab,
} from "../../../../shared/services/workspace.service"; // Adjusted path from user input
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { AsyncPipe, NgFor, NgIf } from "@angular/common";

@Component({
  selector: "app-default-footer",
  templateUrl: "./default-footer.component.html",
  styleUrls: ["./default-footer.component.scss"],
  standalone: true,
  imports: [AsyncPipe, NgFor, NgIf],
})
export class DefaultFooterComponent
  extends FooterComponent
  implements AfterViewInit
{
  public tabs$: Observable<WorkspaceTab[]>;

  // ViewChild to access the scrollable element in the template
  @ViewChild("tabListScrollContainer")
  tabListContainer!: ElementRef<HTMLDivElement>;

  constructor(
    public workspaceService: WorkspaceService,
    public router: Router
  ) {
    super();
    this.tabs$ = this.workspaceService.tabs$;
  }

  ngAfterViewInit(): void {
    // ElementRef is available here if needed for initial checks
  }

  scrollTabs(direction: "left" | "right"): void {
    if (this.tabListContainer) {
      const container = this.tabListContainer.nativeElement;
      const scrollAmount = 150; // Scroll distance in pixels

      if (direction === "left") {
        container.scrollLeft -= scrollAmount;
      } else {
        container.scrollLeft += scrollAmount;
      }
    }
  }
}
