import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-view-borrowing-history-body',
  templateUrl: './view-borrowing-history-body.component.html',
  styleUrls: ['./view-borrowing-history-body.component.scss'],
})
export class ViewBorrowingHistoryBodyComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }

  goToDashboard() {
    this.router.navigate(['check-in-check-out-module/borrowing-history-page']);
  }
}
