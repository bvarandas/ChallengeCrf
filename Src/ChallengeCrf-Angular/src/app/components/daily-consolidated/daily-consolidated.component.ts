import { Component, Input } from '@angular/core';
import { DailyConsolidated } from 'src/app/DailyConsolidated';
import { DailyConsolidatedService } from '../../daily-consolidated.service';
import { FormControl, FormGroup } from '@angular/forms';
import { CashFlow } from 'src/app/CashFlow';
import { HubConnection } from '@aspnet/signalr';
@Component({
  selector: 'app-daily-consolidated',
  templateUrl: './daily-consolidated.component.html',
  styleUrls: ['./daily-consolidated.component.css']
})
export class DailyConsolidatedComponent {
  formulario: any;
  tituloFormulario: string;
  @Input() dailyConsolidated: DailyConsolidated;

  constructor(private dailyConsolidatedService: DailyConsolidatedService)
  {
  }
  public registerOnServerEvents( hubConnection:HubConnection) : void {
    hubConnection.on('ReceiveMessageDC', 
    (data: DailyConsolidated)=> { this.dailyConsolidated = data; });
  }
  public GetInitial(): void  {
    this.dailyConsolidatedService.GetByDate(new Date().toISOString()).subscribe(resultado=>{
          //this.dailyConsolidated = resultado;
        });

  }
}
