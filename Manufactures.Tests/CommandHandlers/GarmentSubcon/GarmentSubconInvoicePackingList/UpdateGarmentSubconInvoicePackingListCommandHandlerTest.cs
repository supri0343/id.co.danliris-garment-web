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
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconInvoicePackingList
{
    public class UpdateGarmentSubconInvoicePackingListCommandHandlerTest : BaseCommandUnitTest
    { 
        private readonly Mock<ISubconInvoicePackingListRepository> _mockSubconInvoicePackingListRepository;
        private readonly Mock<ISubconInvoicePackingListItemRepository> _mockSubconInvoicePackingListItemRepository;

        public UpdateGarmentSubconInvoicePackingListCommandHandlerTest()
        {
            _mockSubconInvoicePackingListRepository = CreateMock<ISubconInvoicePackingListRepository>();
            _mockSubconInvoicePackingListItemRepository = CreateMock<ISubconInvoicePackingListItemRepository>();

            _MockStorage.SetupStorage(_mockSubconInvoicePackingListRepository);
            _MockStorage.SetupStorage(_mockSubconInvoicePackingListItemRepository);
        }

        private UpdateGarmentSubconInvoicePackingListCommandHandler CreateUpdateGarmentSubconInvoicePackingListCommandHandler()
        {
            return new UpdateGarmentSubconInvoicePackingListCommandHandler(_MockStorage.Object, _MockWebApiContext.Object, _MockHttpService.Object);
        }


        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SubconInvoicePackingListGuid = Guid.NewGuid();
            Guid SubconInvoicePackingListItemGuid = Guid.NewGuid();
            UpdateGarmentSubconInvoicePackingListCommandHandler unitUnderTest = CreateUpdateGarmentSubconInvoicePackingListCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconInvoicePackingListCommand UpdateGarmentSubconInvoicePackingListCommand = new UpdateGarmentSubconInvoicePackingListCommand()
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
                        Id =SubconInvoicePackingListItemGuid,
                        InvoicePackingListId =  SubconInvoicePackingListGuid,
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

            UpdateGarmentSubconInvoicePackingListCommand.SetIdentity(SubconInvoicePackingListGuid);

            _mockSubconInvoicePackingListRepository
                .Setup(s => s.Query)
                .Returns(new List<SubconInvoicePackingListReadModel>()
                {
                    new SubconInvoicePackingListReadModel(SubconInvoicePackingListGuid)
                }.AsQueryable());

            _mockSubconInvoicePackingListItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<SubconInvoicePackingListItemReadModel, bool>>>()))
                .Returns(new List<SubconInvoicePackingListItem>()
                {
                    new SubconInvoicePackingListItem(SubconInvoicePackingListItemGuid, SubconInvoicePackingListGuid,  "dlNo", DateTimeOffset.Now, new Domain.Shared.ValueObjects.ProductId(1), "productCode", "productName", "productRemark", "designColor", 1, new Domain.Shared.ValueObjects.UomId(1), "uomUnit", 1,1,1,1)
                });


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
            var result = await unitUnderTest.Handle(UpdateGarmentSubconInvoicePackingListCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }



    }
}
