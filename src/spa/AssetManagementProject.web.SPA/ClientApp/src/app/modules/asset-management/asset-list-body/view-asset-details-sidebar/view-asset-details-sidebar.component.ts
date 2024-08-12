import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-view-asset-details-sidebar',
  templateUrl: './view-asset-details-sidebar.component.html',
  styleUrls: ['./view-asset-details-sidebar.component.scss'],
})
export class ViewAssetDetailsSidebarComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

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

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
