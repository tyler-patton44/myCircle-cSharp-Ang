import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../http.service';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import {ActivatedRoute, Router} from '@angular/router';
@Component({
  selector: 'app-circle',
  templateUrl: './inviteUser.component.html',
  styleUrls: ['./inviteUser.component.css']
})
export class inviteUserComponent implements OnInit {

  constructor(private _httpService: HttpService, private route: ActivatedRoute, private routing: Router) { }
  newInvite:any;
  inviteError:any;
  circId:any;
  ngOnInit() {
    this._httpService.checkLogin().subscribe(data => {
      if(data['Message'] != "Success"){
        this._httpService.LogoutUser().subscribe(data => {
          this.routing.navigate(['/']);
        })
      }
    })
    this.newInvite = {email: ""};

    this.route.params.subscribe((res)=>{
      this.circId = res.id;
    })
  }
  onSubmit(){
    this.inviteError = null;
    let observable = this._httpService.inviteUser(this.circId, this.newInvite.email);
    observable.subscribe(data => {
      console.log(data);
      if(data['Message'] != "Success"){
        if(data['email']){
          this.inviteError = [{"errorMessage": data['email']}];
        }
        if(data['hacking']){
          this._httpService.LogoutUser().subscribe(data => {
            this.routing.navigate(['/']);
          })
        }
      }
      else{
        this.newInvite = {email: ""};
        this.routing.navigate(['/users/circle/'+this.circId]);
      }
    })
  }
  
}
