
      import { NgModule } from '@angular/core';
      import { Routes, RouterModule } from '@angular/router';
      import { AuthorizeGuard } from '../../../api-authorization/authorize-guard';
      import { AssetManagementAppComponent } from './asset-management-app.component';
      import { AssetListBodyComponent  } from './asset-list-body/asset-list-body.component';
      import { ViewAssetDetailsBodyComponent  } from './asset-list-body/view-asset-details-body/view-asset-details-body.component';
      import { AddEditAssetPageComponent  } from './asset-list-body/add-edit-asset-page/add-edit-asset-page.component';

      const routes: Routes = [
        {
          path: '',
          component: AssetManagementAppComponent,
            children: [
                {
                  path: 'asset-list',
                  component: AssetListBodyComponent, 
                },
                {
                  path: 'view-asset-details',
                  component: ViewAssetDetailsBodyComponent, 
                },
                {
                  path: 'add-edit-asset',
                  component: AddEditAssetPageComponent, 
                },
                { path: '', redirectTo: 'asset-list', pathMatch: 'full'}
            ]
        }
      ];
      
      @NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule],
      })
      export class AssetManagementAppRoutingModule {}