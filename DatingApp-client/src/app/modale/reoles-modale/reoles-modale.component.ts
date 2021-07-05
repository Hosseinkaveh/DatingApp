import { Component, Input, OnInit ,EventEmitter} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-reoles-modale',
  templateUrl: './reoles-modale.component.html',
  styleUrls: ['./reoles-modale.component.css']
})
export class ReolesModaleComponent implements OnInit {

  @Input() updateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[];

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  updateRoles() {
    console.log(this.roles);
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }
  changes(){
    console.log(this.roles);
  }
}
