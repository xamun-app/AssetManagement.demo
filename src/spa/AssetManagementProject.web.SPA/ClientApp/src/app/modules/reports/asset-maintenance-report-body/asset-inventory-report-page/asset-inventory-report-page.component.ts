import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

export interface rolesTable {
  Id: string;
  AssetName: string;
  Category: string;
  Location: string;
  Supplier: string;
  Status: string;
  TotalCost: string;
  PurchaseDate: string;
  Expiry: string;
}

const ELEMENT_DATA: rolesTable[] = [
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
  {
    Id: '##',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Supplier: 'Supplier',
    Status: 'In Use',
    TotalCost: '100000',
    PurchaseDate: 'mm/dd/yyyy',
    Expiry: 'mm/dd/yyyy',
  },
];

@Component({
  selector: 'app-asset-inventory-report-page',
  templateUrl: './asset-inventory-report-page.component.html',
  styleUrls: ['./asset-inventory-report-page.component.scss'],
})
export class AssetInventoryReportPageComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'Id',
    'AssetName',
    'Category',
    'Location',
    'Supplier',
    'Status',
    'TotalCost',
    'PurchaseDate',
    'Expiry',
    // 'Action',
  ];
  dataSource = ELEMENT_DATA;

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
