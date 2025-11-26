import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, of, throwError } from "rxjs";
import { environment } from "../../../environments/environment";
import { tap, catchError } from "rxjs/operators";

export interface UserSettingResponse {
  message: string;
}

@Injectable({
  providedIn: "root",
})
export class UserSettingService {
  private apiUrl = `${environment.apiBaseUrl}/UserSetting`;

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem("accessToken");
    return new HttpHeaders({
      "Content-Type": "application/json",
      Authorization: token ? `Bearer ${token}` : "",
    });
  }

  getSetting(key: string): Observable<string> {
    return this.http
      .get(`${this.apiUrl}/${key}`, {
        headers: this.getAuthHeaders(),
        responseType: "text",
      })
      .pipe(
        catchError((err) => {
          if (err.status === 404 || err.status === 204) {
            return of("");
          }
          return throwError(() => err);
        })
      );
  }

  saveSetting(
    settingKey: string,
    settingValue: string
  ): Observable<UserSettingResponse> {
    const payload = { settingKey, settingValue };

    return this.http
      .post<UserSettingResponse>(this.apiUrl, payload, {
        headers: this.getAuthHeaders(),
      })
      .pipe(
        catchError((err) => {
          return throwError(() => err);
        })
      );
  }
}
