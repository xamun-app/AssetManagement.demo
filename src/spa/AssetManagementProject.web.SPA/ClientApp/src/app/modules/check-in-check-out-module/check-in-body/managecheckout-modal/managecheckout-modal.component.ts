import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-managecheckout-modal',
  templateUrl: './managecheckout-modal.component.html',
  styleUrls: ['./managecheckout-modal.component.scss'],
})
export class ManagecheckoutModalComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }

  goToDashboard() {
    this.router.navigate(['check-in-check-out-module/check-out-page']);
  }
}
