import { Component } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { DailyConsolidatedComponent } from './components/daily-consolidated/daily-consolidated.component'
import { CashflowComponent } from './components/cashflow/cashflow.component';
import { CashFlow } from './CashFlow';
import { DailyConsolidated } from './DailyConsolidated';
import { delay } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'ChallengeCrf-Angular';
  cashFlows: CashFlow[];
  dailyConsolidated: DailyConsolidated;
  private _hubConnection: HubConnection;
  private _compDC: DailyConsolidatedComponent;
  private _compCF: CashflowComponent;

  constructor(compDC: DailyConsolidatedComponent,  compCF: CashflowComponent){
    
    this._compDC = compDC;
    this._compCF = compCF;
    this.CreateConnection();
    //setTimeout(() => { this.connectToMessageBroker();}, 2000);
    //setTimeout(() => { this.startConnection();}, 2000);
    this.startConnection();
  }

  private CreateConnection(){
    console.log('CreateConnection');
    this._hubConnection = new HubConnectionBuilder()
                              .withUrl("http://localhost:9010/hubs/brokerhub")
                              .build();

    this._hubConnection.on('ReceiveMessageCF', 
      (data: CashFlow[])=> 
      {  
        this.cashFlows = data; 
      });

    this._hubConnection.on('ReceiveMessageDC', 
      (data: DailyConsolidated)=> 
      { 
        this.dailyConsolidated = data; 
      });
  }

  private startConnection() : void {
    this._hubConnection
    .start()
    .then(()=> {
      console.log('Hub connection started');
      this.connectToMessageBroker();
    })
    .catch(()=> {
      setTimeout(() => { this.startConnection();}, 5000);
    });
  }
  connectToMessageBroker()
  {
    this._hubConnection.invoke('ConnectToMessageBroker');
    // this._compDC.GetInitial();
    // this._compCF.GetInitial();
  }
}