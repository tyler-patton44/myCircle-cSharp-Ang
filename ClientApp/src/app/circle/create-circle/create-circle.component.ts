import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../http.service';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import {ActivatedRoute, Router} from '@angular/router';
@Component({
  selector: 'app-create-circle',
  templateUrl: './create-circle.component.html',
  styleUrls: ['./create-circle.component.css']
})
export class CreateCircleComponent implements OnInit {

  constructor(private _httpService: HttpService, private route: ActivatedRoute, private routing: Router) { }
  newCirc:any;
  error_title:any;
  ngOnInit() {
    this._httpService.checkLogin().subscribe(data => {
      if(data['Message'] != "Success"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
    })
    this.newCirc = {title: ""};
  }
  onSubmit2(){
    this.error_title = null;
    let observable = this._httpService.createCircle(this.newCirc);
    observable.subscribe(data => {
      if(data['Message'] != "Success"){
        if(data['title']){
          if(data['Message'] === "Error"){
            this.error_title = [{"errorMessage": data['title']}];
          }
          else{
          this.error_title = data['title']['errors'];
          }
        }
        if(data['hacking']){
          this._httpService.LogoutUser().subscribe(data => {
            this.routing.navigate(['/']);
          })
        }
      }
      else{
        this.newCirc = {title: ""};
        this.routing.navigate(['/homepage']);
      }
    })
  }
  
}
