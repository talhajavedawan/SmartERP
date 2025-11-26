import {
  ApplicationConfig,
  importProvidersFrom,
  provideZoneChangeDetection,
} from "@angular/core";
import { provideRouter } from "@angular/router";
import { routes } from "./app.routes";

import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { AgGridModule } from "ag-grid-angular";
import { ModuleRegistry, AllCommunityModule } from "ag-grid-community";

import { IconSetService } from "@coreui/icons-angular";
import { authInterceptor } from "./shared/services/authentication/auth.interceptor";
import { JwtHelperService, JWT_OPTIONS } from "@auth0/angular-jwt";

import { provideAnimations } from "@angular/platform-browser/animations";
import { provideToastr } from "ngx-toastr";
import { iconSubset } from "./Theme/icons/icon-subset";

// ✅ Register AG Grid modules once
ModuleRegistry.registerModules([AllCommunityModule]);

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),

    // ✅ HTTP Client + Interceptor
    provideHttpClient(withInterceptors([authInterceptor])),

    // ✅ Router
    provideRouter(routes),

    // ✅ AG Grid (needed as standalone import)
    importProvidersFrom(AgGridModule),

    // ✅ Animations & Toastr
    provideAnimations(),
    provideToastr({
      timeOut: 3000,
      positionClass: "toast-bottom-right",
      preventDuplicates: true,
    }),

    // ✅ Register CoreUI Icons globally
    {
      provide: IconSetService,
      useFactory: () => {
        const icons = new IconSetService();
        icons.icons = iconSubset;
        return icons;
      },
    },

    // ✅ JWT Configuration
    {
      provide: JWT_OPTIONS,
      useValue: {
        tokenGetter: () => {
          return localStorage.getItem('access_token');
        },
        allowedDomains: ['localhost:5000', 'localhost:5001'], // Add your API domains
        disallowedRoutes: ['localhost:5000/api/auth/login', 'localhost:5001/api/auth/login'],
      }
    },
    JwtHelperService,
  ],
};
