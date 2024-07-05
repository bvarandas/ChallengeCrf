import { RegistersComponent } from './components/registers/registers.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CashflowComponent } from './components/cashflow/cashflow.component';
import { DailyConsolidatedComponent } from './components/daily-consolidated/daily-consolidated.component'

const routes: Routes = [{
  path:'registers', component: RegistersComponent
},{
  path:'cashflow', component: CashflowComponent
},{
  path:'daily-consolidated', component: DailyConsolidatedComponent
}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
