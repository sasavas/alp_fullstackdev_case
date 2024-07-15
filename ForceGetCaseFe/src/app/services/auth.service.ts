import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { API_URL, AUTH_KEY } from '../shared/constants';
import { ApiResponse } from './base-http.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private relativePath = "Users";

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json',
    });
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('AUTH_KEY'); // Replace 'AUTH_KEY' with your actual token key
    return !!token; // Returns true if token exists, otherwise false
  }

  register(createUserModel: CreateUserModel): Observable<CreateUserModel> {
    return this.http.post<CreateUserModel>(
      `${API_URL}/${this.relativePath}/register`,
      createUserModel,
      { headers: this.getHeaders() }
    );
  }

  authenticate(loginUserModel: LoginUserModel): Observable<ApiResponse<LoginResponseModel>> {
    return this.http.post(
      `${API_URL}/${this.relativePath}/authenticate`,
      loginUserModel,
      { headers: this.getHeaders() }
    ).pipe(
      tap((loginResponse: any) => {
        console.log("loginResponse", loginResponse.result);
        localStorage.setItem(AUTH_KEY, loginResponse.result.token);
      })
    );
  }
}

export interface CreateUserModel {
  email: string;
  password: string;
}

export interface LoginUserModel {
  email: string;
  password: string;
}

export interface LoginResponseModel {
  email: string;
  token: string;
}
