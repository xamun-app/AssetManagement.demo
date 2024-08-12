import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-borrowing-history-sidebar',
  templateUrl: './borrowing-history-sidebar.component.html',
  styleUrls: ['./borrowing-history-sidebar.component.scss'],
})
export class BorrowingHistorySidebarComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
