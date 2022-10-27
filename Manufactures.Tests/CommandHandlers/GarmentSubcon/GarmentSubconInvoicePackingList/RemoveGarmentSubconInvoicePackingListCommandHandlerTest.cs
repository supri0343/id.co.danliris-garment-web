using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.InvoicePackingList.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
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
    public class RemoveGarmentSubconInvoicePackingListCommandHandlerTest : BaseCommandUnitTest
    {

        private readonly Mock<ISubconInvoicePackingListRepository> _mockSubconInvoicePackingListRepository;
        private readonly Mock<ISubconInvoicePackingListItemRepository> _mockSubconInvoicePackingListItemRepository;

        public RemoveGarmentSubconInvoicePackingListCommandHandlerTest()
        {
            _mockSubconInvoicePackingListRepository = CreateMock<ISubconInvoicePackingListRepository>();
            _mockSubconInvoicePackingListItemRepository = CreateMock<ISubconInvoicePackingListItemRepository>();

            _MockStorage.SetupStorage(_mockSubconInvoicePackingListRepository);
            _MockStorage.SetupStorage(_mockSubconInvoicePackingListItemRepository);
        }

        private RemoveGarmentSubconInvoicePackingListCommandHandler CreateRemoveGarmentSubconInvoicePackingListCommandHandler()
        {
            return new RemoveGarmentSubconInvoicePackingListCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SubconInvoicePackingListGuid = Guid.NewGuid();
            Guid SubconInvoicePackingListItemGuid = Guid.NewGuid();
            RemoveGarmentSubconInvoicePackingListCommandHandler unitUnderTest = CreateRemoveGarmentSubconInvoicePackingListCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconInvoicePackingListCommand RemoveGarmentServiceSubconSewingCommand = new RemoveGarmentSubconInvoicePackingListCommand(SubconInvoicePackingListGuid);

            _mockSubconInvoicePackingListRepository
                .Setup(s => s.Query)
                .Returns(new List<SubconInvoicePackingListReadModel>()
                {
                    new SubconInvoicePackingListReadModel(SubconInvoicePackingListGuid)
                }.AsQueryable());
            //_mockSubconInvoicePackingListRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<SubconInvoicePackingListReadModel>().AsQueryable());


            _mockSubconInvoicePackingListItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<SubconInvoicePackingListItemReadModel, bool>>>()))
                .Returns(new List<SubconInvoicePackingListItem>()
                {
                    new SubconInvoicePackingListItem(
                       SubconInvoicePackingListItemGuid, 
                       SubconInvoicePackingListGuid,  
                       "", 
                       DateTimeOffset.Now, 
                       new Domain.Shared.ValueObjects.ProductId(1), 
                       "productCode", 
                       "productName", 
                       "productRemark", 
                       "designColor", 
                       1, 
                       new Domain.Shared.ValueObjects.UomId(1), 
                       "uomUnit", 
                       1,
                       1,
                       1,
                       1)
                });
            //_mockSubconInvoicePackingListRepository
            //     .Setup(s => s.Query)
            //     .Returns(new List<SubconInvoicePackingListReadModel>().AsQueryable());

            _mockSubconInvoicePackingListRepository
                .Setup(s => s.Update(It.IsAny<SubconInvoicePackingList>()))
                .Returns(Task.FromResult(It.IsAny<SubconInvoicePackingList>()));

            _mockSubconInvoicePackingListItemRepository
                .Setup(s => s.Update(It.IsAny<SubconInvoicePackingListItem>()))
                .Returns(Task.FromResult(It.IsAny<SubconInvoicePackingListItem>()));

            //_mockServiceSubconSewingRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewing>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewing>()));
            //_mockServiceSubconSewingItemRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingItem>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingItem>()));
            //_mockServiceSubconSewingDetailRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingDetail>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSubconSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
