import { Component } from '@angular/core';
@Component({
  selector: 'app-splash-screen',
  standalone: true,
  templateUrl: './splash-screen.component.html',
})
export class SplashScreenComponent {
  showPopup = false;

  openPopup() {
  console.log('openPopup triggered');
  this.showPopup = true;
}

closePopup() {
  console.log('closePopup triggered');
  this.showPopup = false;
}
}
