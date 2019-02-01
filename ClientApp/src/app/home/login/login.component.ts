import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../http.service';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import {ActivatedRoute, Router} from '@angular/router';
@Component({
  selector: 'app-task',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class loginComponent implements OnInit {
  logUser:any;
  error_user:any;
  error_pass:any;
  constructor(private _httpService: HttpService, private route: ActivatedRoute, private routing: Router) { }

  ngOnInit() {
    this.logUser = {email: "", password: ""};
  }
  onSubmit2(){
    this.error_user = null;
    this.error_pass = null;
    let observable = this._httpService.LogginUser(this.logUser);
    observable.subscribe(data => {
      if(data['Message'] != "Success"){
        if(data['email']){
          if(data['Message'] === "Error"){
            this.error_user = [{"errorMessage": data['email']}];
          }
          else{
          this.error_user = data['email']['errors'];
          }
        }
        if(data['password']){
          if(data['Message'] === "Error"){
            this.error_pass = [{"errorMessage": data['password']}];
          }
          else{
            this.error_pass = data['password']['errors'];
          }
        }
      }else{
      this.logUser = {email: "", password: ""};
      this.routing.navigate(['/homepage']);
    }
    })
  }
}
