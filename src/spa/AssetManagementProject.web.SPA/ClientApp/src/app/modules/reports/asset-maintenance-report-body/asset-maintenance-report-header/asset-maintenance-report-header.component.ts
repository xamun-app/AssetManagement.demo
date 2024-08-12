import { Component, OnInit, Input} from '@angular/core';
  import { DomSanitizer } from '@angular/platform-browser';
  
  
  @Component({
    selector: 'app-asset-maintenance-report-header',
    templateUrl: './asset-maintenance-report-header.component.html',
    styleUrls: ['./asset-maintenance-report-header.component.scss']
  })
  
  export class AssetMaintenanceReportHeaderComponent implements OnInit {
    
    
  
    constructor(private sanitized: DomSanitizer ) { }
  
    ngOnInit(): void {
    }

    
  
    innerHTMLSanitizer(value?: any){
      return this.sanitized.bypassSecurityTrustHtml(value)
    }
  }

  

  