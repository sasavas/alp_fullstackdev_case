import { Routes } from '@angular/router';
import { LoginComponent } from './pages/auth/login/login.component';
import { RegisterComponent } from './pages/auth/register/register.component';
import { QuoteListComponent } from './pages/main-app/quote-list/quote-list.component';
import { QuoteFormComponent } from './pages/main-app/quote-form/quote-form.component';
import { AuthGuard } from './services/auth.guard';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/quote' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'quote', component: QuoteFormComponent, canActivate: [AuthGuard] },
  { path: 'quote-list', component: QuoteListComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '/error' }
];
