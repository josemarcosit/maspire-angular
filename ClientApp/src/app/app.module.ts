import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
// translation
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { VehicleFormComponent } from './vehicle/vehicle-form/vehicle-form.component';
import { VehicleService } from './services/vehicle.service';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppErrorHandler } from './app.error-handler';
import { VehicleComponent } from './vehicle/vehicle/vehicle.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PaginationComponent } from './shared/pagination/pagination.component';
import { ViewVehicleComponent } from './vehicle/view-vehicle/view-vehicle.component';
import { LoginComponent } from './auth/login.component';
import { SignupComponent } from './auth/signup.component';
import { AuthInterceptor } from './services/auth.interceptor';
import { LanguageHeaderInterceptor } from './services/language-header.interceptor';
import { AuthGuard } from './services/auth.guard';
import { LanguageSelectorComponent } from './shared/components/language-selector/language-selector.component';

export function HttpLoaderFactory(http: HttpClient) {
  // use absolute path to avoid problems when Angular is hosted under a subfolder
  return new TranslateHttpLoader(http, '/assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    VehicleFormComponent,
    VehicleComponent,
    ViewVehicleComponent,
    PaginationComponent,
    LoginComponent,
    SignupComponent,
    LanguageSelectorComponent,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    ToastrModule.forRoot({
      timeOut: 10000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    // configure ngx-translate
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
    RouterModule.forRoot([
      { path: 'login', component: LoginComponent },
      { path: 'register', component: SignupComponent },
      { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
      { path: 'vehicles/new', component: VehicleFormComponent, canActivate: [AuthGuard] },
      { path: 'vehicles/edit/:id', component: VehicleFormComponent, canActivate: [AuthGuard] },
      { path: 'vehicles/:id', component: ViewVehicleComponent, canActivate: [AuthGuard] },
      { path: 'vehicles', component: VehicleComponent, canActivate: [AuthGuard] },
      { path: 'home', component: HomeComponent },
      { path: '**', redirectTo: 'home' },
    ]),
    CommonModule,
    BrowserAnimationsModule,
    RouterModule,
    FontAwesomeModule,
  ],
  providers: [
    { provide: ErrorHandler, useClass: AppErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: LanguageHeaderInterceptor, multi: true },
    // the TranslateService is provided by TranslateModule
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    VehicleService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
