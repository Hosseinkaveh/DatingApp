import { Component, OnInit } from '@angular/core';
import { member } from '../_models/member';
import { Pageination } from '../_models/PageInation';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  member: Partial<member[]>
  predicate = 'liked'
  pageNumber = 1;
  pageSize = 2;
  pageInation: Pageination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    console.log(this.predicate);

    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe(response => {
      this.member = response.result;
      this.pageInation = response.pageination;
    })

  }
  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadLikes();
  }

}
