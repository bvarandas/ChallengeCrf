import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DailyConsolidated } from './DailyConsolidated';
import { Observable } from 'rxjs';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-type': 'application/json'
  })
};
@Injectable({
  providedIn: 'root'
})
export class DailyConsolidatedService {
  url = 'http://localhost:5200/api/dailyconsolidated';
  constructor(private http: HttpClient) { }
  
  GetById(dailyConsolidatedId:string):Observable<DailyConsolidated>{
    const apiUrl=`${this.url}/${dailyConsolidatedId}`;
    return this.http.get<DailyConsolidated>(apiUrl);
  }
  GetByDate(date:string):Observable<DailyConsolidated>{
    const apiUrl=`${this.url}?date=${date}`;
    return this.http.get<DailyConsolidated>(apiUrl);
  }
}
