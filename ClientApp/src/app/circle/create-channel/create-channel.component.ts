import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../http.service';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import {ActivatedRoute, Router} from '@angular/router';
@Component({
  selector: 'app-create-circle',
  templateUrl: './create-channel.component.html',
  styleUrls: ['./create-channel.component.css']
})
export class CreateChannelComponent implements OnInit {

  constructor(private _httpService: HttpService, private route: ActivatedRoute, private routing: Router) { }
  newChannel:any;
  error_name:any;
  circId:any;
  ngOnInit() {
    this._httpService.checkLogin().subscribe(data => {
      if(data['Message'] != "Success"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
    })
    var observable= this.route.params;
    observable.subscribe((res)=>{
      this.circId = res.id;
    })
    this.newChannel = {name: ""};
  }
  onSubmit2(){
    this.error_name = null;
    let observable = this._httpService.createChannel(this.newChannel,this.circId);
    observable.subscribe(data => {
      if(data['Message'] != "Success"){
        if(data['title']){
          if(data['Message'] === "Error"){
            this.error_name = [{"errorMessage": data['name']}];
          }
          else{
          this.error_name = data['name']['errors'];
          }
        }
        if(data['hacking']){
          this._httpService.LogoutUser().subscribe(data => {
            this.routing.navigate(['/']);
          })
        }
      }
      else{
        this.newChannel = {name: ""};
        this.routing.navigate(['/homepage']);
      }
    })
  }
  
}
