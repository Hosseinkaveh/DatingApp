import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-membercard',
  templateUrl: './membercard.component.html',
  styleUrls: ['./membercard.component.css']
})
export class MembercardComponent implements OnInit {

  @Input() member: member;

  constructor(private memberService: MembersService, private toastrService: ToastrService) { }

  ngOnInit(): void {
  }

  addLike(member: member) {
    this.memberService.addLike(member.userName).subscribe(x => {
      this.toastrService.success('You have like' + member.userName);
    });

  }

}
