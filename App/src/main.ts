import { bootstrapApplication } from "@angular/platform-browser";
import { provideAnimations } from "@angular/platform-browser/animations";
import { AppComponent } from "./app/app.component";
import { appConfig } from "./app/app.config";
import { provideHttpClient } from "@angular/common/http";
import { en_US, provideNzI18n } from "ng-zorro-antd/i18n";
import { registerLocaleData } from "@angular/common";
import en from "@angular/common/locales/en";
import { FormsModule } from "@angular/forms";
import { importProvidersFrom } from "@angular/core";

// ✅ CoreUI services
import { SidebarService, SidebarNavHelper } from "@coreui/angular";

registerLocaleData(en);

bootstrapApplication(AppComponent, {
  providers: [
    // bring in everything from app.config.ts
    ...appConfig.providers,

    // extra providers (not included in appConfig)
    provideHttpClient(),
    provideNzI18n(en_US),
    importProvidersFrom(FormsModule),
    provideAnimations(),

    // ✅ Required CoreUI services
    SidebarService,
    SidebarNavHelper,
  ],
}).catch((err) => console.error(err));
