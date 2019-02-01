import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { HttpService } from './http.service';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms'; // <-- import FormsModule.
import { AppRoutingModule } from './app-routing.module';
import { loginComponent } from './home/login/login.component';
import { registerComponent } from './home/register/register.component';
import { mainComponent } from './homepage/main/main.component';
import { chatComponent } from './homepage/chat/chat.component';
import { CreateChannelComponent } from './circle/create-channel/create-channel.component';
import { CreateCircleComponent } from './circle/create-circle/create-circle.component';
import { circleUsersComponent } from './circle/circleUsers/circleUsers.component';
import { inviteUserComponent } from './circle/inviteUser/inviteUser.component';
import { OrderModule } from 'ngx-order-pipe';


@NgModule({
  declarations: [AppComponent, loginComponent, registerComponent, mainComponent,CreateChannelComponent, CreateCircleComponent, chatComponent, circleUsersComponent, inviteUserComponent],
  imports: [BrowserModule, AppRoutingModule,HttpClientModule,
    FormsModule, OrderModule],
  providers: [HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }