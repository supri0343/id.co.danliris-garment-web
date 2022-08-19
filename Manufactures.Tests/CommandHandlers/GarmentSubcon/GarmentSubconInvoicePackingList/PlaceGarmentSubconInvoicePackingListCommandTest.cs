using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.InvoicePackingList.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconInvoicePackingList
{
    public class PlaceGarmentSubconInvoicePackingListCommandTest : BaseCommandUnitTest
    {
        private readonly Mock<ISubconInvoicePackingListRepository> _mockSubconInvoicePackingListRepository;
        private readonly Mock<ISubconInvoicePackingListItemRepository> _mockSubconInvoicePackingListItemRepository;

        public PlaceGarmentSubconInvoicePackingListCommandTest()
        {
            _mockSubconInvoicePackingListRepository = CreateMock<ISubconInvoicePackingListRepository>();
            _mockSubconInvoicePackingListItemRepository = CreateMock<ISubconInvoicePackingListItemRepository>();

            _MockStorage.SetupStorage(_mockSubconInvoicePackingListRepository);
            _MockStorage.SetupStorage(_mockSubconInvoicePackingListItemRepository);
        }

        private PlaceGarmentSubconInvoicePackingListCommandHandler CreatePlaceGarmentSubconInvoicePackingListCommandHandler()
        {
            return new PlaceGarmentSubconInvoicePackingListCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid subconContractGuid = Guid.NewGuid();
            PlaceGarmentSubconInvoicePackingListCommandHandler unitUnderTest = CreatePlaceGarmentSubconInvoicePackingListCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconInvoicePackingListCommand GarmentSubconInvoicePackingListCommand = new PlaceGarmentSubconInvoicePackingListCommand()
            {
                InvoiceNo = "test",
                BCType = "test",
                ContractNo = "test",
                Date = DateTimeOffset.Now,
                
                Supplier = new Supplier
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },

                SupplierName ="test",
                SupplierAddress = "test",
                SupplierCode = "test",
                NW = 1,
                GW = 1,
                Remark = "test",
                 
                
                Items = new List<SubconInvoicePackingListItemValueObject>()
                {
                    new SubconInvoicePackingListItemValueObject
                    {
                        Uom=new Uom
                        {
                            Id=1,
                            Unit="unit"
                        },
                        Product=new Product
                        {
                            Id=1,
                            Name="name",
                            Code="code"
                        }
                    }
                }
            };
            _mockSubconInvoicePackingListRepository
                .Setup(s => s.Query)
                .Returns(new List<SubconInvoicePackingListReadModel>().AsQueryable());

            _mockSubconInvoicePackingListRepository
                .Setup(s => s.Update(It.IsAny<SubconInvoicePackingList>()))
                .Returns(Task.FromResult(It.IsAny<SubconInvoicePackingList>()));

            _mockSubconInvoicePackingListItemRepository
                .Setup(s => s.Update(It.IsAny<SubconInvoicePackingListItem>()))
                .Returns(Task.FromResult(It.IsAny<SubconInvoicePackingListItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(GarmentSubconInvoicePackingListCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
