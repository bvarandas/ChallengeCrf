# ChallengeCrf
Projeto proposto para desafio.
---

Projeto se propoe a fazer um CRUD com patterns de mercado e mecanismos usados em soluções no mercado de capitais.

Requisito arquitetural relevantes: 
* O serviço de controle de lançamento não deve ficar indisponível se o sistema de consolidado diário cair.
* O serviço recebe 500 requisições por segundos, com no máximo 5% de perda de requisições

---

A escolha por essa arquitetura de mensageria foi fetia depois de analisarmos os requisitos arquiteturais propostos no desafio.
É o tipo de arquitetura que usamos no mercado de capitais para trade e análise de risco pré-trade e pós-trade.

Arquitetura - Message/Event Driven e alguns elementos de Clean Architecture.
* Command-> Event
* Query-> Reply

Usando  **Filas do RabbiMQ** para coreografia do ambiente - Importante na quantidade de mensagens (recebimento e entrega), e também inportante na alta disponibilidade e resiliência da entrega, pois foi implementado, Ack e NAck no consumer.

**Protobuf** para compactação na camada de transporte entre serviços (Array de Bytes)- Importante na compactação das mensagens para transporte e para melhor armazenamento e envio pelo RabbitMQ.

**SignalR** no response do para o client/Angular.(Tela) - Importante para recebimento assincrono das informações de Consolidado Diário e  Lançamento de Fluxo de caixa na tela.

**Entity Framework** - Facilidade e rapidez na implementação. Hoje a perfomance  muito está muito satisfatória e a capacidade de gravação em lote. 

**MongoDB** - NoSQL com Facilidade e rapidez na implementação. Ótima Perfomance e a capacidade de gravação em lote. O MongoDB também conta com conexões transacionadas. 

**Essa abordagem também restringe que cada serviço tenha sua responsabilidade separadamente, garantindo a coesão da programação e também mantendo suas lógicas desacopladas.**
---


Modelo da arquitetura C4


![arq_crf_gif](https://github.com/bvarandas/ChallengeCrf/assets/13907905/a96c3abb-1dcb-4396-be77-52005d921012)

---

Padrões Criacionais usados:
* Singleton - para usar somente uma instancia de cada módulo

Padrões Comportmentais
* Command
* Mediator 

Mais Parterns
* **Domain Notification** - para notificações centralizdas, validando os lançamentos na application
* **CQRS - com Coreografia** - para leitura ficar separadamente da gravação, melhorando assim a performance da applicação
* **Injeção de depedencia** - Injetando as dependências, para usarmos as interfaces dos objetos, desacoplando as chamadas dos métodos entre os objetos.
* **Unit of Work** - O commit é feito nas handles para mantermos os objetos(repo/event) desacoplados, mandando eventos depois de salvar/deletar o command
* **Event Sourcing** (removido) - Event Sourcing foi removido por questões de simplicidade do sistema, não achei necessário nesse tipo de sistema simples.

Alguns conceitos de solid tbm foram usados, como:
* **Single Responsibility Principle** - deve ter uma unica resposabilidade (falta de coesão, alto aclopamento, 
dificuldades na implementação de testes automatizados, Dificuldade para reproveitar o código)
* **Open/Close Principle** - As entidade devem ser abertas para ampliação, mas fechadas para  modificação. 
(Usar abstract class ex. CashFlowCommand, concrete class InsertCashFlowCommand e UpdateCashFlowCommand)
* **Interface Segregation Principle**- Muitas interfaces específicas são melhores do que uma interface geral 
(Violação IRepositorio para repositório de CashFlow e DailyConsolidated. o Ideal é ICashFlowRepository, 
e um IDailyConsolidatedRepository para cada classe)
* **Dependency Inversion Principle** - Depender de abstrações e não de classes concretas.

---

Telas do app
![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/28a7749b-b92a-4001-b800-83299251ef13)

![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/6aea9bf4-5ee8-46ee-917b-62b12db53801)


Instruções para rodar

Setar o visual Studio para rodar o docker compose
![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/f291af70-68bf-4391-9af9-b83890a04676)

![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/88e23b2b-814c-4e3e-a182-b8075c4e2234)

F5

Irá subir os seguintes containers

![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/d4553f77-03b2-4128-b6c1-31221a0b52ef)



Na visual studio Code, entrar na pasta 

![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/2925361b-7de8-4b1b-b632-806a4d31d1e8)

![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/9b7e4af0-b126-4ffa-a91c-231fcb563313)

Abrir um terminal e digitar ng serve

Acessar no navegador http://localhost:4200/


Irá subir os containers de:
rabbitmq-server - RabbitMQ
mongo - MongoDB
challengecrf.api - Api de requisições para Controle de lançamentos e consolidado diário
challengecrf.queue - Worker para Producer e consumer para o serviço 
angularcontainer - Front End em angular para efetuar o cadastro. http//localhost:4200/cashflow



