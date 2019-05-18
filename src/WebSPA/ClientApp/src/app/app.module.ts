import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app.routing';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FeedComponent } from './applications/feed/feed.component';
import { ChatComponent } from './applications/chat/chat.component';
import { LoginComponent } from './applications/login/login.component';
import { RegisterComponent } from './applications/register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FeedCardComponent } from './applications/feed/feed-card/feed-card.component';
import { FeedBoxComponent } from './applications/feed/feed-box/feed-box.component';
import { ProfileBoxComponent } from './applications/feed/profile-box/profile-box.component';
import { PersonCardComponent } from './applications/people/person-card/person-card.component';
import { PeopleComponent } from './applications/people/people.component';

@NgModule({
  declarations: [
    AppComponent,
    FeedComponent,
    ChatComponent,
    LoginComponent,
    RegisterComponent,
    FeedCardComponent,
    FeedBoxComponent,
    PeopleComponent,
    PersonCardComponent,
    ProfileBoxComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
