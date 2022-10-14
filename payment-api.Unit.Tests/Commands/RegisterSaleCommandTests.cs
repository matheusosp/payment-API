using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using PaymentAPI.Application.Commands;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace payment_api.Unit.Tests.Commands
{
    [TestFixture]
    public class RegisterSaleCommandTests
    {
        private Mock<ISaleRepository> _moqSaleRepository;
        private Mock<IUnitOfWork> _moqUnitOfWork;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _moqUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Default);
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));
            _moqSaleRepository = new Mock<ISaleRepository>(MockBehavior.Default);
        }

        [Test]
        public async Task Dado_um_comando_valido_a_venda_deve_ser_gerada()
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
                Seller = new SellerRequest { 
                    CPF = 123456,
                    Name =  "SellerTeste",
                    Email = "teste@email.com",
                    Phone = 12345
                }    
            };
            var sale = _mapper.Map<Sale>(command);
            _moqSaleRepository.Setup(s => s.Add(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Test]
        public async Task Dado_um_pedido_sem_itens_o_mesmo_nao_deve_ser_gerado()
        {
            // Arrange
            var command = new RegisterSaleCommand
            {
                Date = DateTime.Now,
                Seller = new SellerRequest
                {
                    CPF = 123456,
                    Name = "SellerTeste",
                    Email = "teste@email.com",
                    Phone = 12345
                }
            };
            var sale = _mapper.Map<Sale>(command);
            _moqSaleRepository.Setup(s => s.Add(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("A venda tem que ter pelo menos 1 item.");
        }
        [Test]
        public async Task Dado_um_pedido_sem_vendedor_o_mesmo_nao_deve_ser_gerado()
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
            var sale = _mapper.Map<Sale>(command);
            _moqSaleRepository.Setup(s => s.Add(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("A venda deve ter um vendedor");
        }
        [Test]
        public async Task Dado_um_pedido_sem_data_o_mesmo_nao_deve_ser_gerado()
        {
            // Arrange
            var command = new RegisterSaleCommand
            {
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
            var sale = _mapper.Map<Sale>(command);
            _moqSaleRepository.Setup(s => s.Add(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("A venda deve ter uma data");
        }

        [Test]
        [TestCaseSource(nameof(ComandoComItensInvalidos))]
        [TestCaseSource(nameof(ComandoComVendedoresInvalidos))]
        public async Task Dado_um_pedido_com_itens_invalidos_o_mesmo_nao_deve_ser_gerado(RegisterSaleCommand command, string expectedResult)
        {
            // Arrange
            var sale = _mapper.Map<Sale>(command);
            _moqSaleRepository.Setup(s => s.Add(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(expectedResult);
        }
        private static IEnumerable<TestCaseData>  ComandoComItensInvalidos()
        {
            yield return new TestCaseData(new RegisterSaleCommand() //Item Com nome pequeno
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="I",
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
            }, "Name do item deve ter entre 2 e 20 caracteres");
            yield return new TestCaseData(new RegisterSaleCommand() //Item Sem Nome
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
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
            }, "Propriedade Name do item deve estar preenchida.");
            yield return new TestCaseData(new RegisterSaleCommand() //Item Com preço negativo
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
                        Price = -1,
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
            }, "Propriedade Price do item deve ser maior ou igual a 0.");

            yield return new TestCaseData(new RegisterSaleCommand() //Item Sem preço
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
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
            }, "Propriedade Price do item deve estar preenchida.");
            yield return new TestCaseData(new RegisterSaleCommand() //Item Com quantidade invalida
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
                        Price = 5,
                        Quantity = 0
                    },
                },
                Seller = new SellerRequest
                {
                    CPF = 123456,
                    Name = "SellerTeste",
                    Email = "teste@email.com",
                    Phone = 12345
                }
            }, "Propriedade Quantity do item deve estar preenchida e ser maior que 0.");
            yield return new TestCaseData(new RegisterSaleCommand() //Item Sem quantidade
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
                        Price = 5
                    },
                },
                Seller = new SellerRequest
                {
                    CPF = 123456,
                    Name = "SellerTeste",
                    Email = "teste@email.com",
                    Phone = 12345
                }
            }, "Propriedade Quantity do item deve estar preenchida e ser maior que 0.");
        }
        private static IEnumerable<TestCaseData> ComandoComVendedoresInvalidos()
        {
            yield return new TestCaseData(new RegisterSaleCommand() //Vendedor Com nome pequeno
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
                        Price = 5,
                        Quantity = 5
                    },
                },
                Seller = new SellerRequest
                {
                    CPF = 123456,
                    Name = "S",
                    Email = "teste@email.com",
                    Phone = 12345
                }
            }, "Name do vendedor deve ter entre 2 e 20 caracteres");
            yield return new TestCaseData(new RegisterSaleCommand() //Vendedor Sem CPF
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
                        Price = 5,
                        Quantity = 5
                    },
                },
                Seller = new SellerRequest
                {
                    Name = "Im",
                    Email = "teste@email.com",
                    Phone = 12345
                }
            }, "CPF do vendedor deve ser preenchido.");
            yield return new TestCaseData(new RegisterSaleCommand() //Vendedor Com Email Invalido
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
                        Price = 5,
                        Quantity = 5
                    },
                },
                Seller = new SellerRequest
                {
                    Name = "Im",
                    CPF = 123456,
                    Email = "teste",
                    Phone = 12345
                }
            }, "Email do vendedor deve conter um email válido");
            yield return new TestCaseData(new RegisterSaleCommand() //Vendedor Sem Email
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
                        Price = 5,
                        Quantity = 5
                    },
                },
                Seller = new SellerRequest
                {
                    Name = "Im",
                    CPF = 123456,
                    Phone = 12345
                }
            }, "A propriedade Email do vendedor deve estar preenchida.");
            yield return new TestCaseData(new RegisterSaleCommand() //Vendedor Sem Phone
            {
                Date = DateTime.Now,
                Items = new List<ItemRequest>() {
                    new ItemRequest
                    {
                        Name="Im",
                        Price = 5,
                        Quantity = 5
                    },
                },
                Seller = new SellerRequest
                {
                    Name = "Im",
                    CPF = 123456,
                    Email = "teste@email.com"
                }
            }, "Phone do vendedor deve ser preenchido.");
        }
        private RegisterSaleCommandHandler GetCommand()
        {
            return new RegisterSaleCommandHandler(
                _moqSaleRepository.Object,
                _moqUnitOfWork.Object,
                _mapper
            );
        }
    }
}
