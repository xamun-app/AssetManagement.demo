import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-borrowing-history-breadcrumb',
  templateUrl: './borrowing-history-breadcrumb.component.html',
  styleUrls: ['./borrowing-history-breadcrumb.component.scss'],
})
export class BorrowingHistoryBreadcrumbComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
