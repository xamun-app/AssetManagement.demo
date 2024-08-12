import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

export interface rolesTable {
  Id: string;
  AssetName: string;
  Details: string;
  Status: string;
  StartDate: string;
  EndDate: string;
}

const ELEMENT_DATA: rolesTable[] = [
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',
    Status: 'Completed',
    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
  {
    Id: 'XXXXXX',
    AssetName: 'Asset Name',
    Details: 'Details',

    Status: 'Completed',

    StartDate: '12/12/2023 10:30pm',
    EndDate: '12/13/2023 6:30am',
  },
];

@Component({
  selector: 'app-scheduled-maintenance-page',
  templateUrl: './scheduled-maintenance-page.component.html',
  styleUrls: ['./scheduled-maintenance-page.component.scss'],
})
export class ScheduledMaintenancePageComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'Id',
    'Details',
    'AssetName',
    'StartDate',
    'EndDate',
    'Status',
    'Action',
  ];
  dataSource = ELEMENT_DATA;

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
