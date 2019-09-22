import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { SwUpdate } from '@angular/service-worker';

import { Events, MenuController, Platform, ToastController } from '@ionic/angular';

import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';

import { Storage } from '@ionic/storage';

import { UserDataService } from './providers/user-data';
import { User } from './shared/models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  appPages = [
    {
      title: 'Feed',
      url: '/app/tabs/feed',
      icon: 'home'
    },
    {
      title: 'Procurar',
      url: '/app/tabs/search',
      icon: 'search'
    },
    {
      title: 'Account',
      url: '/app/tabs/account',
      icon: 'person'
    },
    {
      title: 'Novo Post',
      url: '/app/tabs/new-post',
      icon: 'add-circle'
    },
    {
      title: 'Support',
      url: '/support',
      icon: 'help'
    }
  ];
  loggedIn = false;
  _darkmode = false;

  constructor(
    private events: Events,
    private menu: MenuController,
    private platform: Platform,
    private router: Router,
    private splashScreen: SplashScreen,
    private statusBar: StatusBar,
    private storage: Storage,
    private userData: UserDataService,
    private swUpdate: SwUpdate,
    private toastCtrl: ToastController,
  ) {
    this.detectColorScheme();
    this.initializeApp();
  }

  get darkMode(): boolean {
    return this._darkmode;
  }

  set darkMode(res: boolean) {
    this.storage.set('darkMode', res);
    if (res) {
      document.documentElement.setAttribute('data-theme', 'dark');
    } else {
      document.documentElement.setAttribute('data-theme', 'light');
    }
    this._darkmode = res;
  }

  async ngOnInit() {
    this.checkLoginStatus();
    this.listenForLoginEvents();

    this.swUpdate.available.subscribe(async res => {
      const toast = await this.toastCtrl.create({
        message: 'Update available!',
        showCloseButton: true,
        position: 'bottom',
        closeButtonText: `Reload`
      });

      await toast.present();

      toast
        .onDidDismiss()
        .then(() => this.swUpdate.activateUpdate())
        .then(() => window.location.reload());
    });
  }

  initializeApp() {
    this.platform.ready().then(() => {
      this.statusBar.styleDefault();
      this.splashScreen.hide();
    });
  }

  detectColorScheme() {
    let theme = 'light';    // default to light

    // local storage is used to override OS theme settings
    this.storage.get('darkMode').then(res => {
      if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        // OS theme setting detected as dark
        theme = 'dark';
      }

      res ? theme = 'dark' : theme = 'light';

      // dark theme preferred, set document with a `data-theme` attribute
      if (theme === 'dark') {
        document.documentElement.setAttribute('data-theme', 'dark');
        this.darkMode = true;
      } else {
        this._darkmode = false;
      }
    });
  }

  async checkLoginStatus() {
    const loggedIn = await this.userData.isLoggedIn();
    return this.updateLoggedInStatus(loggedIn);
  }

  updateLoggedInStatus(loggedIn: boolean) {
    this.loggedIn = loggedIn;
  }

  listenForLoginEvents() {
    this.events.subscribe('login:loginInit', () => {
      if (this.loggedIn) {
        this.router.navigateByUrl('/app/tabs/feed');
      }
    });

    this.events.subscribe('user:login', (user: User) => {
      this.updateLoggedInStatus(true);
      this.router.navigateByUrl('/app/tabs/feed');
    });

    this.events.subscribe('user:signup', user => {
      this.updateLoggedInStatus(true);
      this.router.navigateByUrl('/login');
    });

    this.events.subscribe('user:logout', () => {
      this.updateLoggedInStatus(false);
      this.router.navigateByUrl('/login');
    });
  }

  logout() {
    this.menu.enable(false);
    this.userData.logout().then(() => {
      return this.router.navigateByUrl('/login');
    });
  }

  openTutorial() {
    this.menu.enable(false);
    this.storage.set('ion_did_tutorial', false);
    this.router.navigateByUrl('/tutorial');
  }
}
