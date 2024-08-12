import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manage-checkin-modal',
  templateUrl: './manage-checkin-modal.component.html',
  styleUrls: ['./manage-checkin-modal.component.scss'],
})
export class ManageCheckinModalComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }

  goToDashboard() {
    this.router.navigate(['check-in-check-out-module/check-in-page']);
  }
}
