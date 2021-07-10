import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { createMessage } from '../_models/createMessage';
import { Group } from '../_models/group';
import { Message } from '../_models/messages';
import { PageInationResult } from '../_models/PageInation';
import { User } from '../_models/User';
import { getPageinationResult, setParams } from './pageinationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl = environment.apiUrl;
  hubUrl = environment.hubsUrl;
  private hubConnection:HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient) { }

  createHubConnection(user:User,otherUsername:string)
  {
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl+'message?user='+otherUsername,{
      accessTokenFactory: ()=>user.token
    })
    .withAutomaticReconnect()
    .build()

    this.hubConnection.start().catch(error =>console.log(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSource.next(messages);
    })

    this.hubConnection.on('NewMessage',message =>{
      this.messageThread$.pipe(take(1)).subscribe(messages =>{
        this.messageThreadSource.next([...messages,message])
      })
    })
    this.hubConnection.on('UpdatedGroup',(group:Group)=>{
      if(group.connection.some(x =>x.username === otherUsername)){
        this.messageThread$.pipe(take(1)).subscribe(messages =>{
          messages.forEach(message =>{
            if(!message.dateRead){
              message.dateRead = new Date(Date.now())
            }
          })
          this.messageThreadSource.next([...messages]);
        })
      }
    })

  }
  stopHubConnection()
  {
    if(this.hubConnection){
      this.hubConnection.stop();
    }
  }

  getMessage(pageNumber, pageSize, container) {
    let params = setParams(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPageinationResult<Message[]>(this.baseUrl + 'message', params, this.http);
  }

  getMessageThread(uesrname:string)
  {
    return this.http.get<Message[]>(this.baseUrl + 'message/thread/'+ uesrname)
  }
 async createMessage(username:string,content:string){
    return this.hubConnection.invoke('SendMessage',{RecipieantUserName:username,content})
    .catch(error =>console.log(error));
  }

  deleteMessage(id:number)
  {
    return this.http.delete(this.baseUrl+'message/'+id)
  }
}
