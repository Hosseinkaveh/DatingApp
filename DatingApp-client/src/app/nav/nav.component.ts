import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

 model:any={};


  constructor(public accountService:AccountService,private route:Router,private toastrService:ToastrService) { }

  ngOnInit(): void {
  }

  login(){
    this.accountService.login(this.model).subscribe(response =>{
      this.route.navigateByUrl('/members');
    },error=>{
      console.log(error);
      this.toastrService.error(error.error,'bad');
    });
  }

  LogOut(){
    this.accountService.LogOut();
    this.route.navigateByUrl('/');

  }


}
