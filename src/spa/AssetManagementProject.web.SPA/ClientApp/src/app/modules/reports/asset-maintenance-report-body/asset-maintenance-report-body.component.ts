import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

export interface rolesTable {
  Id: string;
  AssetName: string;
  Category: string;
  Location: string;
  Type: string;
  Cost: string;
  Status: string;
  LastMaintenance: string;
  NextMaintenance: string;
}

const ELEMENT_DATA: rolesTable[] = [
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Repaired',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Repaired',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Ongoing',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Stopped',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Stopped',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Repaired',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Ongoing',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Repaired',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Ongoing',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Ongoing',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Ongoing',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Ongoing',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
  {
    Id: '####',
    AssetName: 'Asset Name',
    Category: 'Category',
    Location: 'Location',
    Type: 'Type',
    Cost: '100000',
    Status: 'Ongoing',
    LastMaintenance: 'mm/dd/yyyy',
    NextMaintenance: 'mm/dd/yyyy',
  },
];

@Component({
  selector: 'app-asset-maintenance-report-body',
  templateUrl: './asset-maintenance-report-body.component.html',
  styleUrls: ['./asset-maintenance-report-body.component.scss'],
})
export class AssetMaintenanceReportBodyComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'Id',
    'AssetName',
    'Category',
    'Location',
    'Type',
    'Cost',
    'Status',
    'LastMaintenance',
    'NextMaintenance',
    // 'Action',
  ];
  dataSource = ELEMENT_DATA;

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
