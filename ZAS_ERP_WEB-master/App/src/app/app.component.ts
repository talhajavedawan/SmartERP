import { Component, OnInit } from "@angular/core";
import {
  RouterOutlet,
  Router,
  NavigationEnd,
  ActivatedRoute,
} from "@angular/router";
import { CommonModule } from "@angular/common";
import { navItems } from "./Theme/layout/default-layout/_nav";
import { Title } from "@angular/platform-browser";
import { filter, map, mergeMap } from "rxjs/operators";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
  standalone: true,
  imports: [RouterOutlet, CommonModule],
})
export class AppComponent implements OnInit {
  // title = "employee-management";
  // public navItems = navItems;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title
  ) {}

  ngOnInit(): void {
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        map(() => {
          let route = this.activatedRoute;
          while (route.firstChild) {
            route = route.firstChild;
          }
          return route;
        }),
        mergeMap((route) => route.data)
      )
      .subscribe((data) => {
        const pageTitle = data["title"] ? data["title"] : "ZAS ERP";
        this.titleService.setTitle(pageTitle);
      });
  }
}
