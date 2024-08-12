
        import { NgModule } from '@angular/core';
        import { MaterialExampleModule } from './asset-tracking-material.module';
        import { CommonModule } from '@angular/common';
        import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';import {AssetTrackingAppRoutingModule } from './asset-tracking-app-routing.module';import { AssetTrackingAppComponent } from './asset-tracking-app.component';
        import { TrackingPageHeaderComponent  } from './tracking-page-body/tracking-page-header/tracking-page-header.component';
        import { TrackingPageHeaderService } from './tracking-page-body/tracking-page-header/tracking-page-header.service';
        import { TrackingPageSidebarComponent  } from './tracking-page-body/tracking-page-sidebar/tracking-page-sidebar.component';
        import { TrackingPageSidebarService } from './tracking-page-body/tracking-page-sidebar/tracking-page-sidebar.service';
        import { TrackingPageBreadcrumbComponent  } from './tracking-page-body/tracking-page-breadcrumb/tracking-page-breadcrumb.component';
        import { TrackingPageBreadcrumbService } from './tracking-page-body/tracking-page-breadcrumb/tracking-page-breadcrumb.service';
        import { TrackingPageBodyComponent  } from './tracking-page-body/tracking-page-body.component';
        import { TrackingPageBodyService } from './tracking-page-body/tracking-page-body.service';
        import { MovementHistoryBreadcrumbComponent  } from './tracking-page-body/movement-history-breadcrumb/movement-history-breadcrumb.component';
        import { MovementHistoryBreadcrumbService } from './tracking-page-body/movement-history-breadcrumb/movement-history-breadcrumb.service';
        import { MovementHistoryBodyComponent  } from './tracking-page-body/movement-history-body/movement-history-body.component';
        import { MovementHistoryBodyService } from './tracking-page-body/movement-history-body/movement-history-body.service';
        import { MaintenanceHistoryBreadcrumbComponent  } from './tracking-page-body/maintenance-history-breadcrumb/maintenance-history-breadcrumb.component';
        import { MaintenanceHistoryBreadcrumbService } from './tracking-page-body/maintenance-history-breadcrumb/maintenance-history-breadcrumb.service';
        import { MaintenanceHistoryBodyComponent  } from './tracking-page-body/maintenance-history-body/maintenance-history-body.component';
        import { MaintenanceHistoryBodyService } from './tracking-page-body/maintenance-history-body/maintenance-history-body.service';
        import { ViewMaintenanceHistoryBreadcrumbComponent  } from './tracking-page-body/view-maintenance-history-breadcrumb/view-maintenance-history-breadcrumb.component';
        import { ViewMaintenanceHistoryBreadcrumbService } from './tracking-page-body/view-maintenance-history-breadcrumb/view-maintenance-history-breadcrumb.service';
        import { ViewMaintenanceHistoryPageComponent  } from './tracking-page-body/view-maintenance-history-page/view-maintenance-history-page.component';
        import { ViewMaintenanceHistoryPageService } from './tracking-page-body/view-maintenance-history-page/view-maintenance-history-page.service';
        import { ViewMovementHistoryPageComponent  } from './tracking-page-body/view-movement-history-page/view-movement-history-page.component';
        import { ViewMovementHistoryPageService } from './tracking-page-body/view-movement-history-page/view-movement-history-page.service';
        import { ViewMovementHistoryBreadcrumbComponent  } from './tracking-page-body/view-movement-history-breadcrumb/view-movement-history-breadcrumb.component';
        import { ViewMovementHistoryBreadcrumbService } from './tracking-page-body/view-movement-history-breadcrumb/view-movement-history-breadcrumb.service';
        import { AuthorizeInterceptor } from '../../../api-authorization/authorize.interceptor';

        @NgModule({
          imports:      [ CommonModule, MaterialExampleModule, HttpClientModule, AssetTrackingAppRoutingModule ],
          declarations: [ AssetTrackingAppComponent, TrackingPageHeaderComponent , TrackingPageSidebarComponent , TrackingPageBreadcrumbComponent , TrackingPageBodyComponent , MovementHistoryBreadcrumbComponent , MovementHistoryBodyComponent , MaintenanceHistoryBreadcrumbComponent , MaintenanceHistoryBodyComponent , ViewMaintenanceHistoryBreadcrumbComponent , ViewMaintenanceHistoryPageComponent , ViewMovementHistoryPageComponent , ViewMovementHistoryBreadcrumbComponent 
        ],
        providers: [HttpClient, TrackingPageHeaderService, TrackingPageSidebarService, TrackingPageBreadcrumbService, TrackingPageBodyService, MovementHistoryBreadcrumbService, MovementHistoryBodyService, MaintenanceHistoryBreadcrumbService, MaintenanceHistoryBodyService, ViewMaintenanceHistoryBreadcrumbService, ViewMaintenanceHistoryPageService, ViewMovementHistoryPageService, ViewMovementHistoryBreadcrumbService, { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }],
        entryComponents: [ 
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          ],
        bootstrap:    [ AssetTrackingAppComponent ]
      })
      export class AssetTrackingAppModule { }