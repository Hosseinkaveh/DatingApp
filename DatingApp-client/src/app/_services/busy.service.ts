import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount=0;

  constructor(private busyService:NgxSpinnerService) { }

  busy(){
    this.busyRequestCount++;
    this.busyService.show(undefined,{
      type:'ball-pulse',
      bdColor:'rgba(0, 0, 0, 0.8)',
      color:'#fff',
      fullScreen:true,
      size:'medium',
    })

  }

  idle(){
    this.busyRequestCount--;
    if(this.busyRequestCount <= 0){
      this.busyRequestCount = 0;
      this.busyService.hide();
    }

  }

}
