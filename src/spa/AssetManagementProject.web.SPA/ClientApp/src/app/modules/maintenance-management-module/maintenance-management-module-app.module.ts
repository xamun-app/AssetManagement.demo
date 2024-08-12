
        import { NgModule } from '@angular/core';
        import { MaterialExampleModule } from './maintenance-management-module-material.module';
        import { CommonModule } from '@angular/common';
        import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';import {MaintenanceManagementModuleAppRoutingModule } from './maintenance-management-module-app-routing.module';import { MaintenanceManagementModuleAppComponent } from './maintenance-management-module-app.component';
        import { ScheduledMaintenanceHeaderComponent  } from './scheduled-maintenance-page/scheduled-maintenance-header/scheduled-maintenance-header.component';
        import { ScheduledMaintenanceHeaderService } from './scheduled-maintenance-page/scheduled-maintenance-header/scheduled-maintenance-header.service';
        import { ScheduledMaintenanceSidebarComponent  } from './scheduled-maintenance-page/scheduled-maintenance-sidebar/scheduled-maintenance-sidebar.component';
        import { ScheduledMaintenanceSidebarService } from './scheduled-maintenance-page/scheduled-maintenance-sidebar/scheduled-maintenance-sidebar.service';
        import { MaintenanceRecordBreadcrumbComponent  } from './scheduled-maintenance-page/maintenance-record-breadcrumb/maintenance-record-breadcrumb.component';
        import { MaintenanceRecordBreadcrumbService } from './scheduled-maintenance-page/maintenance-record-breadcrumb/maintenance-record-breadcrumb.service';
        import { MaintenanceRecordBodyComponent  } from './scheduled-maintenance-page/maintenance-record-body/maintenance-record-body.component';
        import { MaintenanceRecordBodyService } from './scheduled-maintenance-page/maintenance-record-body/maintenance-record-body.service';
        import { MaintenanceHistoryBreadcrumbComponent  } from './scheduled-maintenance-page/maintenance-history-breadcrumb/maintenance-history-breadcrumb.component';
        import { MaintenanceHistoryBreadcrumbService } from './scheduled-maintenance-page/maintenance-history-breadcrumb/maintenance-history-breadcrumb.service';
        import { MaintenanceHistoryBodyComponent  } from './scheduled-maintenance-page/maintenance-history-body/maintenance-history-body.component';
        import { MaintenanceHistoryBodyService } from './scheduled-maintenance-page/maintenance-history-body/maintenance-history-body.service';
        import { ScheduledMaintenancePageComponent  } from './scheduled-maintenance-page/scheduled-maintenance-page.component';
        import { ScheduledMaintenancePageService } from './scheduled-maintenance-page/scheduled-maintenance-page.service';
        import { ScheduledMaintenanceBreadcrumbComponent  } from './scheduled-maintenance-page/scheduled-maintenance-breadcrumb/scheduled-maintenance-breadcrumb.component';
        import { ScheduledMaintenanceBreadcrumbService } from './scheduled-maintenance-page/scheduled-maintenance-breadcrumb/scheduled-maintenance-breadcrumb.service';
        import { AuthorizeInterceptor } from '../../../api-authorization/authorize.interceptor';

        @NgModule({
          imports:      [ CommonModule, MaterialExampleModule, HttpClientModule, MaintenanceManagementModuleAppRoutingModule ],
          declarations: [ MaintenanceManagementModuleAppComponent, ScheduledMaintenanceHeaderComponent , ScheduledMaintenanceSidebarComponent , MaintenanceRecordBreadcrumbComponent , MaintenanceRecordBodyComponent , MaintenanceHistoryBreadcrumbComponent , MaintenanceHistoryBodyComponent , ScheduledMaintenancePageComponent , ScheduledMaintenanceBreadcrumbComponent 
        ],
        providers: [HttpClient, ScheduledMaintenanceHeaderService, ScheduledMaintenanceSidebarService, MaintenanceRecordBreadcrumbService, MaintenanceRecordBodyService, MaintenanceHistoryBreadcrumbService, MaintenanceHistoryBodyService, ScheduledMaintenancePageService, ScheduledMaintenanceBreadcrumbService, { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }],
        entryComponents: [ 
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          ],
        bootstrap:    [ MaintenanceManagementModuleAppComponent ]
      })
      export class MaintenanceManagementModuleAppModule { }