import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { createMessage } from '../_models/createMessage';
import { Message } from '../_models/messages';
import { PageInationResult } from '../_models/PageInation';
import { getPageinationResult, setParams } from './pageinationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMessage(pageNumber, pageSize, container) {
    let params = setParams(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPageinationResult<Message[]>(this.baseUrl + 'message', params, this.http);
  }

  getMessageThread(uesrname:string)
  {
    return this.http.get<Message[]>(this.baseUrl + 'message/thread/'+ uesrname)
  }
  createMessage(username:string,content:string){
    return this.http.post<Message>(this.baseUrl+'message',{RecipieantUserName:username,content});
  }

  deleteMessage(id:number)
  {
    return this.http.delete(this.baseUrl+'message/'+id)
  }
}
