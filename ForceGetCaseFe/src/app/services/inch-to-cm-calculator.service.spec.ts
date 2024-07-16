import { TestBed } from '@angular/core/testing';

import { InchToCmCalculatorService } from './inch-to-cm-calculator.service';

describe('InchToCmCalculatorService', () => {
  let service: InchToCmCalculatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InchToCmCalculatorService);
  });

  it('should be created', () => {
    let result = service.inchToCm(1);
    expect(result).toBe(2.54);
  });
});
