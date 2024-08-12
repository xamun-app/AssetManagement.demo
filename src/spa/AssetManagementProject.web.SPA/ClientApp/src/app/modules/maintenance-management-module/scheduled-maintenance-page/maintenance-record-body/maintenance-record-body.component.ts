import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

export interface rolesTable {
  Id: string;
  AssetName: string;
  Details: string;
  Status: string;
  Assigned: string;
  EndDate: string;
}

const ELEMENT_DATA: rolesTable[] = [
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    Assigned: 'Name',
    EndDate: '12/13/2023 6:30am',
  },
];

@Component({
  selector: 'app-maintenance-record-body',
  templateUrl: './maintenance-record-body.component.html',
  styleUrls: ['./maintenance-record-body.component.scss'],
})
export class MaintenanceRecordBodyComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'Id',
    'Details',
    'AssetName',
    'Assigned',
    'EndDate',
    'Status',
    'Action',
  ];
  dataSource = ELEMENT_DATA;

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
