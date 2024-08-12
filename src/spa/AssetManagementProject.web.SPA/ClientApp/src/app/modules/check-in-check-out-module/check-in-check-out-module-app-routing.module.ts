
      import { NgModule } from '@angular/core';
      import { Routes, RouterModule } from '@angular/router';
      import { AuthorizeGuard } from '../../../api-authorization/authorize-guard';
      import { CheckInCheckOutModuleAppComponent } from './check-in-check-out-module-app.component';
      import { CheckInBodyComponent  } from './check-in-body/check-in-body.component';
      import { CheckOutBodyComponent  } from './check-in-body/check-out-body/check-out-body.component';
      import { BorrowingHistoryBodyComponent  } from './check-in-body/borrowing-history-body/borrowing-history-body.component';
      import { ManagecheckoutModalComponent  } from './check-in-body/managecheckout-modal/managecheckout-modal.component';
      import { ManageCheckinModalComponent  } from './check-in-body/manage-checkin-modal/manage-checkin-modal.component';
      import { ViewBorrowingHistoryBodyComponent  } from './check-in-body/view-borrowing-history-body/view-borrowing-history-body.component';

      const routes: Routes = [
        {
          path: '',
          component: CheckInCheckOutModuleAppComponent,
            children: [
                {
                  path: 'check-in-page',
                  component: CheckInBodyComponent, 
                },
                {
                  path: 'check-out-page',
                  component: CheckOutBodyComponent, 
                },
                {
                  path: 'borrowing-history-page',
                  component: BorrowingHistoryBodyComponent, 
                },
                {
                  path: 'manage-check-out-modal',
                  component: ManagecheckoutModalComponent, 
                },
                {
                  path: 'manage-check-in-modal',
                  component: ManageCheckinModalComponent, 
                },
                {
                  path: 'view-borrowing-history',
                  component: ViewBorrowingHistoryBodyComponent, 
                },
                { path: '', redirectTo: 'check-in-page', pathMatch: 'full'}
            ]
        }
      ];
      
      @NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule],
      })
      export class CheckInCheckOutModuleAppRoutingModule {}