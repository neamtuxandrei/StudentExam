import { NgModule } from '@angular/core';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { FeaturesModule } from './features/features.module';
import { SharedModule } from './shared/shared.module';
import { AuthInterceptor, AuthModule, LogLevel } from 'angular-auth-oidc-client';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MessageService,ConfirmationService } from "primeng/api";
import { ToastModule } from 'primeng/toast';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
   
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ToastModule,
    FormsModule,
    CoreModule,
    FeaturesModule,
    SharedModule,
    AuthModule.forRoot({
      config: {
          authority: environment.baseUrl,
          redirectUrl: window.location.origin,
          postLogoutRedirectUri: window.location.origin,
          clientId: 'examsupporttoolui',
          scope: 'openid profile email roles offline_access',
          responseType: 'code',
          silentRenew: true,
          renewTimeBeforeTokenExpiresInSeconds: 10,
          ignoreNonceAfterRefresh: true, // this is required if the id_token is not returned
          useRefreshToken: true,
          logLevel: LogLevel.Debug,
          secureRoutes: [environment.baseUrl]
      },
    }),
  ],
  bootstrap: [AppComponent],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    MessageService,
    ConfirmationService,
    DatePipe
  ],
})
export class AppModule { }
