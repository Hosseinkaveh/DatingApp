import { Injectable } from '@angular/core';
import {  CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private toastrService:ToastrService,private accoutnService:AccountService) {}

  canActivate(): Observable<boolean> {
   return this.accoutnService.currentUser$.pipe(
      map( user => {
        if (user) return true;
        this.toastrService.error('You shall not pass');
      })
    )

  }

}
