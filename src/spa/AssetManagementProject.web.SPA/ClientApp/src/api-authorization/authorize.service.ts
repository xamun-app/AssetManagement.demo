import { Inject, Injectable } from '@angular/core';

import { UserManager, UserManagerSettings, User, WebStorageStateStore } from 'oidc-client';
import { Observable, of } from 'rxjs';

import { Router } from '@angular/router';
import { environment } from '../environments/environment';




@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {

  private manager = new UserManager(getClientSettings());
  private user: User = null;

    constructor(private router: Router) {
    this.manager.getUser().then(user => {
      this.user = user;
    });
  }

  logout() {
    this.user = null;
  }

  isLoggedIn(): boolean {
    return this.user != null && !this.user.expired;
  }

  getClaims(): any {
    return this.user.profile;
  }

  getAuthorizationHeaderValue(): string {
    return `${this.user.token_type} ${this.user.access_token}`;
  }

  startAuthentication(): Promise<void> {
    return this.manager.signinRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      console.log(user);
      this.user = user;
      window.history.replaceState({},
            window.document.title,
            window.location.origin + window.location.pathname);
    });
    }

  getAccessToken(): Observable<string> {
    return of(this.user.access_token);
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    authority: `${environment.identityServerUrl}`,
    client_id: `${environment.clientId}`,
    redirect_uri: `${environment.redirectUri}`,
    post_logout_redirect_uri: `${environment.postLogoutRedirectUri}`,
    response_type: "code",
    scope: "openid profile BlastAsiaCatalyst_api",
    filterProtocolClaims: true,
    loadUserInfo: true,
    userStore: new WebStorageStateStore({
      store: localStorage
    }),
    automaticSilentRenew: true
  };
}
