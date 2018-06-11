import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/observable';
import { AuthProviderBase } from './auth-provider-base';
import { LinkedInAuthProvider } from './linkedin-auth.provider';

import { environment } from '../../environments/environment';

const AccessToken_CacheKey = 'access_token';
const User_CacheKey = 'user_info';

@Injectable()
export class AuthService {
  isLoggedIn = false;
  redirectUrl: string;

  public static getProvider(accountType: string, authService: AuthService)
    : AuthProviderBase {
    switch (accountType) {
      case 'linkedin':
        return new LinkedInAuthProvider(authService);
    }
  }

  constructor(private http: HttpClient) {

  }

  get accessToken(): string {
    return localStorage.getItem(AccessToken_CacheKey);
  }

  set accessToken(token: string) {
    localStorage.setItem(AccessToken_CacheKey, token);
  }

  login(): Observable<boolean> {
    return null;
  }

  logout(): void {
    this.isLoggedIn = false;
  }

  public fetchUserInfo(type: string, code: string) {
    this.http.post(`${environment.apiServerUrl}/auth`, { accountType: type, code: code });
  }

  // public isAccessTokenValid(): boolean {
  //   const accessToken = this.accessToken;
  //   return tokenNotExpired(null, accessToken);
  // }
}