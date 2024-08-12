import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-check-out-breadcrumb',
  templateUrl: './check-out-breadcrumb.component.html',
  styleUrls: ['./check-out-breadcrumb.component.scss'],
})
export class CheckOutBreadcrumbComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
