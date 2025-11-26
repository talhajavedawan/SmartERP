import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class ProfilePictureService {
  private profilePictureSubject = new BehaviorSubject<string | null>(null);
  public profilePicture$ = this.profilePictureSubject.asObservable();

  updateProfilePicture(url: string | null) {
    this.profilePictureSubject.next(url);
  }

  getCurrentProfilePicture(): string | null {
    return this.profilePictureSubject.value;
  }
}
