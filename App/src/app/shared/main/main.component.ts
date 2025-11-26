import { Component, ElementRef, ViewChild, AfterViewInit } from "@angular/core";
import { Router, RouterLink } from "@angular/router";

@Component({
  selector: "app-main",
  standalone: true,
  imports: [RouterLink],
  templateUrl: "./main.component.html",
  styleUrls: ["./main.component.css"],
})
export class MainComponent implements AfterViewInit {
  @ViewChild("lazyVideo") lazyVideo!: ElementRef<HTMLVideoElement>;

  constructor(private router: Router) {}

  onRegister(): void {
    this.router.navigate(["/register"]);
  }

  onLogin(): void {
    this.router.navigate(["/login"]);
  }

  /** ✅ Lazy load video when visible */
  ngAfterViewInit(): void {
    const video = this.lazyVideo.nativeElement;

    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            video.load(); // start loading when visible
            observer.unobserve(video);
          }
        });
      },
      { threshold: 0.1 }
    );

    observer.observe(video);
  }
}
