import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-scheduled-maintenance-breadcrumb',
  templateUrl: './scheduled-maintenance-breadcrumb.component.html',
  styleUrls: ['./scheduled-maintenance-breadcrumb.component.scss'],
})
export class ScheduledMaintenanceBreadcrumbComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
