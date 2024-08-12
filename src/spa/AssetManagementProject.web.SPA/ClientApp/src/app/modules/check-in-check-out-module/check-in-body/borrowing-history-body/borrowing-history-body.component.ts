import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

export interface rolesTable {
  Id: string;
  AssetName: string;
  AssignedName: string;
  DateTime: string;
  Status: string;
}

const ELEMENT_DATA: rolesTable[] = [
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
  {
    Id: '##',
    AssetName: 'Description',
    AssignedName: 'Account Name',
    DateTime: '12/12/2023 10:00pm',
    Status: 'Available',
  },
];

@Component({
  selector: 'app-borrowing-history-body',
  templateUrl: './borrowing-history-body.component.html',
  styleUrls: ['./borrowing-history-body.component.scss'],
})
export class BorrowingHistoryBodyComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'Id',
    'AssetName',
    'Status',
    'AssignedName',
    'DateTime',
    'Action',
  ];
  dataSource = ELEMENT_DATA;

  goToViewBorrowingHistoryPage() {
    this.router.navigateByUrl(
      '/check-in-check-out-module/view-borrowing-history'
    );
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
