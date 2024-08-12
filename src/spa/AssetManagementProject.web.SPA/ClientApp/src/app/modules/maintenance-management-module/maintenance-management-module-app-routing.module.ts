
      import { NgModule } from '@angular/core';
      import { Routes, RouterModule } from '@angular/router';
      import { AuthorizeGuard } from '../../../api-authorization/authorize-guard';
      import { MaintenanceManagementModuleAppComponent } from './maintenance-management-module-app.component';
      import { ScheduledMaintenancePageComponent  } from './scheduled-maintenance-page/scheduled-maintenance-page.component';
      import { MaintenanceRecordBodyComponent  } from './scheduled-maintenance-page/maintenance-record-body/maintenance-record-body.component';
      import { MaintenanceHistoryBodyComponent  } from './scheduled-maintenance-page/maintenance-history-body/maintenance-history-body.component';

      const routes: Routes = [
        {
          path: '',
          component: MaintenanceManagementModuleAppComponent,
            children: [
                {
                  path: 'scheduled-maintenance-body',
                  component: ScheduledMaintenancePageComponent, 
                },
                {
                  path: 'maintenance-record-body',
                  component: MaintenanceRecordBodyComponent, 
                },
                {
                  path: 'maintenance-history-body',
                  component: MaintenanceHistoryBodyComponent, 
                },
                { path: '', redirectTo: 'scheduled-maintenance-body', pathMatch: 'full'}
            ]
        }
      ];
      
      @NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule],
      })
      export class MaintenanceManagementModuleAppRoutingModule {}