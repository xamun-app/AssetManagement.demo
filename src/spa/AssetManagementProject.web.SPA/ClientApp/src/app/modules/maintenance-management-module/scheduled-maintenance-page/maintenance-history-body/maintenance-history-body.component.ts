import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

export interface rolesTable {
  Activity: string;
  Cost: string;
  DateTime: string;
}

const ELEMENT_DATA: rolesTable[] = [
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
  {
    Activity: 'Description',
    Cost: 'P10000',
    DateTime: '12/12/2023 10:00pm',
  },
];

@Component({
  selector: 'app-maintenance-history-body',
  templateUrl: './maintenance-history-body.component.html',
  styleUrls: ['./maintenance-history-body.component.scss'],
})
export class MaintenanceHistoryBodyComponent implements OnInit {
  constructor(private sanitized: DomSanitizer) {}

  ngOnInit(): void {}

  displayedColumns: string[] = ['Activity', 'Cost', 'DateTime', 'Action'];
  dataSource = ELEMENT_DATA;

  innerHTMLSanitizer(value?: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
