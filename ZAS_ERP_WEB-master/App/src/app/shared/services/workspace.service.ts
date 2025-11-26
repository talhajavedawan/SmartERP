import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable } from "rxjs";

export interface WorkspaceTab {
  route: string;
  title: string;
}

@Injectable({
  providedIn: "root",
})
export class WorkspaceService {
  private tabsSubject = new BehaviorSubject<WorkspaceTab[]>([]);
  public tabs$: Observable<WorkspaceTab[]> = this.tabsSubject.asObservable();

  constructor(private router: Router) {}
  openTab(route: string, title: string): void {
    const normalizedRoute = route.split("?")[0];

    const currentTabs = this.tabsSubject.getValue();
    const existingTab = currentTabs.find((t) => t.route === normalizedRoute);

    if (!existingTab) {
      // Add new tab
      this.tabsSubject.next([
        ...currentTabs,
        { route: normalizedRoute, title },
      ]);
    }

    // Navigate to the route to show the content
    this.router.navigate([normalizedRoute]);
  }

  closeTab(route: string): void {
    const currentTabs = this.tabsSubject.getValue();
    const newTabs = currentTabs.filter((t) => t.route !== route);
    this.tabsSubject.next(newTabs);

    const wasActive = this.router.url.split("?")[0] === route;

    if (wasActive) {
      if (newTabs.length > 0) {
        // Navigate to the last opened tab
        this.router.navigate([newTabs[newTabs.length - 1].route]);
      } else {
        // Fallback to the dashboard if no tabs are left
        this.router.navigate(["/dashboard"]);
      }
    }
  }

  isTabActive(route: string): boolean {
    return this.router.url.split("?")[0] === route;
  }
}
