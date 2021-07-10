import { HttpClient } from '@angular/common/http';
import { SplitInterpolation } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { User } from 'src/app/_models/User';
import { AccountService } from './_services/account.service';
import { PresenceService } from './_services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'DatingApp';
  users:any;

  constructor(private http:HttpClient,private accountService:AccountService,private presence:PresenceService) {}

  ngOnInit() {
    this.setCurrentUser();

  }

  setCurrentUser(){
    const user :User = JSON.parse(localStorage.getItem('user'));
    if (user)
    {
      this.accountService.setCurrentUser(user);
      this.presence.createHubConnection(user)

    }

  }
}
