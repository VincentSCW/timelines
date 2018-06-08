import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/observable';

@Injectable()
export class AuthService {
  isLoggedIn = false;
  redirectUrl: string;

  login(): Observable<boolean> {
    return null;
  }

  logout(): void {
    this.isLoggedIn = false;
  }
}