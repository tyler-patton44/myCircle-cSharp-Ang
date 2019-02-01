import { loginComponent } from './home/login/login.component';
import { registerComponent } from './home/register/register.component';
import { mainComponent } from './homepage/main/main.component';
import {CreateCircleComponent} from './circle/create-circle/create-circle.component';
import { CreateChannelComponent } from './circle/create-channel/create-channel.component';
import { circleUsersComponent } from './circle/circleUsers/circleUsers.component';
import { inviteUserComponent } from './circle/inviteUser/inviteUser.component';




import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
  { path: '',component: loginComponent },
  { path: 'register',component: registerComponent },
  { path: 'create/channel/:id',component: CreateChannelComponent }, 
  { path: 'create/circle',component: CreateCircleComponent },
  { path: 'users/circle/:id',component: circleUsersComponent },
  { path: 'users/invite/:id',component: inviteUserComponent }, 
  { path: 'homepage',component: mainComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }