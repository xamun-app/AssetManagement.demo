
        import { NgModule } from '@angular/core';
        import { MaterialExampleModule } from './asset-management-material.module';
        import { CommonModule } from '@angular/common';
        import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';import {AssetManagementAppRoutingModule } from './asset-management-app-routing.module';import { AssetManagementAppComponent } from './asset-management-app.component';
        import { AssetListHeaderComponent  } from './asset-list-body/asset-list-header/asset-list-header.component';
        import { AssetListHeaderService } from './asset-list-body/asset-list-header/asset-list-header.service';
        import { AssetListBodyComponent  } from './asset-list-body/asset-list-body.component';
        import { AssetListBodyService } from './asset-list-body/asset-list-body.service';
        import { AddEditAssetPageComponent  } from './asset-list-body/add-edit-asset-page/add-edit-asset-page.component';
        import { AddEditAssetPageService } from './asset-list-body/add-edit-asset-page/add-edit-asset-page.service';
        import { ViewAssetDetailsBreadcrumbComponent  } from './asset-list-body/view-asset-details-breadcrumb/view-asset-details-breadcrumb.component';
        import { ViewAssetDetailsBreadcrumbService } from './asset-list-body/view-asset-details-breadcrumb/view-asset-details-breadcrumb.service';
        import { ViewAssetDetailsSidebarComponent  } from './asset-list-body/view-asset-details-sidebar/view-asset-details-sidebar.component';
        import { ViewAssetDetailsSidebarService } from './asset-list-body/view-asset-details-sidebar/view-asset-details-sidebar.service';
        import { ViewAssetDetailsBodyComponent  } from './asset-list-body/view-asset-details-body/view-asset-details-body.component';
        import { ViewAssetDetailsBodyService } from './asset-list-body/view-asset-details-body/view-asset-details-body.service';
        import { AssetListSidebarComponent  } from './asset-list-body/asset-list-sidebar/asset-list-sidebar.component';
        import { AssetListSidebarService } from './asset-list-body/asset-list-sidebar/asset-list-sidebar.service';
        import { AssetListBreadcrumbComponent  } from './asset-list-body/asset-list-breadcrumb/asset-list-breadcrumb.component';
        import { AssetListBreadcrumbService } from './asset-list-body/asset-list-breadcrumb/asset-list-breadcrumb.service';
        import { AuthorizeInterceptor } from '../../../api-authorization/authorize.interceptor';

        @NgModule({
          imports:      [ CommonModule, MaterialExampleModule, HttpClientModule, AssetManagementAppRoutingModule ],
          declarations: [ AssetManagementAppComponent, AssetListHeaderComponent , AssetListBodyComponent , AddEditAssetPageComponent , ViewAssetDetailsBreadcrumbComponent , ViewAssetDetailsSidebarComponent , ViewAssetDetailsBodyComponent , AssetListSidebarComponent , AssetListBreadcrumbComponent 
        ],
        providers: [HttpClient, AssetListHeaderService, AssetListBodyService, AddEditAssetPageService, ViewAssetDetailsBreadcrumbService, ViewAssetDetailsSidebarService, ViewAssetDetailsBodyService, AssetListSidebarService, AssetListBreadcrumbService, { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }],
        entryComponents: [ 
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          ],
        bootstrap:    [ AssetManagementAppComponent ]
      })
      export class AssetManagementAppModule { }