import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Register } from './Register';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class RegistersService {
  url = 'http://localhost:5200/api/register';

  constructor(private http: HttpClient) { }

  GetAll(): Observable<Register[]>{
    return this.http.get<Register[]>(this.url);
  }

  GetById(registerId:number):Observable<Register>{
    const apiUrl=`${this.url}/${registerId}`;
    return this.http.get<Register>(apiUrl);
  }

  InsertRegister(register: Register): Observable<any>{
    return this.http.post<Register>(this.url,register, httpOptions);
  }

  UpdateRegister(register: Register) : Observable<any>{
    return this.http.put<Register>(this.url,register, httpOptions);
  }

  RemoveRegister(registerId: number) : Observable<any>{
    const apiUrl=`${this.url}/${registerId}`;
    return this.http.delete<number>(apiUrl, httpOptions);
  }

}
