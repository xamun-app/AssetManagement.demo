import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-check-in-header',
  templateUrl: './check-in-header.component.html',
  styleUrls: ['./check-in-header.component.scss'],
})
export class CheckInHeaderComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
