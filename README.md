# ChallengeCrf
Projeto de desafio porposto pelo time desenvolvimento do Banco Carrefour.
Com SignalR, segue o  video.

Projeto se propoe a fazer um crud com patterns de mercado e mecanismos usados em soluções no mercado de capitais, 
como mesageria e bibliotecas de compressão de dados para camada de transporte como protobuf.

Arquitetura - Message/Event Driven e DDD
Command-> Event
Query-> Reply
Usando  Filas do RabbiMQ para coreografia do ambiente.
Protobuf para compactação no transporte.
SignalR no response do para o client/Angular.

DDD - modelagem de software que segue um conjunto de práticas com 
objetivo de facilitar a implementação de complexas regras e processos de negócios que tratamos como domínio.

Padrões Criacionais usados:
Factory
Singleton

Padrões Comportmentais
Command
Mediator 

Mais
Ioc - Inversão de controle

CQRS - com Coreografia

Injeção de depedencia

Unit of Work

Event Sourcing

Alguns conceitos de solid tbm foram usados.

Instruções para rodar
Rodar o docker do rabbitMQ

docker run -d --hostname rabbitserver --name rabbitmq-server -p 15672:15672 -p 5672:5672 rabbitmq:3-management

rodar o script no mssql
CreateDdbChallengeB3.sql

Modelo da arquitetura.
![Arq_ChallengeB3](https://github.com/bvarandas/ChallengeB3/assets/13907905/3f5e852c-e0f7-4f5d-a46d-289813343551)

Dados Cadastrados
![image](https://github.com/bvarandas/ChallengeB3/assets/13907905/a59d4ac7-1746-4cf3-9f0b-198cc4578028)

Event Source

![image](https://github.com/bvarandas/ChallengeB3/assets/13907905/59c39874-8d82-46e5-a75c-a8a718203fcc)




