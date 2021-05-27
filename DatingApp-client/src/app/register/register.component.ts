import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model:any={};
  @Output() CancelReg = new EventEmitter();
  constructor(private accountService:AccountService,private toastrService:ToastrService) { }

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
      this.toastrService.error(error.error)
    });
  }

}
