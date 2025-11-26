import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  constructor(private toastr: ToastrService) {}

  handle(error: any) {
    if (!error || !error.status) {
      this.toastr.error('Unknown error occurred', 'Error');
      return;
    }

    switch (error.status) {
      case 400:
        this.toastr.warning(error.error?.message || 'Bad Request', 'Warning');
        break;

      case 401:
        this.toastr.error('You are not authorized. Please login again.', 'Unauthorized');
        break;

      case 403:
        this.toastr.error('You don’t have permission to do this.', 'Forbidden');
        break;

      case 404:
        this.toastr.info('The requested resource was not found.', 'Not Found');
        break;

      case 500:
        this.toastr.error('Internal Server Error. Try again later.', 'Server Error');
        break;

      default:
        this.toastr.error(error.error?.message || 'Something went wrong', 'Error');
        break;
    }
  }
}
