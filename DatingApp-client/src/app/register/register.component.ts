import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model:any={};
  registerForm:FormGroup;
  @Output() CancelReg = new EventEmitter();

  constructor(private accountService:AccountService,private toastrService:ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      username: new FormControl('',Validators.required),
      password: new FormControl('',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]),
      confirmPassword: new FormControl('',[Validators.required,this.matchValue('password')])
    })
    this.registerForm.controls['password'].valueChanges.subscribe(() =>{
      this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })

  }

  matchValue(matchTo:string):ValidatorFn
  {
    return (control:AbstractControl) =>{
      return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching:true}
    }
  }

  CancelRegister(){
    this.CancelReg.emit(false)
  }

  Register(){
    console.log(this.registerForm.value);

    // this.accountService.register(this.model).subscribe(x =>{
    //   this.CancelRegister();
    // },error=>{
    //   console.log(error);
    //   this.toastrService.error(error.error)
    // });
  }

}
