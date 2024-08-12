import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
import { ClipboardModule } from 'ngx-clipboard';
import { InlineSVGModule } from 'ng-inline-svg';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { environment } from 'src/environments/environment';
import { AuthorizeInterceptor } from '../api-authorization/authorize.interceptor';
import { MaterialExampleModule } from './material.module';import { AssetManagementAppModule } from './modules/asset-management/asset-management-app.module';
import { AssetTrackingAppModule } from './modules/asset-tracking/asset-tracking-app.module';
import { ReportsAppModule } from './modules/reports/reports-app.module';
import { CheckInCheckOutModuleAppModule } from './modules/check-in-check-out-module/check-in-check-out-module-app.module';
import { MaintenanceManagementModuleAppModule } from './modules/maintenance-management-module/maintenance-management-module-app.module';






@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ClipboardModule,
    AppRoutingModule,
    InlineSVGModule.forRoot(),
    NgbModule,
    MaterialExampleModule
,
	AssetManagementAppModule,
	AssetTrackingAppModule,
	ReportsAppModule,
	CheckInCheckOutModuleAppModule,
	MaintenanceManagementModuleAppModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
