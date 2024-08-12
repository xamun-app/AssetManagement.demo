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
  selector: 'app-check-in-body',
  templateUrl: './check-in-body.component.html',
  styleUrls: ['./check-in-body.component.scss'],
})
export class CheckInBodyComponent implements OnInit {
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

  goToManageCheckInModal() {
    this.router.navigateByUrl(
      '/check-in-check-out-module/manage-check-in-modal'
    );
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
