import { Component, OnInit, Input} from '@angular/core';
  import { DomSanitizer } from '@angular/platform-browser';
  
  
  @Component({
    selector: 'app-maintenance-record-breadcrumb',
    templateUrl: './maintenance-record-breadcrumb.component.html',
    styleUrls: ['./maintenance-record-breadcrumb.component.scss']
  })
  
  export class MaintenanceRecordBreadcrumbComponent implements OnInit {
    
    
  
    constructor(private sanitized: DomSanitizer ) { }
  
    ngOnInit(): void {
    }

    
  
    innerHTMLSanitizer(value?: any){
      return this.sanitized.bypassSecurityTrustHtml(value)
    }
  }

  

  