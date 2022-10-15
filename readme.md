## Running the Application Locally
Para rodar a aplicação localmente, o dotnet 6.0 precisar estar instalado, e seguir com os seguintes comandos:
1. `cd payment-api`
2. `dotnet run`

No seu navegador, entre no seguinte Link:
```
https://localhost:7256/api-docs/index.html
```

![](swagger.png)
![](Tests.png)

### Arquitetura e Organização

Para o desenvolvimento do sistema foi utilizada a arquitetura de camadas baseada no modelo Domain Driven Design da figura abaixo. 

![Arquitetura de Camadas](https://user-images.githubusercontent.com/42355371/74002848-3ba2f200-494f-11ea-9488-c3a22e4f53bd.jpg)

Cada projeto representa uma camada. As responsabilidades de cada camada são:

	- Apresentação: interação com o usuário (A apresentação não tem nesse projeto);
	- Serviços Distribuidos: disponibiliza endpoints para serem utilizados pela camada de apresentação (Projeto PaymentAPI);
	- Aplicação: gerencia os recursos da solução (Projeto PaymentAPI.Application);
	- Domínio: contém os objetos e as regras de negócio (Projeto PaymentAPI.Domain);
	- Infraestrutura: serviços externos e camada de acesso à dados (Projeto PaymentAPI.Infra.EF).