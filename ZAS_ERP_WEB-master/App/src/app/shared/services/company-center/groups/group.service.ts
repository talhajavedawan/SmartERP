import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Group } from '../../../../Modules/company-center/groups/group.model';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private baseUrl = environment.apiBaseUrl;
  private apiUrl = `${this.baseUrl}/Group`;

   constructor(private http: HttpClient){}
getAllGroups(status: string = 'all'): Observable<Group[]> {
  return this.http.get<Group[]>(`${this.apiUrl}/GetAllGroups`, {
    params: { status }
  });
}


   getGroupById(id: number): Observable<Group>{
   return this.http.get<Group> (`${this.apiUrl}/GetGroupById/${id}`);
    
}
createGroup(group: Group): Observable<Group>{
  return this.http.post<Group>(`${this.apiUrl}/CreateGroup`, group);
}
updateGroup(id:number, group: Group): Observable<Group>{
  return this.http.put<Group>(`${this.apiUrl}/UpdateGroup/${id}`, group); 
}
}