import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzFormModule, NzFormItemComponent, NzFormLabelComponent } from 'ng-zorro-antd/form';
import { Router } from '@angular/router';
import { QuoteConfig, QuoteRequest, QuoteService, QuoteValidationResult } from '../../../services/quote.service';
import { NzOptionComponent, NzSelectModule } from 'ng-zorro-antd/select';
import { CommonModule, KeyValuePipe } from '@angular/common';
import { NzToolTipComponent, NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs';

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
  ]
})
export class QuoteFormComponent implements OnInit {
  form!: FormGroup;
  config: QuoteConfig | undefined;
  ready: boolean = false;
  validationError: string | null = null; // Add this line

  constructor(
    private fb: FormBuilder,
    private quoteService: QuoteService,
    private message: NzMessageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      mode: [null, [Validators.required]],
      movementType: [null, [Validators.required]],
      incoterms: [null, [Validators.required]],
      city: [null, [Validators.required]],
      packageType: [null, [Validators.required]],
      unit1: [null, [Validators.required]],
      unit2: [null, [Validators.required]],
      currency: [null, [Validators.required]],
      count: [null, [Validators.required, Validators.min(1)]],
    });

    this.quoteService.getConfig().subscribe({
      next: (config) => {
        this.config = config;
        this.initializeFormWithConfig(config);
        console.log("quote component", config);
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
      unit2: Object.keys(config.weightUnits)[0],
      currency: Object.keys(config.currencies)[0],
      city: Object.keys(config.cities)[0],
      packageType: Object.keys(config.packageTypes)[0]
    });
  }

  setupValidationTriggers(): void {
    this.form.valueChanges.pipe(
      debounceTime(1000),
      distinctUntilChanged(),
      tap(() => console.log("form valid", this.form.valid)),
      switchMap(() => this.validateQuote())
    ).subscribe({
      next: (response: QuoteValidationResult) => {
        if (!response.valid) {
          this.validationError = response.reason;
          this.message.error('Validation failed: ' + response.reason);
          this.form.get('count')?.setErrors({ invalid: true });
        } else {
          this.validationError = null;
          this.form.get('count')?.setErrors(null);
        }
      },
      error: (error: any) => {
        this.message.error('Validation request failed: ' + error.message);
        this.validationError = 'Validation request failed: ' + error.message;
        this.form.get('count')?.setErrors({ invalid: true });
      }
    });
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
    if (this.form.valid) {
      const quoteRequest: QuoteRequest = {
        mode: this.form.get('mode')?.value,
        movementType: this.form.get('movementType')?.value,
        incoterms: this.form.get('incoterms')?.value,
        city: this.form.get('city')?.value,
        packageType: this.form.get('packageType')?.value,
        unit1: this.form.get('unit1')?.value,
        unit2: this.form.get('unit2')?.value,
        currency: this.form.get('currency')?.value,
        count: this.form.get('count')?.value,
      };

      this.quoteService.createQuote(quoteRequest).subscribe({
        next: () => {
          this.message.success('Quote request sent successfully! Redirecting to results...', { nzDuration: 3000 });
          setTimeout(() => {
            this.router.navigate(['/result'], { state: { data: quoteRequest } });
          }, 3000);
        },
        error: (error) => {
          this.message.error('Quote request failed: ' + error.message);
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
}
