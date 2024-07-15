import { Injectable } from '@angular/core';
import { BaseHttpService } from './base-http.service';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuoteService {
  private apiUrl = 'quotes';

  constructor(private baseHttp: BaseHttpService) { }

  getConfig(): Observable<QuoteConfig> {
    return this.baseHttp.get<QuoteConfig>(`${this.apiUrl}/config`).pipe(
      tap((config) => console.log("in service", config))
    );
  }

  createQuote(data: QuoteRequest): Observable<number> {
    return this.baseHttp.post<QuoteRequest, number>(`${this.apiUrl}`, data);
  }

  validateQuote(data: QuoteValidationRequest): Observable<QuoteValidationResult> {
    return this.baseHttp.post(`${this.apiUrl}/validateQuote`, data);
  }

  getAllQuotes(): Observable<QuoteDto[]> {
    return this.baseHttp.get<QuoteDto[]>(`${this.apiUrl}`);
  }
}

export interface QuoteConfig {
  modes: Record<number, string>;
  movementTypes: Record<number, string>;
  incoterms: Record<number, string>;
  lengthUnits: Record<number, string>;
  weightUnits: Record<number, string>;
  currencies: Record<number, string>;
  cities: Record<number, string>;
  packageTypes: Record<number, string>;
}

export interface QuoteDto {
  mode: string;
  movementType: string;
  incoterms: string;
  lengthUnit: string;
  weightUnit: string;
  currency: string;
  city: string;
  packageType: string;
}

export interface QuoteRequest {
  mode: number;
  movementType: number;
  incoterms: number;
  city: number;
  packageType: number;
  unit1: number;
  unit2: number;
  currency: number;
  count: number;
}

export interface QuoteValidationRequest {
  packageType: number;
  count: number;
  mode: number;
}

export interface QuoteValidationResult {
  valid: boolean;
  reason: string;
}
