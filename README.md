# ChallengeCrf
Projeto proposto para desafio.
---

A Proposta inicial é fazer lançamentos de fluxo de caixa, crédito e débito, mais ou menos um

CRUD com patterns de mercado e mecanismos usados em soluções de mensageria.

Requisitos arquiteturais relevantes: 
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

**Ocelot** - Importante para Proxy reverso em ambiente DMZ. Ele também pode gerenciar e validar conexões Sginalr e websocket, validando se o cliente é válido ou não, antes de chegar no ambiente de backend.

**Docker** - Para colocar parte a aplicação, ou aplicação inteira em containers. Porém, o ideal aqui é somente escalabilizar parte da aplicação. 

**Kubernetes** - para orquestração dos containers. Porém, o ideal aqui é somente escalabilizar parte da aplicação. 

**SignalR** no response do para o client/Angular.(Tela) - Importante para recebimento assincrono das informações de Consolidado Diário e  Lançamento de Fluxo de caixa na tela.

**Entity Framework** - Facilidade e rapidez na implementação. Hoje a perfomance  muito está muito satisfatória e a capacidade de gravação em lote. 

**MongoDB** - NoSQL com Facilidade e rapidez na implementação. Ótima Perfomance e a capacidade de gravação em lote. O MongoDB também conta com conexões transacionadas. 

**ElasticSearch** - Para centralização dos logs. Não ter acesso a logs, não só prejudica o time de tecnologia no SLA, mas também tira a dinamicidade da empresa em dar uma resposta ao cliente, caso alguma coisa não saia como esperado.

**Kibana** - Importante para visualização dos logs de modo padronizado e com ampla gama de funcionalidades de dashboards.

**XUnit e Moq** - Os testes funcionais foram efetuados  nas Hnndlers e nas Services.

**JMeter** - para testes não funcionais é recomendado o JMeter, como teste de conexões no signalr e testes de conexões nas APIS.

**Essa abordagem também restringe que cada serviço tenha sua responsabilidade separadamente, garantindo a coesão da programação (Código) e também mantendo suas lógicas de negócio   desacopladas. Melhorando não só a perfomance, mas também o aperfeiçoando a mantenabilidade. Também, é possível notar no diagrama que é possível viabilizar e escala somente uma parte do sistema**
---

Modelo da arquitetura C4

![arq_crf_gif](https://github.com/bvarandas/ChallengeCrf/assets/13907905/628331bc-8116-4088-a7e7-18749501f89f)

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

---

Instruções para rodar

Setar o visual Studio para rodar o docker compose
![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/375a4cbe-f130-4fb7-9706-8bb98bbdc38a)

F5

Irá subir os seguintes containers

![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/bb70b6b9-dcac-493e-a3da-30dc011b55ef)

---


Na visual studio Code, entrar na pasta 
![image](https://github.com/bvarandas/ChallengeCrf/assets/13907905/e8e127e0-afa3-4548-8fd2-332abe9f27bb)


Abrir um terminal e digitar ng serve

Acessar no navegador http://localhost:4200/

---

Testes



Obrigado pela Visita, seja bem vindo a opinar em qualquer melhoria.!!!



