
      import { NgModule } from '@angular/core';
      import { Routes, RouterModule } from '@angular/router';
      import { AuthorizeGuard } from '../../../api-authorization/authorize-guard';
      import { ReportsAppComponent } from './reports-app.component';
      import { AssetMaintenanceReportBodyComponent  } from './asset-maintenance-report-body/asset-maintenance-report-body.component';
      import { AssetInventoryReportPageComponent  } from './asset-maintenance-report-body/asset-inventory-report-page/asset-inventory-report-page.component';

      const routes: Routes = [
        {
          path: '',
          component: ReportsAppComponent,
            children: [
                {
                  path: 'asset-maintenance-report',
                  component: AssetMaintenanceReportBodyComponent, 
                },
                {
                  path: 'asset-inventory-report-page',
                  component: AssetInventoryReportPageComponent, 
                },
                { path: '', redirectTo: 'asset-maintenance-report', pathMatch: 'full'}
            ]
        }
      ];
      
      @NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule],
      })
      export class ReportsAppRoutingModule {}