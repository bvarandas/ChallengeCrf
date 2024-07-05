import { Component, TemplateRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Register } from 'src/app/Register';
import { RegistersService } from 'src/app/registers.service';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-registers',
  templateUrl: './registers.component.html',
  styleUrls: ['./registers.component.css']
})

export class RegistersComponent {
  formulario: any;
  tituloFormulario: string;
  registers: Register[];

  visibilidadeTabela: boolean =true;
  visibilidadeFormulario: boolean =false;
  description: string;
  registerId: number;
  modalRef : BsModalRef;

  private _hubConnection: HubConnection;

  constructor(private registersService: RegistersService,
    private modalService: BsModalService){
      this.CreateConnection();
      this.registerOnServerEvents();
      this.startConnection();

    }

    connectToMessageBroker(){
      this._hubConnection.invoke('ConnectToMessageBroker');
    }

    private CreateConnection(){
      this._hubConnection = new HubConnectionBuilder()
                                .withUrl("http://localhost:5200/hubs/brokerhub")
                                .build();
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

    private registerOnServerEvents() : void {
      this._hubConnection.on('ReceiveMessage', 
      (data: Register[])=> { this.registers = data; });
    }

  ngOnInit(): void{

    this.registersService.GetAll().subscribe(resultado=>{
      this.registers = resultado;
    });
  }

  

  ExibirFormularioAtualizacao(registerId: number) : void  {
    this.visibilidadeTabela = false;
    this.visibilidadeFormulario = true;

    this.registersService.GetById(registerId).subscribe((resultado)=>{
      //this.tituloFormulario = `Atualizar ${resultado.description}`;

      this.formulario = new FormGroup({
        registerId: new FormControl(resultado.registerId),
        description: new FormControl(resultado.description),
        status: new FormControl(resultado.status),
        date: new FormControl(resultado.date)
      });
    });
  }

  ExibirFormularioCadastro() : void {
    this.visibilidadeTabela = false;
    this.visibilidadeFormulario = true;
    this.tituloFormulario ="Novo registro";
    this.formulario = new FormGroup({
      description: new FormControl(null),
      status: new FormControl(null),
      date: new FormControl(null)
    });
  }

  EnviarFormulario(): void{
    const register: Register = this.formulario.value;
    
    if (register.registerId > 0){
      this.registersService.UpdateRegister(register).subscribe((resultado)=>{
        this.visibilidadeTabela = true;
        this.visibilidadeFormulario = false;
        alert("Registro atualizado com sucesso");
        this.registersService.GetAll().subscribe((registros)=>{
          this.registers = registros;
        });
      });
    }
    else
    {
      this.registersService.InsertRegister(register).subscribe((resultado)=>{
        this.visibilidadeTabela = true;
        this.visibilidadeFormulario = false;
          alert("Registro inserido com sucesso");
          this.registersService.GetAll().subscribe((registros)=>{
            this.registers = registros;
          });
        }
      );
    }

  }

  Voltar() : void{
    this.visibilidadeTabela = true;
    this.visibilidadeFormulario = false;
  }

  ExibirConfirmacaoExclusao(registerId: number, description: string, 
    conteudoModal: TemplateRef<any>) : void{

    this.modalRef = this.modalService.show(conteudoModal);
    this.registerId = registerId;
    this.description = description;
  }

  RemoverRegistro(registerId: number)  {
    this.registersService.RemoveRegister(registerId).subscribe((resultado)=>{
      this.modalRef.hide();
      alert("Registro excluído com sucesso");
      this.registersService.GetAll().subscribe((registros)=>{
        //this.registers = registros;
      });
    });
  }
}