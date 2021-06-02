import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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
  validationErrors:string[]=[];

  constructor(private accountService:AccountService,private toastrService:ToastrService,
    private fb:FormBuilder,private router:Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      Gender: ['mail'],
      username: ['',Validators.required],
      KnownAs: ['',Validators.required],
      DateOfBirth: ['',Validators.required],
      City: ['',Validators.required],
      Contry: ['',Validators.required],
      password: ['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword: ['',[Validators.required,this.matchValue('password')]]
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
    this.accountService.register(this.registerForm.value).subscribe(x =>{
        this.router.navigateByUrl('/members');
    },error=>{
      this.validationErrors = error;
    });
  }

}
