import { Component, OnInit, Input} from '@angular/core';
  import { DomSanitizer } from '@angular/platform-browser';
  
  
  @Component({
    selector: 'app-maintenance-history-breadcrumb',
    templateUrl: './maintenance-history-breadcrumb.component.html',
    styleUrls: ['./maintenance-history-breadcrumb.component.scss']
  })
  
  export class MaintenanceHistoryBreadcrumbComponent implements OnInit {
    
    
  
    constructor(private sanitized: DomSanitizer ) { }
  
    ngOnInit(): void {
    }

    
  
    innerHTMLSanitizer(value?: any){
      return this.sanitized.bypassSecurityTrustHtml(value)
    }
  }

  

  