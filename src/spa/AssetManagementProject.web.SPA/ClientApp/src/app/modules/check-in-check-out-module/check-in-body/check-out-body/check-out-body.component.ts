import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

export interface rolesTable {
  Id: string;
  AssetName: string;
  Category: string;
  Condition: string;
  Status: string;
}

const ELEMENT_DATA: rolesTable[] = [
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Condition: 'Good',
    Status: 'Available',
  },
];

@Component({
  selector: 'app-check-out-body',
  templateUrl: './check-out-body.component.html',
  styleUrls: ['./check-out-body.component.scss'],
})
export class CheckOutBodyComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'Id',
    'AssetName',
    'Category',
    'Condition',
    'Status',
    'Action',
  ];
  dataSource = ELEMENT_DATA;

  goToManageCheckoutModal() {
    this.router.navigateByUrl(
      '/check-in-check-out-module/manage-check-out-modal'
    );
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
