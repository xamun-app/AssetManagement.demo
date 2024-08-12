
        import { NgModule } from '@angular/core';
        import { MaterialExampleModule } from './check-in-check-out-module-material.module';
        import { CommonModule } from '@angular/common';
        import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';import {CheckInCheckOutModuleAppRoutingModule } from './check-in-check-out-module-app-routing.module';import { CheckInCheckOutModuleAppComponent } from './check-in-check-out-module-app.component';
        import { CheckInHeaderComponent  } from './check-in-body/check-in-header/check-in-header.component';
        import { CheckInHeaderService } from './check-in-body/check-in-header/check-in-header.service';
        import { CheckInBreadcrumbComponent  } from './check-in-body/check-in-breadcrumb/check-in-breadcrumb.component';
        import { CheckInBreadcrumbService } from './check-in-body/check-in-breadcrumb/check-in-breadcrumb.service';
        import { CheckInBodyComponent  } from './check-in-body/check-in-body.component';
        import { CheckInBodyService } from './check-in-body/check-in-body.service';
        import { ManageCheckinModalComponent  } from './check-in-body/manage-checkin-modal/manage-checkin-modal.component';
        import { ManageCheckinModalService } from './check-in-body/manage-checkin-modal/manage-checkin-modal.service';
        import { CheckOutBreadcrumbComponent  } from './check-in-body/check-out-breadcrumb/check-out-breadcrumb.component';
        import { CheckOutBreadcrumbService } from './check-in-body/check-out-breadcrumb/check-out-breadcrumb.service';
        import { CheckOutBodyComponent  } from './check-in-body/check-out-body/check-out-body.component';
        import { CheckOutBodyService } from './check-in-body/check-out-body/check-out-body.service';
        import { ManagecheckoutModalComponent  } from './check-in-body/managecheckout-modal/managecheckout-modal.component';
        import { ManagecheckoutModalService } from './check-in-body/managecheckout-modal/managecheckout-modal.service';
        import { BorrowingHistoryBreadcrumbComponent  } from './check-in-body/borrowing-history-breadcrumb/borrowing-history-breadcrumb.component';
        import { BorrowingHistoryBreadcrumbService } from './check-in-body/borrowing-history-breadcrumb/borrowing-history-breadcrumb.service';
        import { BorrowingHistoryBodyComponent  } from './check-in-body/borrowing-history-body/borrowing-history-body.component';
        import { BorrowingHistoryBodyService } from './check-in-body/borrowing-history-body/borrowing-history-body.service';
        import { ViewBorrowingHistoryBreadcrumbComponent  } from './check-in-body/view-borrowing-history-breadcrumb/view-borrowing-history-breadcrumb.component';
        import { ViewBorrowingHistoryBreadcrumbService } from './check-in-body/view-borrowing-history-breadcrumb/view-borrowing-history-breadcrumb.service';
        import { ViewBorrowingHistorySidebarComponent  } from './check-in-body/view-borrowing-history-sidebar/view-borrowing-history-sidebar.component';
        import { ViewBorrowingHistorySidebarService } from './check-in-body/view-borrowing-history-sidebar/view-borrowing-history-sidebar.service';
        import { ViewBorrowingHistoryBodyComponent  } from './check-in-body/view-borrowing-history-body/view-borrowing-history-body.component';
        import { ViewBorrowingHistoryBodyService } from './check-in-body/view-borrowing-history-body/view-borrowing-history-body.service';
        import { BorrowingHistorySidebarComponent  } from './check-in-body/borrowing-history-sidebar/borrowing-history-sidebar.component';
        import { BorrowingHistorySidebarService } from './check-in-body/borrowing-history-sidebar/borrowing-history-sidebar.service';
        import { CheckInSidebarComponent  } from './check-in-body/check-in-sidebar/check-in-sidebar.component';
        import { CheckInSidebarService } from './check-in-body/check-in-sidebar/check-in-sidebar.service';
        import { AuthorizeInterceptor } from '../../../api-authorization/authorize.interceptor';

        @NgModule({
          imports:      [ CommonModule, MaterialExampleModule, HttpClientModule, CheckInCheckOutModuleAppRoutingModule ],
          declarations: [ CheckInCheckOutModuleAppComponent, CheckInHeaderComponent , CheckInBreadcrumbComponent , CheckInBodyComponent , ManageCheckinModalComponent , CheckOutBreadcrumbComponent , CheckOutBodyComponent , ManagecheckoutModalComponent , BorrowingHistoryBreadcrumbComponent , BorrowingHistoryBodyComponent , ViewBorrowingHistoryBreadcrumbComponent , ViewBorrowingHistorySidebarComponent , ViewBorrowingHistoryBodyComponent , BorrowingHistorySidebarComponent , CheckInSidebarComponent 
        ],
        providers: [HttpClient, CheckInHeaderService, CheckInBreadcrumbService, CheckInBodyService, ManageCheckinModalService, CheckOutBreadcrumbService, CheckOutBodyService, ManagecheckoutModalService, BorrowingHistoryBreadcrumbService, BorrowingHistoryBodyService, ViewBorrowingHistoryBreadcrumbService, ViewBorrowingHistorySidebarService, ViewBorrowingHistoryBodyService, BorrowingHistorySidebarService, CheckInSidebarService, { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }],
        entryComponents: [ 
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          
          ],
        bootstrap:    [ CheckInCheckOutModuleAppComponent ]
      })
      export class CheckInCheckOutModuleAppModule { }