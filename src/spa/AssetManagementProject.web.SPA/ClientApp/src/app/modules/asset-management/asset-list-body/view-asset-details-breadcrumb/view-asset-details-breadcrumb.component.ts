import { Component, OnInit, Input} from '@angular/core';
  import { DomSanitizer } from '@angular/platform-browser';
  
  
  @Component({
    selector: 'app-view-asset-details-breadcrumb',
    templateUrl: './view-asset-details-breadcrumb.component.html',
    styleUrls: ['./view-asset-details-breadcrumb.component.scss']
  })
  
  export class ViewAssetDetailsBreadcrumbComponent implements OnInit {
    
    
  
    constructor(private sanitized: DomSanitizer ) { }
  
    ngOnInit(): void {
    }

    
  
    innerHTMLSanitizer(value?: any){
      return this.sanitized.bypassSecurityTrustHtml(value)
    }
  }

  

  