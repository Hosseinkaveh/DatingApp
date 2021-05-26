import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators'
import { User } from 'src/_models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl= 'https://localhost:5001/api/';
  private currentUserSource = new ReplaySubject<User>(1);//1 size of buffer
  currentUser$ =  this.currentUserSource.asObservable();

  constructor(private http:HttpClient) { }

  login(model:any){
    return this.http.post(this.baseUrl + 'Account/Login',model).pipe(
      map((response:User)=>{
        const user = response;
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }
  register(model:any){
   return this.http.post(this.baseUrl+'Account/register',model).pipe(
      map((response:User) =>{
        const user = response;
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }
  LogOut(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }


  setCurrentUser(user:User){
    this.currentUserSource.next(user)
  }



}
