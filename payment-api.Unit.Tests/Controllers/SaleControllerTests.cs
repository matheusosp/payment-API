using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using payment_api.Controllers;
using payment_api.Unit.Tests.Commands;
using PaymentAPI.Application.Commands;
using PaymentAPI.Application.Queries;
using PaymentAPI.Application.Validators;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace payment_api.Unit.Tests.Controllers
{
    public class SaleControllerTests
    {
        private Mock<IMediator> _moqMediator;

        [SetUp]
        public void SetUp()
        {
            _moqMediator = new Mock<IMediator>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _moqMediator.VerifyAll();
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_StatusCode_201_Ao_Registrar_Venda()
        {
            // Arrange
            var command = new RegisterSaleCommand
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="ItemTeste",
                        Price = 5,
                        Quantity = 5
                    },
                },
                Seller = new SellerRequest
                {
                    CPF = 123456,
                    Name = "SellerTeste",
                    Email = "teste@email.com",
                    Phone = 12345
                }
            };
            var validator = new RegisterSaleCommandValidator();

            var validation = validator.Validate(command);

            _moqMediator
                .Setup(p => p.Send(command, default))
                .Returns(Task.FromResult(Result.SuccessIf(validation.IsValid, new Sale() { }, "")));

            // Act
            var result = await GetController().RegisterSale(command, default);

            // Assert
            var createdResult = result as ObjectResult;

            createdResult.StatusCode.Should().Be(201);
        }
        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_StatusCode_200_Ao_Atualizar_Venda()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Status = SaleStatus.ApprovedPayment,
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="ItemTeste",
                        Price = 5,
                        Quantity = 5
                    },
                },
                Seller = new SellerRequest
                {
                    CPF = 123456,
                    Name = "SellerTeste",
                    Email = "teste@email.com",
                    Phone = 12345
                }
            };
            var validator = new UpdateSaleCommandValidator(SaleStatus.AwaitingPayment);

            var validation = validator.Validate(command);

            _moqMediator
                .Setup(p => p.Send(command, default))
                .Returns(Task.FromResult(Result.SuccessIf(validation.IsValid, new Sale() { }, "")));

            // Act
            var result = await GetController().UpdateById(1, command, default);

            // Assert
            var Okesult = result as ObjectResult;

            Okesult.StatusCode.Should().Be(200);
        }
        [Test]
        public async Task Deve_Verificar_Metodo_Com_Retorno_BadRequest_Quando_Resultado_For_Falha()
        {
            // Arrange
            var command = new RegisterSaleCommand
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="ItemTeste",
                        Price = 5,
                        Quantity = 5
                    },
                }
            };
            var validator = new RegisterSaleCommandValidator();

            var validation = validator.Validate(command);

            _moqMediator
                .Setup(p => p.Send(command, default))
                .Returns(Task.FromResult(Result.SuccessIf(validation.IsValid, new Sale() { }, validation.Errors.Select(e => e.ErrorMessage).FirstOrDefault())));

            // Act
            var result = await GetController().RegisterSale(command, default);

            // Assert
            var badRequest = result as BadRequestObjectResult;

            badRequest.StatusCode.Should().Be(400);
        }
        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_StatusCode_400_Ao_Atualizar_Venda()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Status = SaleStatus.Delivered,
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="ItemTeste",
                        Price = 5,
                        Quantity = 5
                    },
                },
                Seller = new SellerRequest
                {
                    CPF = 123456,
                    Name = "SellerTeste",
                    Email = "teste@email.com",
                    Phone = 12345
                }
            };
            var validator = new UpdateSaleCommandValidator(SaleStatus.AwaitingPayment);

            var validation = validator.Validate(command);

            _moqMediator
                .Setup(p => p.Send(command, default))
                .Returns(Task.FromResult(Result.SuccessIf(validation.IsValid, new Sale() { }, validation.Errors.Select(e => e.ErrorMessage).FirstOrDefault())));

            // Act
            var result = await GetController().UpdateById(1,command, default);

            // Assert
            var badRequest = result as BadRequestObjectResult;

            badRequest.StatusCode.Should().Be(400);
        }
        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_StatusCode_200_Ao_Buscar_Venda()
        {
            // Arrange
            _moqMediator
                .Setup(p => p.Send(It.IsAny<RetrieveSaleByIdQuery>(), default))
                .Returns(Task.FromResult(Result.Success(new Sale() { })));

            // Act
            var result = await GetController().RetrieveSaleById(1, default);

            // Assert
            var badRequest = result as ObjectResult;

            badRequest.StatusCode.Should().Be(200);
        }
        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_StatusCode_404_Ao_Buscar_Venda()
        {
            // Arrange
            _moqMediator
                .Setup(p => p.Send(It.IsAny<RetrieveSaleByIdQuery>(), default))
                .Returns(Task.FromResult(Result.Failure<Sale>("Id Não encontrado")));

            // Act
            var result = await GetController().RetrieveSaleById(1, default);

            // Assert
            var badRequest = result as NotFoundResult;

            badRequest.StatusCode.Should().Be(404);
        }
        private SaleController GetController()
        {
            return new SaleController(_moqMediator.Object);
        }
    }
}
