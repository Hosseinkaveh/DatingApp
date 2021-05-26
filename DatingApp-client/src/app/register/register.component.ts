import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model:any={};
  @Output() CancelReg = new EventEmitter();
  constructor(private accountService:AccountService) { }

  ngOnInit(): void {
  }

  CancelRegister(){
    this.CancelReg.emit(false)
  }
  Register(){
    this.accountService.register(this.model).subscribe(x =>{
      this.CancelRegister();
    },error=>{
      console.log(error);

    });
  }

}
