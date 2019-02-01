import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(private _http: HttpClient) {
  }
  CreateUser(newUser){
    return this._http.post('/registration', newUser);   
  }
  LogginUser(user){
    return this._http.post('/loggingIn', user);   
  }
  LogoutUser(){
    return this._http.get('/logout');   
  }
  checkLogin(){
    return this._http.get('/checkLogin');   
  }
  getUserCircles(){
    return this._http.get('/userCirclesData');   
  }
  createCircle(circle){
    return this._http.post('/createCircle', circle);   
  }
  createChannel(channel, id){
    return this._http.post('/createChannel/'+id, channel);   
  }
  createMessage(message, id){
    return this._http.post('/leaveMessage/'+id, message);   
  }
  likeMessage(id){
    return this._http.get('/like/'+id);   
  }
  getUsersInCirc(id){
    return this._http.get('/getUsersInCircle/'+id);   
  }
  manageUsers(id){
    return this._http.get('/manageUsers/'+id);   
  }
  inviteUser(id, email){
    return this._http.get('/inviteCircle/'+id+"/"+email);   
  }
  joinCirc(id){
    return this._http.get('/joinCircle/'+id);   
  }
  removeUser(id, circleId){
    return this._http.get('/removeUser/'+id+"/"+circleId);   
  }
  removeChannel(id, circleId){
    return this._http.get('/removeChannel/'+id+"/"+circleId);   
  }
}