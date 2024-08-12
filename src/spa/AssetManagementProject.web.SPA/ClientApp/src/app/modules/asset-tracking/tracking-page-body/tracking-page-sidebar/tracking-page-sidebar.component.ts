import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tracking-page-sidebar',
  templateUrl: './tracking-page-sidebar.component.html',
  styleUrls: ['./tracking-page-sidebar.component.scss'],
})
export class TrackingPageSidebarComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  goToAssetManagement() {
    this.router.navigateByUrl('asset-management/asset-list');
  }

  goToTrackingPage() {
    this.router.navigateByUrl('asset-tracking/tracking');
  }
  goToMovementHistoryPage() {
    this.router.navigateByUrl('asset-tracking/movement-history');
  }
  goToMaintenancePage() {
    this.router.navigateByUrl('asset-tracking/maintenance-history');
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
