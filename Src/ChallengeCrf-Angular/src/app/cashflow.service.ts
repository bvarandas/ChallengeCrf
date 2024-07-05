import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CashFlow } from './CashFlow';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class CashflowService {
  url = 'http://localhost:9010/api/cashflow';

  constructor(private http: HttpClient) { }

  GetAll(): Observable<CashFlow[]>{
    return this.http.get<CashFlow[]>(this.url);
  }

  GetById(cashFlowId:string):Observable<CashFlow>{
    const apiUrl=`${this.url}/${cashFlowId}`;
    return this.http.get<CashFlow>(apiUrl);
  }

  InsertRegister(cashflow: CashFlow): Observable<any>{
    return this.http.post<CashFlow>(this.url,cashflow, httpOptions);
  }

  UpdateRegister(cashflow: CashFlow) : Observable<any>{
    return this.http.put<CashFlow>(this.url,cashflow, httpOptions);
  }

  RemoveRegister(cashFlowId: string) : Observable<any>{
    const apiUrl=`${this.url}/${cashFlowId}`;
    return this.http.delete<string>(apiUrl, httpOptions);
  }
  
}
