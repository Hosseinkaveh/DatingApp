import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { member } from '../_models/member';
import { Photo } from '../_models/Photo';

// const options ={
//   headers:new HttpHeaders({
//     Authorization:'Bearer '+JSON.parse(localStorage.getItem('user'))?.token
//   })

// }
@Injectable({
  providedIn: 'root'
})

export class MembersService {
  baseUrl = environment.apiUrl;
  member:member[]=[];

  constructor(private http:HttpClient) { }

  getMembers(){
    if(this.member.length > 0) {
      return of(this.member)
    }
   return this.http.get<member[]>(this.baseUrl + 'Users').pipe(
     map(members =>{
       this.member = members;
       return members;
       })
   )
  }

  getMember(username:string){
   const member =  this.member.find(x =>x.userName === username);
   if(member !== undefined) return of(member)
    return this.http.get<member>(this.baseUrl + 'Users/' + username )
   }

   updateMember(member:member)
   {
     return this.http.put(this.baseUrl+'users',member).pipe(
       map(()=>{
         const index = this.member.indexOf(member);
         this.member[index] = member;
       })
     );
   }
   UpdateMainPhoto(photoid){
     return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoid,{});

   }

   DeletePhoto(photoid)
   {
    return this.http.delete(this.baseUrl +'Users/delete-photo/'+ photoid)
   }




}
