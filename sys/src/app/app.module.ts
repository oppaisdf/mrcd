import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './core/app/app.component';
import { LoginComponent } from './routes/login/login.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from './shared/shared.module';
import { NavBarComponent } from './core/components/nav-bar/nav-bar.component';
import { DashComponent } from './routes/base/dash.component';
import { Menu } from './core/components/menu/menu';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavBarComponent,
    DashComponent,
    Menu
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }