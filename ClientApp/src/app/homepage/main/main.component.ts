import { Component, OnInit, ElementRef, ViewChild, AfterViewChecked } from '@angular/core';
import { HttpService } from '../../http.service';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import {ActivatedRoute, Router} from '@angular/router';
import { elementStyleProp } from '@angular/core/src/render3';
@Component({
  selector: 'app-task',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class mainComponent implements OnInit {

  constructor(private _httpService: HttpService, private route: ActivatedRoute, private routing: Router) { }
  showAlert:boolean;
  showCircles:boolean;
  showSettings:boolean;
  circles:any;
  currentCircle:any;
  currentChannel:any;
  currentUser:any;
  // ================================================
  ngOnInit() {
    this.showAlert = false
    this.showCircles = false;
    this.showSettings = false;
    this.getUserCircles();
  }
  // ================================================
  alertClicked(){
    if(this.showCircles === true){
      this.showCircles = false;
    }
    if(this.showAlert === false){
      this.showAlert = true;
    }
    else{
      this.showAlert = false;
    }
  }
  circleClicked(){
    if(this.showAlert === true){
      this.showAlert = false;
    }
    if(this.showCircles === false){
      this.showCircles = true;
    }
    else{
      this.showCircles = false;
    }
  }
  settingsClicked(){
    if(this.showSettings === false){
      this.showSettings = true;
    }
    else{
      this.showSettings = false;
    }
  }
  channelClicked(id){
    for(var chan in this.currentCircle.circle.channels){
      if(this.currentCircle.circle.channels[chan].channelId === id){
        this.currentChannel = this.currentCircle.circle.channels[chan];
      }
    }
  }
  
  // ================================================
  getUserCircles(){
    this._httpService.getUserCircles().subscribe(data => {
      if(data['Message'] === "Error"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
      else{
          this.currentUser = data['user'];
          if(data['usercircles'].length > 0){
            this.circles = data['usercircles'];
            this.currentCircle = data['usercircles'][0];
            this.currentChannel = this.currentCircle.circle.channels[0];
        }
      }
    })
  }

  openCircle(id){
    for(var cir in this.circles){
      if(this.circles[cir].circle.circleId === id){
        this.currentCircle = this.circles[cir];
        this.currentChannel = this.currentCircle.circle.channels[0];
      }
    }
  }
  joinCircle(id){
    this._httpService.joinCirc(id).subscribe(data => {
      if(data['Message'] === "Error"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
      else{
        this.getUserCircles();
      }
    })
  }
  deleteChannel(id){
    this._httpService.removeChannel(id, this.currentCircle.circle.circleId).subscribe(data => {
      if(data['Message'] === "Error"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
      else{
        this.getUserCircles();
      }
    })
  }
  // ================================================
  subbed(){
    this._httpService.getUserCircles().subscribe(data => {
      if(data['Message'] === "Error"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
      else{
        this.currentUser = data['user'];
        this.circles = data['usercircles'];
        //=====================================
        for(var cir in this.circles){
          if(this.circles[cir].circle.circleId === this.currentCircle.circleId){
            this.currentCircle = this.circles[cir];
          }
        }
        for(var chan in this.currentCircle.circle.channels){
          if(this.currentCircle.circle.channels[chan].channelId === this.currentChannel.channelId){
            this.currentChannel = this.currentCircle.circle.channels[chan];
          }
        }
        //=====================================
      }
    })
  }
  // ================================================
  logoutClick(){
    this._httpService.LogoutUser().subscribe(data => {
      this.routing.navigate(['/']);
    })
  }
}
