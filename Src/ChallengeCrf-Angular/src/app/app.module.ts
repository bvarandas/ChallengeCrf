import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {MatNativeDateModule} from '@angular/material/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { RegistersService } from './registers.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule} from 'ngx-bootstrap/modal';
import { RegistersComponent } from './components/registers/registers.component';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule}  from '@angular/material/select';
import { MatFormFieldModule } from "@angular/material/form-field";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CashflowComponent } from './components/cashflow/cashflow.component';
import { CashflowService } from './cashflow.service';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { DailyConsolidatedComponent } from './components/daily-consolidated/daily-consolidated.component';
import { DailyConsolidatedService } from './daily-consolidated.service';
import { MatTabsModule } from '@angular/material/tabs'


@NgModule({
  
  declarations: [
    AppComponent,
    RegistersComponent,
    CashflowComponent,
    DailyConsolidatedComponent
  
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatSelectModule,
    MatNativeDateModule,
    BrowserAnimationsModule,
    CurrencyMaskModule,
    MatTabsModule
  ],
  providers: [HttpClientModule, RegistersService, CashflowService, DailyConsolidatedService, DailyConsolidatedComponent,CashflowComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
