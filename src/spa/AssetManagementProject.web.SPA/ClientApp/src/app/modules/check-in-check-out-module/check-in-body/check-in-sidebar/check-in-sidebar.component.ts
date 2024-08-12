import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-check-in-sidebar',
  templateUrl: './check-in-sidebar.component.html',
  styleUrls: ['./check-in-sidebar.component.scss'],
})
export class CheckInSidebarComponent implements OnInit {
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

  goToCheckInBodyPage() {
    this.router.navigateByUrl('/check-in-check-out-module/check-in-page');
  }
  goToCheckOutBodyPage() {
    this.router.navigateByUrl('/check-in-check-out-module/check-out-page');
  }
  goToBorrowingHistoryBodyPage() {
    this.router.navigateByUrl(
      '/check-in-check-out-module/borrowing-history-page'
    );
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
