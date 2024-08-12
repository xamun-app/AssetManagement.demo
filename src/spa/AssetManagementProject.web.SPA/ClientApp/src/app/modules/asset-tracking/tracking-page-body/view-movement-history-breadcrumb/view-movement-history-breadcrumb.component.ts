import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-view-movement-history-breadcrumb',
  templateUrl: './view-movement-history-breadcrumb.component.html',
  styleUrls: ['./view-movement-history-breadcrumb.component.scss'],
})
export class ViewMovementHistoryBreadcrumbComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
