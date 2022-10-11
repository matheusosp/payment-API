using AutoMapper;
using Moq;
using NUnit.Framework;
using PaymentAPI.Application.Commands;
using PaymentAPI.Application.Queries;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features.Enums;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace payment_api.Unit.Tests.Queries
{
    public class RetrieveSaleByIdQueryTests
    {
        private Mock<ISaleRepository> _moqSaleRepository;

        [SetUp]
        public void SetUp()
        {
            _moqSaleRepository = new Mock<ISaleRepository>(MockBehavior.Default);
        }
        [Test]
        public async Task Dado_um_comando_valido_uma_venda_deve_ser_encontrada()
        {
            // Arrange
            var command = new RetrieveSaleByIdQuery(1);

            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns(new Sale() { });

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Test]
        public async Task Dado_um_comando_invalido_uma_venda_nao_deve_ser_encontrada()
        {
            // Arrange
            var command = new RetrieveSaleByIdQuery(1);

            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns<Sale>(null);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }
        private RetrieveSaleByIdQueryHandler GetCommand()
        {
            return new RetrieveSaleByIdQueryHandler(
                _moqSaleRepository.Object
            );
        }
    }
}
