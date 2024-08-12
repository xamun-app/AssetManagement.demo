import {
  Component,
  OnInit,
  Input,
  ChangeDetectorRef,
  OnDestroy,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { AddEditAssetPageService } from './add-edit-asset-page/add-edit-asset-page.service';
import { Subject, takeUntil } from 'rxjs';

export interface VendorProductManagementTable {
  VendorId: string;
  ProductName: string;
  Description: string;
  Category: string;
  Code: string;
  Status: string;
}

var ELEMENT_DATA: VendorProductManagementTable[] = [];

@Component({
  selector: 'app-asset-list-body',
  templateUrl: './asset-list-body.component.html',
  styleUrls: ['./asset-list-body.component.scss'],
})
export class AssetListBodyComponent implements OnInit, OnDestroy {
  private unsubscribe$ = new Subject<void>();
  constructor(
    private sanitized: DomSanitizer,
    private router: Router,
    private addEditAssetService: AddEditAssetPageService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.addEditAssetService.currentState
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((state) => {
        if (
          state?.assetName &&
          state?.itemDescription &&
          history.state.action != 'Edit'
        ) {
          const payload: VendorProductManagementTable = {
            VendorId: '1',
            ProductName: state?.assetName,
            Description: state?.itemDescription,
            Category: 'Category',
            Code: '930212',
            Status: 'Active',
          };

          ELEMENT_DATA.push(payload);
          this.dataSource = [...ELEMENT_DATA];
        }
        this.cdr.detectChanges();
      });

    if (history.state.action === 'Edit') {
      // Find the index of the item to be updated
      const index = ELEMENT_DATA.findIndex(
        (x) => x.VendorId === history.state.data.id
      );

      // Check if the item is found
      if (index !== -1) {
        // Create a new object with updated data
        const updatedItem: VendorProductManagementTable = {
          ...ELEMENT_DATA[index],
          ProductName: history.state.data.assetName,
          Description: history.state.data.itemDescription,
          // Add other properties if needed
        };

        // Replace the old item with the updated item
        ELEMENT_DATA[index] = updatedItem;
        this.dataSource = [...ELEMENT_DATA];
      }
    }
  }

  goToAddEditPage(action: string, element?: any) {
    this.addEditAssetService.changeState(null);
    this.router.navigate(['asset-management/add-edit-asset'], {
      state: {
        action: action,
        data: element,
      },
    });
  }

  displayedColumns: string[] = [
    'VendorId',
    'ProductName',
    'Description',
    'Category',
    'Code',
    'Status',
    'Action',
  ];
  dataSource = ELEMENT_DATA;

  goToViewAssetDetails(element: VendorProductManagementTable) {
    this.router.navigate(['asset-management/view-asset-details'], {
      state: { data: element },
    });
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
