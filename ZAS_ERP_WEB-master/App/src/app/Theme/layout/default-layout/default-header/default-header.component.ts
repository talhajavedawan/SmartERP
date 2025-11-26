import { NgClass, NgTemplateOutlet } from "@angular/common";
import {
  Component,
  computed,
  inject,
  input,
  OnInit,
  OnDestroy,
} from "@angular/core";
import { NavigationEnd, Router } from "@angular/router";
import { NzMenuModule } from "ng-zorro-antd/menu";
import {
  ColorModeService,
  ContainerComponent,
  DropdownComponent,
  DropdownItemDirective,
  DropdownMenuDirective,
  DropdownToggleDirective,
  HeaderComponent,
  HeaderNavComponent,
  HeaderTogglerDirective,
  NavLinkDirective,
  SidebarToggleDirective,
} from "@coreui/angular";
import { IconDirective } from "@coreui/icons-angular";
import { AuthService } from "../../../../shared/services/authentication/auth.service";
import { filter } from "rxjs/operators";
import { Subscription } from "rxjs";
import { WorkspaceService } from "../../../../shared/services/workspace.service";
import { UserService } from "../../../../shared/services/User.service";
import { EmployeeService } from "../../../../shared/services/employee.service";
import { ProfilePictureService } from "../../../../shared/services/profile-picture.service";
import { NotificationDropdownComponent } from "../../../../shared/components/notification-dropdown/notification-dropdown.component";
@Component({
  selector: "app-default-header",
  templateUrl: "./default-header.component.html",
  imports: [
    ContainerComponent,
    HeaderTogglerDirective,
    SidebarToggleDirective,
    IconDirective,
    HeaderNavComponent,
    NavLinkDirective,
    NgTemplateOutlet,
    DropdownComponent,
    DropdownToggleDirective,
    DropdownMenuDirective,
    DropdownItemDirective,
    NzMenuModule,
    NotificationDropdownComponent,
  ],
  standalone: true,
})
export class DefaultHeaderComponent
  extends HeaderComponent
  implements OnInit, OnDestroy
{
  readonly #colorModeService = inject(ColorModeService);
  readonly colorMode = this.#colorModeService.colorMode;

  readonly colorModes = [
    { name: "light", text: "Light", icon: "cilSun" },
    { name: "dark", text: "Dark", icon: "cilMoon" },
    { name: "auto", text: "Auto", icon: "cilContrast" },
  ];

  readonly icons = computed(() => {
    const currentMode = this.colorMode();
    return (
      this.colorModes.find((mode) => mode.name === currentMode)?.icon ??
      "cilSun"
    );
  });

  private employeeService = inject(EmployeeService);
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private profilePictureService = inject(ProfilePictureService);
  private openWindows: Map<string, Window> = new Map();
  public lastOpenedRoute: string | null = null;

  currentUser: any = null;
  loadingUser = false;
  profilePictureUrl: string | null = null;
  private userSubscription: Subscription | null = null;
  private profilePictureSubscription: Subscription | null = null;

  private readonly WINDOW_FEATURES =
    "menubar=no,location=no,toolbar=no,scrollbars=yes,status=no,resizable=yes,width=1200,height=800";

  constructor(
    private router: Router,
    private workspaceService: WorkspaceService
  ) {
    super();
  }

  ngOnInit(): void {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.lastOpenedRoute = event.urlAfterRedirects;
      });

    setInterval(() => this.cleanupClosedWindows(), 5000);

    // Listen for profile picture updates
    this.profilePictureSubscription =
      this.profilePictureService.profilePicture$.subscribe((newUrl) => {
        if (newUrl) {
          console.log("🔄 Header received updated profile picture:", newUrl);
          this.profilePictureUrl = newUrl;
        }
      });

    this.loadCurrentUserAndProfile();
  }

  ngOnDestroy(): void {
    this.userSubscription?.unsubscribe();
    this.profilePictureSubscription?.unsubscribe();
  }
  private loadCurrentUserAndProfile(): void {
    this.loadingUser = true;

    const isPowerUser = localStorage.getItem("isPowerUser") === "true";

    if (isPowerUser) {
      console.log("🟣 Power User detected — skipping backend fetch");
      this.setPowerUserProfile();
      return;
    }

    console.log("🔵 Loading regular user...");
    this.userSubscription = this.userService.getCurrentUser().subscribe({
      next: (user) => {
        console.log("✅ User data loaded:", user);

        // ✅ If backend returns Administrator user, treat it as Power User
        if (
          user?.userName?.toLowerCase() === "administrator" ||
          user?.email?.toLowerCase().includes("administrator")
        ) {
          console.log(
            "🟣 Backend Administrator detected — using default avatar only"
          );
          this.setPowerUserProfile();
          return;
        }

        if (user && user.userName) {
          this.currentUser = user;
          const employeeId = user.employeeId ?? user.id;
          console.log("👤 Using employeeId:", employeeId);
          this.loadEmployeeProfilePicture(employeeId);
        } else {
          console.log("❌ Invalid user data, using fallback");
          this.fallbackUser();
        }
      },
      error: (err) => {
        console.error("❌ Error loading current user:", err);
        this.fallbackUser();
      },
    });
  }

  private loadEmployeeProfilePicture(employeeId: number): void {
    console.log("🖼️ Loading employee profile picture for ID:", employeeId);

    // 🧠 Safety check: don't try to load picture for Administrator
    if (this.currentUser?.userName?.toLowerCase() === "administrator") {
      console.log("🚫 Skipping profile picture load for Administrator");
      this.profilePictureUrl = this.getDefaultAvatar();
      this.profilePictureService.updateProfilePicture(this.getDefaultAvatar());
      this.loadingUser = false;
      return;
    }

    this.employeeService.getEmployeeById(employeeId).subscribe({
      next: (employee) => {
        console.log("✅ Employee data loaded:", employee);
        if (employee && employee.id) {
          const newUrl = this.employeeService.getProfilePictureUrl(employee.id);
          console.log("📸 Profile picture URL generated:", newUrl);
          this.profilePictureUrl = newUrl;
          this.profilePictureService.updateProfilePicture(newUrl);

          const hasProfilePicture =
            employee.profilePicture ||
            employee.profilePictureUrl ||
            employee.profilePictureFileName;

          if (!hasProfilePicture) {
            console.log("📭 No profile picture data, using default avatar");
            this.profilePictureUrl = this.getDefaultAvatar();
            this.profilePictureService.updateProfilePicture(
              this.getDefaultAvatar()
            );
          }
        } else {
          console.log("❌ Invalid employee data, using default avatar");
          this.profilePictureUrl = this.getDefaultAvatar();
          this.profilePictureService.updateProfilePicture(
            this.getDefaultAvatar()
          );
        }
        this.loadingUser = false;
      },
      error: (err) => {
        console.warn("⚠️ Failed to load employee profile picture:", err);
        this.profilePictureUrl = this.getDefaultAvatar();
        this.profilePictureService.updateProfilePicture(
          this.getDefaultAvatar()
        );
        this.loadingUser = false;
      },
    });
  }

  /** 💡 Helper for Power User setup */
  private setPowerUserProfile(): void {
    this.currentUser = {
      userName: "Administrator",
      email: "Administrator2244@gmail.com",
    };
    this.profilePictureUrl = this.getDefaultAvatar();
    this.profilePictureService.updateProfilePicture(this.getDefaultAvatar());
    this.loadingUser = false;
  }

  /** ────────────────────────────────────────────────
   * 🧩 Utility Helpers
   * ──────────────────────────────────────────────── */
  private fallbackUser(): void {
    this.currentUser = {
      userName: "Administrator",
      email: "Administrator2244@gmail.com",
    };
    this.profilePictureUrl = this.getDefaultAvatar();
    this.profilePictureService.updateProfilePicture(this.getDefaultAvatar());
  }

  private getDefaultAvatar(): string {
    return "https://cdn-icons-png.flaticon.com/512/149/149071.png";
  }

  onImageError(event: any): void {
    console.log("🖼️ Image load error, using default avatar");
    event.target.src = this.getDefaultAvatar();
    this.profilePictureUrl = this.getDefaultAvatar();
    this.profilePictureService.updateProfilePicture(this.getDefaultAvatar());
  }

  refreshProfilePicture(): void {
    if (this.currentUser?.employeeId) {
      const newUrl = this.employeeService.getProfilePictureUrl(
        this.currentUser.employeeId
      );
      this.profilePictureUrl = newUrl;
      this.profilePictureService.updateProfilePicture(newUrl);
    }
  }

  /** ────────────────────────────────────────────────
   * 🔐 Logout + Navigation + Windows
   * ──────────────────────────────────────────────── */
  sidebarId = input("sidebar1");

  public newMessages = [];
  public newNotifications = [];
  public newStatus = [];
  public newTasks = [];

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(["/login"]);

    this.openWindows.forEach((win) => {
      if (!win.closed) win.close();
    });
    this.openWindows.clear();
  }

  isRouteActiveExact(route: string): boolean {
    if (!this.lastOpenedRoute) return false;
    const currentRoute = this.lastOpenedRoute.split("?")[0];
    const baseRoute = route.split("?")[0];
    return currentRoute === baseRoute;
  }

  openFormInTab(route: string, title: string): void {
    this.workspaceService.openTab(route, title);
  }

  openInNewWindowWithFeatures(route: string, event: MouseEvent): void {
    event.preventDefault();

    const url = this.router.serializeUrl(this.router.createUrlTree([route]));
    const fullUrl = window.location.origin + url;
    const windowName = `app_aux_window_${Date.now()}`;

    const newWindow = window.open(fullUrl, windowName, this.WINDOW_FEATURES);
    if (newWindow) {
      newWindow.focus();
      this.openWindows.set(windowName, newWindow);
    }
  }

  openInNewTab(route: string, event: MouseEvent): void {
    event.preventDefault();

    const url = this.router.serializeUrl(this.router.createUrlTree([route]));
    const fullUrl = window.location.origin + url;
    const windowName = `app_tab_${route.replace(/\//g, "_")}_${Date.now()}`;

    const newWindow = window.open(fullUrl, windowName, this.WINDOW_FEATURES);
    if (newWindow) {
      this.openWindows.set(windowName, newWindow);
    }
  }

  private cleanupClosedWindows(): void {
    for (const [windowName, windowRef] of this.openWindows.entries()) {
      if (windowRef.closed) {
        this.openWindows.delete(windowName);
      }
    }
  }
}
