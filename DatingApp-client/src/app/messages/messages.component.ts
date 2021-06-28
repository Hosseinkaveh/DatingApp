import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/messages';
import { Pageination } from '../_models/PageInation';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pageination: Pageination;
  Container = 'Unread'
  pageNumber = 1;
  pageSize = 2;
  loading = false;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.LoadMessage();
  }
  LoadMessage() {
    this.loading = true;
    this.messageService.getMessage(this.pageNumber, this.pageSize, this.Container).subscribe(response => {
      this.pageination = response.pageination
      this.messages = response.result;
      this.loading = false
    })
  }

  pageChange(event: any) {
    this.pageNumber = event.page;
    this.LoadMessage();
  }

  deleteMessage(id:number)
  {
    this.messageService.deleteMessage(id).subscribe(() =>{
      this.messages.splice(this.messages.findIndex(x =>x.id == id),1)
    })
  }

}
