import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { API_URL, AUTH_KEY } from '../shared/constants';

export interface ApiResponse<T> {
  succeeded: boolean;
  result: T;
  errors: string[];
}

@Injectable({
  providedIn: 'root',
})
export class BaseHttpService {
  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem(AUTH_KEY);
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });
  }

  get<TRes>(endpoint: string): Observable<TRes> {
    return this.http
      .get<ApiResponse<TRes>>(`${API_URL}/${endpoint}`, {
        headers: this.getHeaders(),
      })
      .pipe(
        map(response => this.handleResponse(response)),
        catchError(this.handleError)
      );
  }

  post<TReq, TRes>(endpoint: string, data: TReq): Observable<TRes> {
    return this.http
      .post<ApiResponse<TRes>>(`${API_URL}/${endpoint}`, data, {
        headers: this.getHeaders(),
      })
      .pipe(
        map(response => this.handleResponse(response)),
        catchError(this.handleError)
      );
  }

  put<TReq, TRes>(endpoint: string, data: TReq): Observable<TRes> {
    return this.http
      .put<ApiResponse<TRes>>(`${API_URL}/${endpoint}`, data, {
        headers: this.getHeaders(),
      })
      .pipe(
        map(response => this.handleResponse(response)),
        catchError(this.handleError)
      );
  }

  delete<TRes>(endpoint: string): Observable<TRes> {
    return this.http
      .delete<ApiResponse<TRes>>(`${API_URL}/${endpoint}`, {
        headers: this.getHeaders(),
      })
      .pipe(
        map(response => this.handleResponse(response)),
        catchError(this.handleError)
      );
  }

  private handleResponse<T>(response: ApiResponse<T>): T {
    if (response.succeeded) {
      return response.result;
    } else {
      throw new Error(response.errors.join(', '));
    }
  }

  private handleError(error: any): Observable<never> {
    console.error(error);
    return throwError(() => new Error('Server error'));
  }
}

export interface ApiResponse<T> {
  succeeded: boolean;
  result: T;
  errors: string[];
}
