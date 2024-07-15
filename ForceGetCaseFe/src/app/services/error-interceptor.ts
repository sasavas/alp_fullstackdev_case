import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { catchError, Observable, tap } from "rxjs";
import { NzMessageService } from 'ng-zorro-antd/message';
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private message: NzMessageService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status.toString().startsWith("4")) {
          this.message.error('Bad Request. Please try again.');
        } else if (error.status.toString().startsWith("5")) {
          this.message.error('Server Error. Redirecting to error page.');
          this.router.navigate(['/error']);
        } else {
          this.message.error(`Error ${error.status}: ${error.message}`);
          this.router.navigate(['/error']);
        }
        throw error;
      })
    );
  }
}
