## Rodando a Aplicação localmente
Para rodar a aplicacao localmente, o dotnet 6.0 precisar estar instalado, e seguir com os seguintes comandos:
1. `cd payment-api`
2. `dotnet run`

No seu navegador, entre no seguinte Link:
```
https://localhost:7256/api-docs/index.html
```

![](swagger.png)
![](Tests.png)

### Arquitetura e Organizacao

Para o desenvolvimento do sistema foi utilizada a arquitetura de camadas baseada no modelo Domain Driven Design da figura abaixo. 

![Arquitetura de Camadas](https://user-images.githubusercontent.com/42355371/74002848-3ba2f200-494f-11ea-9488-c3a22e4f53bd.jpg)

Cada projeto representa uma camada. As responsabilidades de cada camada sao:

	- Apresentacao: interacao com o usuario (A apresentacao nao tem nesse projeto);
	- Servicos Distribuidos: disponibiliza endpoints para serem utilizados pela camada de apresentacao (Projeto PaymentAPI);
	- Aplicacao: gerencia os recursos da solucao (Projeto PaymentAPI.Application);
	- Dominio: contem os objetos e as regras de negocio (Projeto PaymentAPI.Domain);
	- Infraestrutura: servicos externos e camada de acesso a dados (Projeto PaymentAPI.Infra.EF).

### Deploy	
```
https://paymentapi20221018172618.azurewebsites.net/api-docs/index.html
```