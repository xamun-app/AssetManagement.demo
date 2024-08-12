import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-view-maintenance-history-breadcrumb',
  templateUrl: './view-maintenance-history-breadcrumb.component.html',
  styleUrls: ['./view-maintenance-history-breadcrumb.component.scss'],
})
export class ViewMaintenanceHistoryBreadcrumbComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
