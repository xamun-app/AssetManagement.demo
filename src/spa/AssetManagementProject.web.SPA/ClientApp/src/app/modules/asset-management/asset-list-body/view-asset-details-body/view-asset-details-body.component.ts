import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { AddEditAssetPageService } from '../add-edit-asset-page/add-edit-asset-page.service';

@Component({
  selector: 'app-view-asset-details-body',
  templateUrl: './view-asset-details-body.component.html',
  styleUrls: ['./view-asset-details-body.component.scss'],
})
export class ViewAssetDetailsBodyComponent implements OnInit {
  assetDetails: any;

  constructor(
    private sanitized: DomSanitizer,
    private router: Router,
    private addEditAssetService: AddEditAssetPageService
  ) {}

  ngOnInit(): void {
    this.assetDetails = history.state.data;
    console.log(history.state.data);
    console.log(this.assetDetails);
  }

  goToHome() {
    this.addEditAssetService.changeState(null);
    this.router.navigate(['asset-management/asset-list']);
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
