import { Injectable, ErrorHandler, Inject, Injector } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AppErrorHandler implements ErrorHandler {
  constructor(@Inject(Injector) private readonly injector: Injector) {}

  handleError(error: any): void {
    this.injector
      .get(ToastrService)
      .error('Error', error.message || 'An unexpected error occurred');
  }
}
