using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PaymentAPI.Application.Commands;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using PaymentAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace payment_api.Unit.Tests.Commands
{
    public class UpdateSaleCommandTests
    {
        private Mock<ISaleRepository> _moqSaleRepository;
        private Mock<IUnitOfWork> _moqUnitOfWork;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _moqUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Default);
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new UpdateSaleCommandMapping())));
            _moqSaleRepository = new Mock<ISaleRepository>(MockBehavior.Default);
        }
        [Test]
        public async Task Dado_um_comando_valido_a_venda_deve_ser_alterada()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            var sale = _mapper.Map<Sale>(command);
            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns(sale);
            _moqSaleRepository.Setup(s => s.UpdateSaleById(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        [TestCaseSource(nameof(VendaNaoPodeSerAlterada))]
        public async Task Dado_um_comando_invalido_a_venda_nao_deve_ser_alterada_com_a_regra_de_alteracao_do_status(SaleStatus currentStatus,UpdateSaleCommand command)
        {
            // Arrange
            var sale = _mapper.Map<Sale>(command);
            sale.Status = currentStatus;
            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns(sale);
            _moqSaleRepository.Setup(s => s.UpdateSaleById(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("O Status não pode ser alterado");
        }
        [Test]
        [TestCaseSource(nameof(VendaPodeSerAlterada))]
        public async Task Dado_um_comando_valido_a_venda_deve_ser_alterada_com_a_regra_de_alteracao_do_status(SaleStatus currentStatus, UpdateSaleCommand command)
        {
            // Arrange
            var sale = _mapper.Map<Sale>(command);
            sale.Status = currentStatus;
            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns(sale);
            _moqSaleRepository.Setup(s => s.UpdateSaleById(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
        [Test]
        public async Task Dado_um_pedido_sem_itens_o_mesmo_nao_deve_ser_alterado()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns(sale);
            _moqSaleRepository.Setup(s => s.UpdateSaleById(It.IsAny<Sale>())).Returns(sale);

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("A venda tem que ter pelo menos 1 item.");
        }
        [Test]
        public async Task Dado_um_pedido_sem_vendedor_o_mesmo_nao_deve_ser_alterado()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns(sale);
            _moqSaleRepository.Setup(s => s.UpdateSaleById(It.IsAny<Sale>())).Returns(sale);            

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("A venda deve ter um vendedor");
        }
        [Test]
        public async Task Dado_um_pedido_sem_data_o_mesmo_nao_deve_ser_alterado()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns(sale);
            _moqSaleRepository.Setup(s => s.UpdateSaleById(It.IsAny<Sale>())).Returns(sale);            

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("A venda deve ter uma data");
        }

        [Test]
        [TestCaseSource(nameof(ComandoComItensInvalidos))]
        [TestCaseSource(nameof(ComandoComVendedoresInvalidos))]
        public async Task Dado_um_pedido_com_itens_invalidos_o_mesmo_nao_deve_ser_alterado(UpdateSaleCommand command, string expectedResult)
        {
            // Arrange
            var sale = _mapper.Map<Sale>(command);
            _moqSaleRepository.Setup(s => s.GetById(It.IsAny<long>())).Returns(sale);
            _moqSaleRepository.Setup(s => s.UpdateSaleById(It.IsAny<Sale>())).Returns(sale);            

            // Act
            var result = await GetCommand().Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(expectedResult);
        }
        private static IEnumerable<TestCaseData> ComandoComItensInvalidos()
        {
            yield return new TestCaseData(new UpdateSaleCommand() //Item Com nome pequeno
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Item Sem Nome
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Item Com preço negativo
            {
                Status = SaleStatus.AwaitingPayment,
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

            yield return new TestCaseData(new UpdateSaleCommand() //Item Sem preço
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Item Com quantidade invalida
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Item Sem quantidade
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Vendedor Com nome pequeno
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Vendedor Sem CPF
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Vendedor Com Email Invalido
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Vendedor Sem Email
            {
                Status = SaleStatus.AwaitingPayment,
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
            yield return new TestCaseData(new UpdateSaleCommand() //Vendedor Sem Phone
            {
                Status = SaleStatus.AwaitingPayment,
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
        private static IEnumerable<TestCaseData> VendaPodeSerAlterada()
        {
            yield return new TestCaseData(SaleStatus.AwaitingPayment, new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            });
            yield return new TestCaseData(SaleStatus.Canceled, new UpdateSaleCommand
            {
                Status = SaleStatus.Canceled,
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
            });
            yield return new TestCaseData(SaleStatus.Delivered, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.ApprovedPayment, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.SentToCarrier, new UpdateSaleCommand
            {
                Status = SaleStatus.SentToCarrier,
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
            });
            yield return new TestCaseData(SaleStatus.SentToCarrier, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.AwaitingPayment, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.AwaitingPayment, new UpdateSaleCommand
            {
                Status = SaleStatus.Canceled,
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
            });
            yield return new TestCaseData(SaleStatus.ApprovedPayment, new UpdateSaleCommand
            {
                Status = SaleStatus.SentToCarrier,
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
            });
            yield return new TestCaseData(SaleStatus.ApprovedPayment, new UpdateSaleCommand
            {
                Status = SaleStatus.Canceled,
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
            });
        }
        private static IEnumerable<TestCaseData> VendaNaoPodeSerAlterada()
        {
            yield return new TestCaseData(SaleStatus.Delivered,new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            });
            yield return new TestCaseData(SaleStatus.Delivered, new UpdateSaleCommand
            {
                Status = SaleStatus.Canceled,
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
            });
            yield return new TestCaseData(SaleStatus.Delivered, new UpdateSaleCommand
            {
                Status = SaleStatus.SentToCarrier,
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
            });
            yield return new TestCaseData(SaleStatus.Delivered, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.Canceled, new UpdateSaleCommand
            {
                Status = SaleStatus.SentToCarrier,
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
            });
            yield return new TestCaseData(SaleStatus.Canceled, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.Canceled, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.Canceled, new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            });
            yield return new TestCaseData(SaleStatus.AwaitingPayment, new UpdateSaleCommand
            {
                Status = SaleStatus.SentToCarrier,
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
            });
            yield return new TestCaseData(SaleStatus.AwaitingPayment, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.ApprovedPayment, new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            });
            yield return new TestCaseData(SaleStatus.ApprovedPayment, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.SentToCarrier, new UpdateSaleCommand
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
            });
            yield return new TestCaseData(SaleStatus.SentToCarrier, new UpdateSaleCommand
            {
                Status = SaleStatus.AwaitingPayment,
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
            });
            yield return new TestCaseData(SaleStatus.SentToCarrier, new UpdateSaleCommand
            {
                Status = SaleStatus.Canceled,
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
            });
        }
        private UpdateSaleCommandHandler GetCommand()
        {
            return new UpdateSaleCommandHandler(
                _moqSaleRepository.Object,
                _moqUnitOfWork.Object,
                _mapper
            );
        }
    }
}
