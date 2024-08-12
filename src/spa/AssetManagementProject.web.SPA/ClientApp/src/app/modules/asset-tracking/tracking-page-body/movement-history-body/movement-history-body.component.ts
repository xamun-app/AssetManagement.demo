import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

export interface rolesTable {
  AssetName: string;
  Description: string;
  DateTime: string;
}

const ELEMENT_DATA: rolesTable[] = [
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    AssetName: 'Asset Name',
    Description: 'Description',
    DateTime: '12/12/2023 10:00pm',
  },
];

@Component({
  selector: 'app-movement-history-body',
  templateUrl: './movement-history-body.component.html',
  styleUrls: ['./movement-history-body.component.scss'],
})
export class MovementHistoryBodyComponent implements OnInit {
  constructor(private sanitized: DomSanitizer, private router: Router) {}

  ngOnInit(): void {}

  displayedColumns: string[] = [
    'AssetName',
    'Description',
    'DateTime',
    'Action',
  ];
  dataSource = ELEMENT_DATA;

  goToViewMovement() {
    this.router.navigateByUrl('/view-movement-history');
  }

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
