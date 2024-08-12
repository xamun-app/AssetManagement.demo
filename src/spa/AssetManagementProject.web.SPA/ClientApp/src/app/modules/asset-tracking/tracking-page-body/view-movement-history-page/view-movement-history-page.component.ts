import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

export interface branchManagementTable {
  From: string;
  To: string;
  Status: string;
  AssignedName: string;
  CheckInDate: string;
  CheckOutDate: string;
}

const ELEMENT_DATA: branchManagementTable[] = [
  {
    From: 'John Doe',
    To: 'Jane Doe',
    Status: 'Status',
    AssignedName: 'Jane Doe',
    CheckInDate: '',
    CheckOutDate: '',
  },
  {
    From: 'John Doe',
    To: 'Jane Doe',
    Status: 'Status',
    AssignedName: 'Jane Doe',
    CheckInDate: '12/23/2023 10:00pm',
    CheckOutDate: '',
  },
  {
    From: 'John Doe',
    To: 'Jane Doe',
    Status: 'Status',
    AssignedName: 'Jane Doe',
    CheckInDate: '12/23/2023 10:00pm',
    CheckOutDate: '',
  },
  {
    From: 'John Doe',
    To: 'Jane Doe',
    Status: 'Status',
    AssignedName: 'Jane Doe',
    CheckInDate: '12/23/2023 10:00pm',
    CheckOutDate: '12/23/2023 10:00pm',
  },
  {
    From: 'John Doe',
    To: 'Jane Doe',
    Status: 'Status',
    AssignedName: 'Jane Doe',
    CheckInDate: '12/23/2023 10:00pm',
    CheckOutDate: '',
  },
];

@Component({
  selector: 'app-view-movement-history-page',
  templateUrl: './view-movement-history-page.component.html',
  styleUrls: ['./view-movement-history-page.component.scss'],
})
export class ViewMovementHistoryPageComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'From',
    'To',
    'Status',
    'AssignedName',
    'CheckInDate',
    'CheckOutDate',
  ];
  dataSource = ELEMENT_DATA;

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }

  goToDashboard() {
    this.router.navigate(['asset-tracking/movement-history']);
  }
}
