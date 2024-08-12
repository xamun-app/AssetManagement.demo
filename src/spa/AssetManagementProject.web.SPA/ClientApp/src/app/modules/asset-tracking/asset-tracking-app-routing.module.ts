
      import { NgModule } from '@angular/core';
      import { Routes, RouterModule } from '@angular/router';
      import { AuthorizeGuard } from '../../../api-authorization/authorize-guard';
      import { AssetTrackingAppComponent } from './asset-tracking-app.component';
      import { TrackingPageBodyComponent  } from './tracking-page-body/tracking-page-body.component';
      import { MovementHistoryBodyComponent  } from './tracking-page-body/movement-history-body/movement-history-body.component';
      import { MaintenanceHistoryBodyComponent  } from './tracking-page-body/maintenance-history-body/maintenance-history-body.component';
      import { ViewMovementHistoryPageComponent  } from './tracking-page-body/view-movement-history-page/view-movement-history-page.component';
      import { ViewMaintenanceHistoryPageComponent  } from './tracking-page-body/view-maintenance-history-page/view-maintenance-history-page.component';

      const routes: Routes = [
        {
          path: '',
          component: AssetTrackingAppComponent,
            children: [
                {
                  path: 'tracking',
                  component: TrackingPageBodyComponent, 
                },
                {
                  path: 'movement-history',
                  component: MovementHistoryBodyComponent, 
                },
                {
                  path: 'maintenance-history',
                  component: MaintenanceHistoryBodyComponent, 
                },
                {
                  path: 'view-movement-history',
                  component: ViewMovementHistoryPageComponent, 
                },
                {
                  path: 'view-maintenance-history',
                  component: ViewMaintenanceHistoryPageComponent, 
                },
                { path: '', redirectTo: 'tracking', pathMatch: 'full'}
            ]
        }
      ];
      
      @NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule],
      })
      export class AssetTrackingAppRoutingModule {}