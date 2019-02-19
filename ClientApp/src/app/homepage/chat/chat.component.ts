import { Component, OnInit, Input, Output, ElementRef, ViewChild, AfterViewChecked, EventEmitter } from '@angular/core';
import { HttpService } from '../../http.service';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import {ActivatedRoute, Router} from '@angular/router';
@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class chatComponent implements OnInit {
  @Input() curCircle: any;
  @Input() channelToShow: any;
  @Input() curUser: any;
  @Output() onSubmission = new EventEmitter<any>();
  constructor(private _httpService: HttpService, private route: ActivatedRoute, private routing: Router) { }
  error_message:any;
  error_image:any;
  newMessage:any;
  circleUsers:any;
  imageReady:any;
  order: string = 'createdAt';
  ngOnInit() {
    this.newMessage = {content: "", image: ""};
    this.getUsers();
  }
  onSubmit(){
    this.error_message = null;
    let observable = this._httpService.createMessage(this.newMessage,this.channelToShow.channelId);
    observable.subscribe(data => {
      if(data['Message'] != "Success"){
        if(data['content']){
          if(data['Message'] === "Error"){
            this.error_message = [{"errorMessage": data['content']}];
          }
          else{
          this.error_message = data['content']['errors'];
          }
        }
        if(data['image']){
          this.error_image = data['image']['errors'];
        }
        if(data['hacking']){
          this._httpService.LogoutUser().subscribe(data => {
            this.routing.navigate(['/']);
          })
        }
      }
      else{
        this.newMessage = {content: "", image: ""};
        this.onSubmission.emit("done");
      }
    })
  }

  isLiked(likes){
    var flag = true;
    for(var l in likes){
      if(this.curUser.userId === likes[l].userId){
        flag = false;
      }
    }
    return flag;
  }
  likeMessage(id){
    this._httpService.likeMessage(id).subscribe(data => {
      if(data['Message'] === "Error"){
        if(data['hacking']){
          this._httpService.LogoutUser().subscribe(data => {
            this.routing.navigate(['/']);
          })
        }
      }
      else{
        this.onSubmission.emit("done");
      }
    })
  }
  getUsers(){
    this._httpService.getUsersInCirc(this.curCircle.circleId).subscribe(data => {
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

  changeListener($event) : void {
    if($event.target.files[0].type.includes('image')){
    this.imageReady = "green";
    this.readThis($event.target);
    }
    else{
      alert("Not a valid image");
    }
  }
  
  readThis(inputValue: any): void {
    var file:File = inputValue.files[0];
    var myReader:FileReader = new FileReader();
    myReader.onloadend = (e) => {
     this.newMessage.image = myReader.result;
    }
    myReader.readAsDataURL(file);
  }
}
