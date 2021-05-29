import { Component, Input, OnInit } from '@angular/core';
import { member } from 'src/app/_models/member';

@Component({
  selector: 'app-membercard',
  templateUrl: './membercard.component.html',
  styleUrls: ['./membercard.component.css']
})
export class MembercardComponent implements OnInit {

  @Input() member:member;

  constructor() { }

  ngOnInit(): void {
  }

}
