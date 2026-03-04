import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class LanguageHeaderInterceptor implements HttpInterceptor {
  // avoid injecting LanguageService to prevent circular DI with HttpClient
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const lang = localStorage.getItem('language') || navigator.language || 'en-US';
    const modified = req.clone({
      setHeaders: {
        'Accept-Language': lang,
      },
    });
    return next.handle(modified);
  }
}
