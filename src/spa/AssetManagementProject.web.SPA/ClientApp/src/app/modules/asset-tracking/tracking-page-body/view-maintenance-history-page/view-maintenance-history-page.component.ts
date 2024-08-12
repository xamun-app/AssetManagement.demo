import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

export interface branchManagementTable {
  ServiceName: string;
  Type: string;
  Reason: string;
  Status: string;
  GivenBy: string;
  ServiceDate: string;
  TotalCost: string;
  Notes: string;
}

const ELEMENT_DATA: branchManagementTable[] = [
  {
    ServiceName: 'Name',
    Type: 'Paid Service',
    Status: 'Status',
    Reason: 'Repair',
    GivenBy: 'Jane Doe',
    ServiceDate: '12/23/2023 10:00pm',
    TotalCost: '100',
    Notes: 'For Repair',
  },
  {
    ServiceName: 'Name',
    Type: 'Paid Service',
    Status: 'Status',
    Reason: 'Repair',
    GivenBy: 'Jane Doe',
    ServiceDate: '12/23/2023 10:00pm',
    TotalCost: '100',
    Notes: 'For Repair',
  },
  {
    ServiceName: 'Name',
    Type: 'Paid Service',
    Status: 'Status',
    Reason: 'Repair',
    GivenBy: 'Jane Doe',
    ServiceDate: '12/23/2023 10:00pm',
    TotalCost: '100',
    Notes: 'For Repair',
  },
  {
    ServiceName: 'Name',
    Type: 'Paid Service',
    Status: 'Status',
    Reason: 'Repair',
    GivenBy: 'Jane Doe',
    ServiceDate: '12/23/2023 10:00pm',
    TotalCost: '100',
    Notes: 'For Repair',
  },
  {
    ServiceName: 'Name',
    Type: 'Paid Service',
    Status: 'Status',
    Reason: 'Repair',
    GivenBy: 'Jane Doe',
    ServiceDate: '12/23/2023 10:00pm',
    TotalCost: '100',
    Notes: 'For Repair',
  },
];

@Component({
  selector: 'app-view-maintenance-history-page',
  templateUrl: './view-maintenance-history-page.component.html',
  styleUrls: ['./view-maintenance-history-page.component.scss'],
})
export class ViewMaintenanceHistoryPageComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'ServiceName',
    'Type',
    'Reason',
    'Status',
    'ServiceDate',
    'GivenBy',
    'TotalCost',
    'Notes',
  ];
  dataSource = ELEMENT_DATA;

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }

  goToDashboard() {
    this.router.navigate(['asset-tracking/maintenance-history']);
  }
}
