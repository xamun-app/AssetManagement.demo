import { Component, OnInit, Input} from '@angular/core';
  import { DomSanitizer } from '@angular/platform-browser';
  
  
  @Component({
    selector: 'app-asset-maintenance-report-breadcrumb',
    templateUrl: './asset-maintenance-report-breadcrumb.component.html',
    styleUrls: ['./asset-maintenance-report-breadcrumb.component.scss']
  })
  
  export class AssetMaintenanceReportBreadcrumbComponent implements OnInit {
    
    
  
    constructor(private sanitized: DomSanitizer ) { }
  
    ngOnInit(): void {
    }

    
  
    innerHTMLSanitizer(value?: any){
      return this.sanitized.bypassSecurityTrustHtml(value)
    }
  }

  

  