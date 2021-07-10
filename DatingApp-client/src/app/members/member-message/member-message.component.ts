
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/messages';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-message',
  templateUrl: './member-message.component.html',
  styleUrls: ['./member-message.component.css']
})
export class MemberMessageComponent implements OnInit {
  @ViewChild('messageForm') messageForm:NgForm;
  @Input() messages:Message[];
  @Input() username:string;
  messageContent:string;



  constructor(public messageService:MessageService) { }

  ngOnInit(): void {

  }
  sendMessage(){
    this.messageService.createMessage(this.username,this.messageContent).then(() =>{
      this.messageForm.reset();
    });


  }



}
