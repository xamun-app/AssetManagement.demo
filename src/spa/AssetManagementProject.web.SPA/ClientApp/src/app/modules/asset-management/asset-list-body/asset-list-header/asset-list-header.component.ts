import { Component, OnInit, Input} from '@angular/core';
  import { DomSanitizer } from '@angular/platform-browser';
  
  
  @Component({
    selector: 'app-asset-list-header',
    templateUrl: './asset-list-header.component.html',
    styleUrls: ['./asset-list-header.component.scss']
  })
  
  export class AssetListHeaderComponent implements OnInit {
    
    
  
    constructor(private sanitized: DomSanitizer ) { }
  
    ngOnInit(): void {
    }

    
  
    innerHTMLSanitizer(value?: any){
      return this.sanitized.bypassSecurityTrustHtml(value)
    }
  }

  

  