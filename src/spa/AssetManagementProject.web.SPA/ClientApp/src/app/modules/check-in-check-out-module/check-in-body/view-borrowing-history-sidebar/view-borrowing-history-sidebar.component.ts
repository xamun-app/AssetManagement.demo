import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-view-borrowing-history-sidebar',
  templateUrl: './view-borrowing-history-sidebar.component.html',
  styleUrls: ['./view-borrowing-history-sidebar.component.scss'],
})
export class ViewBorrowingHistorySidebarComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
