import { Component } from "@angular/core";
import { RouterLink, RouterOutlet } from "@angular/router";
import { NgScrollbar } from "ngx-scrollbar";

import { IconDirective, IconSetService } from "@coreui/icons-angular";
import {
  ContainerComponent,
  SidebarBrandComponent,
  SidebarComponent,
  SidebarFooterComponent,
  SidebarHeaderComponent,
  SidebarNavComponent,
  SidebarToggleDirective,
  ShadowOnScrollDirective,
} from "@coreui/angular";

import { DefaultFooterComponent, DefaultHeaderComponent } from "./";
import { navItems } from "./_nav";

// ✅ import the CorUI icon
import {
  cilMenu,
  cilBell,
  cilList,
  cilEnvelopeOpen,
  cilTask,
  cilCommentSquare,
  cilUser,
  cilSettings,
  cilCreditCard,
  cilFile,
  cilLockLocked,
  cilAccountLogout,
  cilUserFollow,
  cilUserUnfollow,
  cilSun,
  cilMoon,
  cilContrast,
  cilChartPie,
  cilBasket,
  cilSpeedometer,
  cilApps,
} from "@coreui/icons";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html",
  styleUrls: ["./default-layout.component.scss"],
  standalone: true,
  imports: [
    SidebarComponent,
    SidebarHeaderComponent,
    SidebarBrandComponent,
    SidebarNavComponent,
    SidebarFooterComponent,
    SidebarToggleDirective,
    ShadowOnScrollDirective,
    ContainerComponent,
    DefaultFooterComponent,
    DefaultHeaderComponent,
    IconDirective,
    NgScrollbar,
    RouterOutlet,
    RouterLink,
  ],
})
export class DefaultLayoutComponent {
  public navItems = [...navItems];

  constructor(private iconSet: IconSetService) {
    // ✅ register icons here for the whole layout
    this.iconSet.icons = {
      cilMenu,
      cilBell,
      cilList,
      cilEnvelopeOpen,
      cilTask,
      cilCommentSquare,
      cilUser,
      cilSettings,
      cilCreditCard,
      cilFile,
      cilLockLocked,
      cilAccountLogout,
      cilUserFollow,
      cilUserUnfollow,
      cilSun,
      cilMoon,
      cilContrast,
      cilChartPie,
      cilBasket,
      cilApps,
      cilSpeedometer,
    };
  }
}
