import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-scheduled-maintenance-sidebar',
  templateUrl: './scheduled-maintenance-sidebar.component.html',
  styleUrls: ['./scheduled-maintenance-sidebar.component.scss'],
})
export class ScheduledMaintenanceSidebarComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  goToAssetManagement() {
    this.router.navigateByUrl('asset-management/asset-list');
  }

  goToAssetTracking() {
    this.router.navigateByUrl('asset-tracking/tracking');
  }

  goToReport() {
    this.router.navigateByUrl('reports/asset-maintenance-report');
  }

  goToScheduledMaintenanceBody() {
    this.router.navigateByUrl(
      'maintenance-management-module/scheduled-maintenance-body'
    );
  }

  goToMaintenanceRecordBody() {
    this.router.navigateByUrl(
      'maintenance-management-module/maintenance-record-body'
    );
  }
  goToMaintenanceHistoryBody() {
    this.router.navigateByUrl(
      'maintenance-management-module/maintenance-history-body'
    );
  }

  goToCheckIn() {
    this.router.navigateByUrl('check-in-check-out-module/check-in-page');
  }

  goToCheckOut() {
    this.router.navigateByUrl('check-in-check-out-module/check-out-page');
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
