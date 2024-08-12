import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-view-borrowing-history-breadcrumb',
  templateUrl: './view-borrowing-history-breadcrumb.component.html',
  styleUrls: ['./view-borrowing-history-breadcrumb.component.scss'],
})
export class ViewBorrowingHistoryBreadcrumbComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
