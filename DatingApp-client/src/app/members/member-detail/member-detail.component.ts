import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/messages';
import { leadingComment } from '@angular/compiler';
import { PresenceService } from 'src/app/_services/presence.service';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs/operators';
import { Route } from '@angular/compiler/src/core';


@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit,OnDestroy {
  @ViewChild('memberTabs',{static:true}) memberTabs:TabsetComponent;
  member: member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab:TabDirective;
  messages:Message[] = [];
  user: User;

  constructor(public presence:PresenceService,private messageService:MessageService
    ,private route:ActivatedRoute,private accountService: AccountService,
    private router:Router) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
      this.router.routeReuseStrategy.shouldReuseRoute =()=>false;
   }

  ngOnInit(): void {
    this.route.data.subscribe(data =>{
      this.member = data.member;
    })

    this.route.queryParams.subscribe(params =>{
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })



    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]
    this.galleryImages = this.getImages();
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      })
    }
    return imageUrls;

  }

  loadMessages()
  {
    this.messageService.getMessageThread(this.member.userName).subscribe(message =>{
      this.messages = message;
    })
  }

  onTabActivated(data:TabDirective)
  {
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.messages.length === 0)
    {
      this.messageService.createHubConnection(this.user, this.member.userName);
    } else {
      this.messageService.stopHubConnection();
    }
  }
  selectTab(tabid:number)
  {
    this.memberTabs.tabs[tabid].active = true;
  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }


}
