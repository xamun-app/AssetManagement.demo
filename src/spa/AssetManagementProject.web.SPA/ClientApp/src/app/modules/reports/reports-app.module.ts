
        import { NgModule } from '@angular/core';
        import { MaterialExampleModule } from './reports-material.module';
        import { CommonModule } from '@angular/common';
        import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';import {ReportsAppRoutingModule } from './reports-app-routing.module';import { ReportsAppComponent } from './reports-app.component';
        import { AssetMaintenanceReportHeaderComponent  } from './asset-maintenance-report-body/asset-maintenance-report-header/asset-maintenance-report-header.component';
        import { AssetMaintenanceReportHeaderService } from './asset-maintenance-report-body/asset-maintenance-report-header/asset-maintenance-report-header.service';
        import { AssetMaintenanceReportSidebarComponent  } from './asset-maintenance-report-body/asset-maintenance-report-sidebar/asset-maintenance-report-sidebar.component';
        import { AssetMaintenanceReportSidebarService } from './asset-maintenance-report-body/asset-maintenance-report-sidebar/asset-maintenance-report-sidebar.service';
        import { AssetMaintenanceReportBreadcrumbComponent  } from './asset-maintenance-report-body/asset-maintenance-report-breadcrumb/asset-maintenance-report-breadcrumb.component';
        import { AssetMaintenanceReportBreadcrumbService } from './asset-maintenance-report-body/asset-maintenance-report-breadcrumb/asset-maintenance-report-breadcrumb.service';
        import { AssetMaintenanceReportBodyComponent  } from './asset-maintenance-report-body/asset-maintenance-report-body.component';
        import { AssetMaintenanceReportBodyService } from './asset-maintenance-report-body/asset-maintenance-report-body.service';
        import { AssetInventoryReportBreadcrumbComponent  } from './asset-maintenance-report-body/asset-inventory-report-breadcrumb/asset-inventory-report-breadcrumb.component';
        import { AssetInventoryReportBreadcrumbService } from './asset-maintenance-report-body/asset-inventory-report-breadcrumb/asset-inventory-report-breadcrumb.service';
        import { AssetInventoryReportPageComponent  } from './asset-maintenance-report-body/asset-inventory-report-page/asset-inventory-report-page.component';
        import { AssetInventoryReportPageService } from './asset-maintenance-report-body/asset-inventory-report-page/asset-inventory-report-page.service';
        import { AuthorizeInterceptor } from '../../../api-authorization/authorize.interceptor';

        @NgModule({
          imports:      [ CommonModule, MaterialExampleModule, HttpClientModule, ReportsAppRoutingModule ],
          declarations: [ ReportsAppComponent, AssetMaintenanceReportHeaderComponent , AssetMaintenanceReportSidebarComponent , AssetMaintenanceReportBreadcrumbComponent , AssetMaintenanceReportBodyComponent , AssetInventoryReportBreadcrumbComponent , AssetInventoryReportPageComponent 
        ],
        providers: [HttpClient, AssetMaintenanceReportHeaderService, AssetMaintenanceReportSidebarService, AssetMaintenanceReportBreadcrumbService, AssetMaintenanceReportBodyService, AssetInventoryReportBreadcrumbService, AssetInventoryReportPageService, { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }],
        entryComponents: [ 
          
          
          
          
          
          
          
          
          
          
          
          ],
        bootstrap:    [ ReportsAppComponent ]
      })
      export class ReportsAppModule { }