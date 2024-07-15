import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class InchToCmCalculatorService {

  constructor() { }

  inchToCm(inch: number): number {
    return inch * 2.54;
  }
}
