import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';

const API_URL = `${environment.apiUrl}`;
const WORKFLOW_API_URL = `${environment.workflowApiUrl}`;

@Injectable({
  providedIn: 'root',
})
export class AddEditAssetPageService {
  constructor(private http: HttpClient) {}

  private stateSource = new BehaviorSubject<any>(null);

  currentState = this.stateSource.asObservable();

  changeState(newState: string) {
    this.stateSource.next(newState);
  }

  private getAPIEndpoint(baseURL: string, queryParameter?: any) {
    switch (baseURL) {
      default:
        return '';
    }
  }
}
