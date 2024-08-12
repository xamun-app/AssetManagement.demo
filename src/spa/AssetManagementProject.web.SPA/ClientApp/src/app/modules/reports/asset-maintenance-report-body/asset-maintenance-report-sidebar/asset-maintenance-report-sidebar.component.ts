import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-asset-maintenance-report-sidebar',
  templateUrl: './asset-maintenance-report-sidebar.component.html',
  styleUrls: ['./asset-maintenance-report-sidebar.component.scss'],
})
export class AssetMaintenanceReportSidebarComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  goToAssetInventoryPage() {
    this.router.navigateByUrl('reports/asset-inventory-report-page');
  }

  goToAssetMaintenancePage() {
    this.router.navigateByUrl('reports/asset-maintenance-report');
  }

  goToAssetManagement() {
    this.router.navigateByUrl('asset-management/asset-list');
  }

  goToAssetTracking() {
    this.router.navigateByUrl('asset-tracking/tracking');
  }

  goMaintenanceManagement() {
    this.router.navigateByUrl(
      'maintenance-management-module/scheduled-maintenance-body'
    );
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
