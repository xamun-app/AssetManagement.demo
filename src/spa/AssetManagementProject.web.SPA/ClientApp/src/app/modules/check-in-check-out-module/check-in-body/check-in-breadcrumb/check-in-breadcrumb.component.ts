import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-check-in-breadcrumb',
  templateUrl: './check-in-breadcrumb.component.html',
  styleUrls: ['./check-in-breadcrumb.component.scss'],
})
export class CheckInBreadcrumbComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
