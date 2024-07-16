import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzFormModule, NzFormItemComponent, NzFormLabelComponent } from 'ng-zorro-antd/form';
import { Router } from '@angular/router';
import { QuoteConfig, QuoteRequest, QuoteService, QuoteValidationResult } from '../../../services/quote.service';
import { NzOptionComponent, NzSelectModule } from 'ng-zorro-antd/select';
import { CommonModule, KeyValuePipe } from '@angular/common';
import { NzToolTipComponent, NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs';
import { NzAutocompleteComponent, NzAutocompleteModule } from 'ng-zorro-antd/auto-complete';
import { NzInputModule } from 'ng-zorro-antd/input';
import { InchToCmCalculatorService } from '../../../services/inch-to-cm-calculator.service';

@Component({
  selector: 'app-quote-form',
  standalone: true,
  templateUrl: './quote-form.component.html',
  styleUrls: ['./quote-form.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzFormItemComponent,
    NzFormLabelComponent,
    NzSelectModule,
    NzOptionComponent,
    KeyValuePipe,
    NzToolTipModule,
    NzToolTipComponent,
    NzIconModule,
    NzAutocompleteModule,
    NzAutocompleteComponent,
    NzInputModule,
  ]
})
export class QuoteFormComponent implements OnInit {
  form!: FormGroup;
  config: QuoteConfig | undefined;
  ready: boolean = false;
  validationError: string | null = null;
  butonDisabled: boolean = false;

  cityOptions: KeyValuePair[] = [];
  filteredCityOptions?: ValueLabelPair[] = [];
  selectedCity?: KeyValuePair;
  centimeter: number = 0;

  constructor(
    private fb: FormBuilder,
    private quoteService: QuoteService,
    private message: NzMessageService,
    private router: Router,
    private inchToCmConverterService: InchToCmCalculatorService,
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      mode: [null, [Validators.required]],
      movementType: [null, [Validators.required]],
      incoterms: [null, [Validators.required]],
      city: [null, [Validators.required, this.validateCity.bind(this)]],
      packageType: [null, [Validators.required]],
      unit1: [null, [Validators.required]],
      length: [null, [Validators.required, Validators.min(1)]],
      unit2: [null, [Validators.required]],
      weight: [null, [Validators.required, Validators.min(1)]],
      currency: [null, [Validators.required]],
      count: [null, [Validators.required, Validators.min(1)]],
    });

    this.quoteService.getConfig().subscribe({
      next: (config) => {
        this.config = config;
        this.cityOptions = Object.entries(this.config.cities).map(([key, val]) => { return { key, value: val }; });
        this.filteredCityOptions = this.cityOptions.map(opt => this.toValueLabel(opt));
        this.initializeFormWithConfig(config);
        this.ready = true;
      },
      error: (error) => {
        this.message.error('Failed to load config: ' + error.message);
      }
    });

    this.setupValidationTriggers();
  }

  initializeFormWithConfig(config: QuoteConfig): void {
    this.form.patchValue({
      mode: Object.keys(config.modes)[0],
      movementType: Object.keys(config.movementTypes)[0],
      incoterms: Object.keys(config.incoterms)[0],
      unit1: Object.keys(config.lengthUnits)[0],
      length: this.form.get('length')?.value,
      unit2: Object.keys(config.weightUnits)[0],
      weight: this.form.get('weight')?.value,
      currency: Object.keys(config.currencies)[0],
      packageType: Object.keys(config.packageTypes)[0]
    });
  }

  setupValidationTriggers(): void {
    this.form.valueChanges.pipe(
      debounceTime(1000),
      distinctUntilChanged(),
      switchMap(() => this.validateQuote())
    ).subscribe({
      next: (response: QuoteValidationResult) => {
        if (!response.valid) {
          this.validationError = response.reason;
          this.message.error('Validation failed: ' + response.reason);
        } else {
          this.validationError = null;
        }
      },
      error: (error: any) => {
        this.message.error('Validation request failed: ' + error.message);
        this.validationError = 'Validation request failed: ' + error.message;
        this.form.get('count')?.setErrors({ invalid: true });
      }
    });

    this.form.get('city')?.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(value => this.filterCities(value));

    this.form.valueChanges.subscribe({
      next: (value) => {
        console.log(value);
        if (value['unit1'] && value['length'] && value['unit1'] == 2) {
          this.centimeter = this.inchToCmConverterService.inchToCm(value['length']);
        } else {
          this.centimeter = value['length'];
        }
      }
    });
  }

  onCityInput(event: Event): void {
    const input = (event.target as HTMLInputElement).value;
    this.filterCities(input);
  }

  filterCities(value: string): void {
    this.filteredCityOptions = this.cityOptions.filter(
      option => option.value.toLowerCase().includes(value.toLowerCase()))
      .map(this.toValueLabel);
  }

  cityChanged(event: any) {
    const selectedCity = this.filteredCityOptions?.find(option => option.value === event?.nzValue);
    if (selectedCity) {
      this.selectedCity = this.toKeyValue(selectedCity);
      this.form.get('city')?.setValue(selectedCity ? selectedCity.value : null);
    } else {
      this.form.get('city')?.valid;
    }
  }

  validateCity(control: AbstractControl): { [key: string]: any; } | null {
    const city = this.cityOptions.find(option => option.key === control.value);
    if (!city) {
      return { invalidCity: true };
    }
    return null;
  }

  validateQuote() {
    const mode = this.form.get('mode')?.value;
    const packageType = this.form.get('packageType')?.value;
    const count = this.form.get('count')?.value;

    if (mode && packageType && count) {
      const validationRequest = { mode, packageType, count };
      return this.quoteService.validateQuote(validationRequest);
    }
    return [];
  }

  onSubmit(): void {
    this.butonDisabled = true;
    if (this.form.valid) {
      const quoteRequest: QuoteRequest = {
        mode: this.form.get('mode')?.value,
        movementType: this.form.get('movementType')?.value,
        incoterms: this.form.get('incoterms')?.value,
        city: parseInt(this.selectedCity?.key ?? "0"),
        packageType: this.form.get('packageType')?.value,
        unit1: this.form.get('unit1')?.value,
        length: this.form.get('length')?.value,
        unit2: this.form.get('unit2')?.value,
        weight: this.form.get('weight')?.value,
        currency: this.form.get('currency')?.value,
        count: this.form.get('count')?.value,
      };

      this.quoteService.createQuote(quoteRequest).subscribe({
        next: () => {
          this.message.success('Quote request sent successfully! Redirecting to results...', { nzDuration: 3000 });
          setTimeout(() => {
            this.router.navigate(['/quote-list'], { state: { data: quoteRequest } });
          }, 3000);
        },
        error: (error) => {
          this.message.error('Quote request failed: ' + error.message);
        },
        complete: () => {
          this.butonDisabled = false;
        }
      });
    } else {
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsTouched();
        }
      });
    }
  }

  toValueLabel(keyVal: KeyValuePair): ValueLabelPair {
    let valLab: ValueLabelPair = {
      value: keyVal.key,
      label: keyVal.value,
    };

    return valLab;
  }

  toKeyValue(valLab: ValueLabelPair): KeyValuePair {
    let keyVal: KeyValuePair = {
      key: valLab.value,
      value: valLab.label,
    };

    return keyVal;
  }
}

type ValueLabelPair = {
  value: string,
  label: string,
};

type KeyValuePair = {
  key: string,
  value: string,
};
