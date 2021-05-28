import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, retry } from 'rxjs/operators';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastrService:ToastrService,private route:Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error =>{
        if(error)
        {
          switch (error.status) {
            case 400:
              if(error.error.errors){
                const modalStateErrors = [];
                for(const key in error.error.errors){
                  if (error.error.errors[key]) {
                    modalStateErrors.push(error.error.errors[key]);
                  }

                }
                throw modalStateErrors.flat();

              }else if(typeof(error.error) === 'object'){
                this.toastrService.error(error.statusText==="OK"?"BadReques":error.statusText,error.status)

              }else{
                this.toastrService.error(error.error, error.status);
              }
              break;

              case 401:
                this.toastrService.error(error.statusText==="OK"?"Unauthorised":error.statusText,error.status)
                break;

                case 404:
                  this.route.navigateByUrl('/not-found')
                  break;

                  case 500:
                    const navigateExtras:NavigationExtras={state:{error:error.error}};
                    this.route.navigateByUrl('/server-error',navigateExtras);
                    break;

                    default:
                      this.toastrService.error('Something unexpected went wrong');
                      console.log(error);
                      break;
          }
        }
        return throwError(error)
      })

    );
  }
}
