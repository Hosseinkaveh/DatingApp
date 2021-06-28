import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/messages';
import { leadingComment } from '@angular/compiler';


@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs',{static:true}) memberTabs:TabsetComponent;
  member: member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab:TabDirective;
  messages:Message[] = [];

  constructor(private memberService : MembersService, private router : ActivatedRoute,
    private messageService:MessageService,private route:ActivatedRoute) {

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
        this.loadMessages();
    }
  }
  selectTab(tabid:number)
  {
    this.memberTabs.tabs[tabid].active = true;
  }

}
