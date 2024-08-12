
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';

const API_URL = `${environment.apiUrl}`;
const WORKFLOW_API_URL = `${environment.workflowApiUrl}`;

@Injectable({
  providedIn: 'root'
})

export class ViewAssetDetailsSidebarService {
  
  constructor(private http: HttpClient) { }

  private getAPIEndpoint(baseURL: string, queryParameter?: any) {
    switch (baseURL) {
      default: return "";
    }
  }


}
