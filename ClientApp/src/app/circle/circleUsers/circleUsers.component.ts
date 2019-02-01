import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../http.service';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import {ActivatedRoute, Router} from '@angular/router';
@Component({
  selector: 'app-circle',
  templateUrl: './circleUsers.component.html',
  styleUrls: ['./circleUsers.component.css']
})
export class circleUsersComponent implements OnInit {

  constructor(private _httpService: HttpService, private route: ActivatedRoute, private routing: Router) { }
  circleUsers:any;
  userMessages:any;
  circId:any;
  order: string = 'likes';
  ngOnInit() {
    this.route.params.subscribe((res)=>{
      this.circId = res.id;
    })
    this.getUsers();
    this.userMessages = [];
  }
  getUsers(){
    this._httpService.manageUsers(this.circId).subscribe(data => {
      if(data['Message'] === "Error"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
      else{
        this.circleUsers = data;
      }
    })
  }
  activity(id){
    this.userMessages = [];
    for(var circ in this.circleUsers){
      if(this.circleUsers[circ].userId == id){
        for(var chan in this.circleUsers[circ].circle.channels){
          for(var mes in this.circleUsers[circ].circle.channels[chan].messages){
            if(this.circleUsers[circ].circle.channels[chan].messages[mes].userId == id){
              this.userMessages.push(this.circleUsers[circ].circle.channels[chan].messages[mes]);
            }
          }
        }
      }
    }
  }
  removeUser(id){
    this._httpService.removeUser(id, this.circId).subscribe(data => {
      if(data['Message'] === "Error"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
      else{
        this.getUsers();
      }
    })
  }
  
}
