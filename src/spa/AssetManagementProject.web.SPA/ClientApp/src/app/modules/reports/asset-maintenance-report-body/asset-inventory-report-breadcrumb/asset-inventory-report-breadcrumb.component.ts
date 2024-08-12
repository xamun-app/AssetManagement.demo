import { Component, OnInit, Input} from '@angular/core';
  import { DomSanitizer } from '@angular/platform-browser';
  
  
  @Component({
    selector: 'app-asset-inventory-report-breadcrumb',
    templateUrl: './asset-inventory-report-breadcrumb.component.html',
    styleUrls: ['./asset-inventory-report-breadcrumb.component.scss']
  })
  
  export class AssetInventoryReportBreadcrumbComponent implements OnInit {
    
    
  
    constructor(private sanitized: DomSanitizer ) { }
  
    ngOnInit(): void {
    }

    
  
    innerHTMLSanitizer(value?: any){
      return this.sanitized.bypassSecurityTrustHtml(value)
    }
  }

  

  