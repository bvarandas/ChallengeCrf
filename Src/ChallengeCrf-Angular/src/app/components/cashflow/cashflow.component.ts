import { CashFlow } from '../../CashFlow';
import { Component, Input, TemplateRef } from '@angular/core';
import { CashflowService } from '../../cashflow.service';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { FormControl, FormGroup } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-cashflow',
  templateUrl: './cashflow.component.html',
  styleUrls: ['./cashflow.component.css'],

})
export class CashflowComponent {
  formulario: any;
  tituloFormulario: string;
  @Input() cashflows: CashFlow[];
  titulo:string;

  visibilidadeTabela: boolean =true;
  visibilidadeFormulario: boolean =false;
  description: string;
  cashFlowId: string;
  modalRef : BsModalRef;

    constructor(private cashflowService: CashflowService,
    private modalService: BsModalService){
    }
    

    public registerOnServerEvents(hubConnection:HubConnection, listCashFlow:CashFlow[]) : void {
      hubConnection.on('ReceiveMessageCF', 
      (data: CashFlow[])=> 
      {  
        //this.cashflows = data; 
        listCashFlow = data;
      });
    }

    public GetInitial() : void{
      this.cashflowService.GetAll().subscribe(resultado=>{
      //this.cashflows = resultado;
    });
    }

  ngOnInit(): void{
    // this.cashflowService.GetAll().subscribe(resultado=>{
    //   this.cashflows = resultado;
    // });
  }
 

  // ExibirFormularioAtualizacao(cashFlowId: string) : void  {
  //   this.visibilidadeTabela = false;
  //   this.visibilidadeFormulario = true;

  //   this.cashflowService.GetById(cashFlowId).subscribe((resultado)=>{
  //     //this.tituloFormulario = `Atualizar ${resultado.description}`;

  //     this.formulario = new FormGroup({
  //       //cashFlowId: new FormControl(resultado.cashFlowId),
  //       description: new FormControl(resultado.description),
  //       entry: new FormControl(resultado.entry),
  //       amount: new FormControl(resultado.amount),
  //       date: new FormControl(resultado.date)
  //     });
  //   });
  // }
  ExibirFormularioAtualizacao(cashFlow: CashFlow) : void  {
    this.visibilidadeTabela = false;
    this.visibilidadeFormulario = true;
    //this.tituloFormulario = `Atualizar ${resultado.description}`;

      this.formulario = new FormGroup({
        cashFlowId: new FormControl(cashFlow.cashFlowId),
        description: new FormControl(cashFlow.description),
        entry: new FormControl(cashFlow.entry),
        amount: new FormControl(cashFlow.amount),
        date: new FormControl(cashFlow.date)
      });
  }
  ExibirFormularioCadastro() : void {
    this.visibilidadeTabela = false;
    this.visibilidadeFormulario = true;
    this.tituloFormulario ="Novo Lançamento";
    this.formulario = new FormGroup({
      description: new FormControl(null),
      amount: new FormControl(null),
      entry: new FormControl(null),
      date: new FormControl(null)
    });
  }

  EnviarFormulario(): void{
    const register: CashFlow = this.formulario.value;
    register.date = new Date();

    if (register.cashFlowId !== undefined){
      register.cashFlowIdTemp = register.cashFlowId;
      this.cashflowService.UpdateRegister(register).subscribe((resultado)=>{
        this.visibilidadeTabela = true;
        this.visibilidadeFormulario = false;
        alert("Lançamento enviado com sucesso");
        this.cashflowService.GetAll().subscribe((registros)=>{
          //this.cashflows = registros;
        });
      });
    }
    else
    {
      this.cashflowService.InsertRegister(register).subscribe((resultado)=>{
        this.visibilidadeTabela = true;
        this.visibilidadeFormulario = false;
          alert("Lançamento enviado com sucesso");
          this.cashflowService.GetAll().subscribe((registros)=>{
            //this.cashflows = registros;
          });
        }
      );
    }

  }

  Voltar() : void{
    this.visibilidadeTabela = true;
    this.visibilidadeFormulario = false;
  }

  ExibirConfirmacaoExclusao(cashflowId: string, description: string, 
    conteudoModal: TemplateRef<any>) : void{

    this.modalRef = this.modalService.show(conteudoModal);
    this.cashFlowId = cashflowId;
    this.description = description;
  }

  RemoverRegistro(registerId: string)  {
    this.cashflowService.RemoveRegister(registerId).subscribe((resultado)=>{
      this.modalRef.hide();
      alert("Solicitação de exclusão de Lançamento enviado com sucesso");
      this.cashflowService.GetAll().subscribe((cacheflows)=>{
        this.cashflows = cacheflows;
      });
    });
  }
}
