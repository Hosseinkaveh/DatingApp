import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { member } from '../_models/member';
import { PageInationResult } from '../_models/PageInation';
import { User } from '../_models/User';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { getPageinationResult, setParams } from './pageinationHelper';

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
  members: member[] = [];
  user: User;
  userParams: UserParams;
  memberCatch = new Map();

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(x => {
      this.user = x;
      this.userParams = new UserParams(this.user);
    })


  }

  getMembers(userParams: UserParams) {
    var response = this.memberCatch.get(Object.values(userParams).join('-'))
    if (response) {
      return of(response);
    }

    let params = setParams(userParams.pageNumber, userParams.pageSize);

    params = params.append('gender', userParams.gender);
    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('OrderBy', userParams.orderBy);

    return getPageinationResult<member[]>(this.baseUrl + 'Users', params, this.http)
      .pipe(map(response => {
        this.memberCatch.set(Object.values(userParams).join('-'), response);
        return response;
      }))
  }

  getMember(username: string) {
    const member = [...this.memberCatch.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: member) => member.userName === username);
    if (member) {
      return of(member)
    }

    return this.http.get<member>(this.baseUrl + 'Users/' + username)
  }

  updateMember(member: member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
  UpdateMainPhoto(photoid) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoid, {});

  }

  DeletePhoto(photoid) {
    return this.http.delete(this.baseUrl + 'Users/delete-photo/' + photoid)
  }

  getUserParams() {
    return this.userParams;
  }
  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }
  setUserParams(params: UserParams) {
    this.userParams = params;

  }

  addLike(userName: string) {
    return this.http.post(this.baseUrl + 'Likes/' + userName, {});
  }

  getLikes(predicate: string, PageNumber: number, PageSize: number) {
    var params = setParams(PageNumber, PageSize);
    params = params.append('predicate', predicate);
    return getPageinationResult<Partial<member[]>>(this.baseUrl + "Likes/", params, this.http);

  }




}
