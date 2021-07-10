import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators'
import { User } from 'src/app/_models/User';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl= environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);//1 size of buffer
  currentUser$ =  this.currentUserSource.asObservable();

  constructor(private http:HttpClient,private presence:PresenceService) { }

  login(model:any){
    return this.http.post(this.baseUrl + 'Account/Login',model).pipe(
      map((response:User)=>{
        const user = response;
        if(user){
         this.setCurrentUser(user);
         this.presence.createHubConnection(user)
        }
      })
    );
  }
  register(model:any){
    console.log(model);

   return this.http.post(this.baseUrl+'Account/register',model).pipe(
      map((response:User) =>{
        const user = response;
        if(user){
          this.setCurrentUser(user);
         this.presence.createHubConnection(user)
        }
      })
    )
  }
  LogOut(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();

  }


  setCurrentUser(user:User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles: user.roles.push(roles);
    localStorage.setItem('user',JSON.stringify(user));
    this.currentUserSource.next(user)
  }

  getDecodedToken(token){
    return JSON.parse(atob(token.split('.')[1]));
  }



}
