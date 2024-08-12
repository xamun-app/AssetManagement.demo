import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../api-authorization/authorize-guard';
import { AuthCallbackComponent } from '../auth-callback/auth-callback.component';

export const routes: Routes = [
{ path: '', redirectTo: 'asset-management', pathMatch: 'full' }, 
{ path: 'maintenance-management-module', loadChildren: () => import('./modules/maintenance-management-module/maintenance-management-module-app.module').then((m) => m.MaintenanceManagementModuleAppModule), },
{ path: 'check-in-check-out-module', loadChildren: () => import('./modules/check-in-check-out-module/check-in-check-out-module-app.module').then((m) => m.CheckInCheckOutModuleAppModule), },
{ path: 'reports', loadChildren: () => import('./modules/reports/reports-app.module').then((m) => m.ReportsAppModule), },
{ path: 'asset-tracking', loadChildren: () => import('./modules/asset-tracking/asset-tracking-app.module').then((m) => m.AssetTrackingAppModule), },
{ path: 'asset-management', loadChildren: () => import('./modules/asset-management/asset-management-app.module').then((m) => m.AssetManagementAppModule), },
  {
    path: 'error',
    loadChildren: () =>
      import('./modules/errors/errors.module').then((m) => m.ErrorsModule),
  },
  {
    path: 'auth-callback',
    component: AuthCallbackComponent
  },
  { path: '**', redirectTo: 'error/404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled'})],
  exports: [RouterModule],
})
export class AppRoutingModule { }
