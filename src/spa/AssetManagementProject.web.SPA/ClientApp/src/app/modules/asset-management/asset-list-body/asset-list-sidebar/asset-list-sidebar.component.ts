import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-asset-list-sidebar',
  templateUrl: './asset-list-sidebar.component.html',
  styleUrls: ['./asset-list-sidebar.component.scss'],
})
export class AssetListSidebarComponent implements OnInit {
  constructor(private router: Router) {}
  ngOnInit(): void {}

  goToAssetListPage() {
    this.router.navigateByUrl('asset-management/asset-list');
  }

  goToAssetTracking() {
    this.router.navigateByUrl('asset-tracking/tracking');
  }

  goToReport() {
    this.router.navigateByUrl('reports/asset-maintenance-report');
  }

  goMaintenanceManagement() {
    this.router.navigateByUrl(
      'maintenance-management-module/scheduled-maintenance-body'
    );
  }
}
