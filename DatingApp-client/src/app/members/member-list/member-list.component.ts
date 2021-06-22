import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { member } from 'src/app/_models/member';
import { Pageination } from 'src/app/_models/PageInation';
import { User } from 'src/app/_models/User';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members:member[];
  pageination:Pageination;
  userParams:UserParams;
  genderList = [{value:'male',display:'Male'},{value:'female',display:'Female'}];

  constructor(private memberService:MembersService) {
    this.userParams = this.memberService.getUserParams();
   }

  ngOnInit(): void {
    this.LoagMembers();
  }

  pageChanged(event:any)
  {
    this.userParams.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    this.LoagMembers();
  }


  LoagMembers(){
    this.memberService.setUserParams(this.userParams);
    
    this.memberService.getMembers(this.userParams).subscribe(response =>{
    this.pageination = response.pageination;
    this.members = response.result;
    })
  }
  resetFilter(){
   this.userParams = this.memberService.resetUserParams();
    this.LoagMembers();
  }

  }


