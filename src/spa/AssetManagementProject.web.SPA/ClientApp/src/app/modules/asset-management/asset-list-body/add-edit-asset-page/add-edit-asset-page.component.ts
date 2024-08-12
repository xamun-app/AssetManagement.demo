import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { AddEditAssetPageService } from './add-edit-asset-page.service';

@Component({
  selector: 'app-add-edit-asset-page',
  templateUrl: './add-edit-asset-page.component.html',
  styleUrls: ['./add-edit-asset-page.component.scss'],
})
export class AddEditAssetPageComponent implements OnInit {
  action: string;

  constructor(
    private sanitized: DomSanitizer,
    private router: Router,
    private addEditAssetService: AddEditAssetPageService
  ) {}

  addEditProductForm: FormGroup = new FormGroup({
    id: new FormControl(''),
    assetName: new FormControl(''),
    itemDescription: new FormControl(''),
  });

  ngOnInit(): void {
    this.action = history.state.action;

    if (this.action == 'Edit') {
      const patch = {
        id: history.state.data.VendorId,
        assetName: history.state.data.ProductName,
        itemDescription: history.state.data.Description,
      };

      this.addEditProductForm.patchValue(patch);
    }
  }

  goToAssetListPage() {
    this.router.navigateByUrl('asset-management/asset-list');
  }

  save() {
    this.addEditAssetService.changeState(this.addEditProductForm.value);
    if (this.action == 'Edit') {
      this.router.navigate(['asset-management/asset-list'], {
        state: {
          action: 'Edit',
          data: this.addEditProductForm.value,
        },
      });
    } else {
      this.goToAssetListPage();
    }
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
