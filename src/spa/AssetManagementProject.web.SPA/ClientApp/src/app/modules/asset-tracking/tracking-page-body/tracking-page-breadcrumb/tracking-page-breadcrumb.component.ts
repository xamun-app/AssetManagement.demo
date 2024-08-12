import { Component, OnInit, Input} from '@angular/core';
  import { DomSanitizer } from '@angular/platform-browser';
  
  
  @Component({
    selector: 'app-tracking-page-breadcrumb',
    templateUrl: './tracking-page-breadcrumb.component.html',
    styleUrls: ['./tracking-page-breadcrumb.component.scss']
  })
  
  export class TrackingPageBreadcrumbComponent implements OnInit {
    
    
  
    constructor(private sanitized: DomSanitizer ) { }
  
    ngOnInit(): void {
    }

    
  
    innerHTMLSanitizer(value?: any){
      return this.sanitized.bypassSecurityTrustHtml(value)
    }
  }

  

  