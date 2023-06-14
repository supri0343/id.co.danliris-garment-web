using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentSubconInvoicePackingListControllerTests : BaseControllerUnitTest
    {
        private Mock<ISubconInvoicePackingListRepository> _mockSubconInvoicePackingListRepository;
        private Mock<ISubconInvoicePackingListItemRepository> _mockSubconInvoicePackingListItemRepository;

        public GarmentSubconInvoicePackingListControllerTests() : base()
        {
            _mockSubconInvoicePackingListRepository = CreateMock<ISubconInvoicePackingListRepository>();
            _mockSubconInvoicePackingListItemRepository = CreateMock<ISubconInvoicePackingListItemRepository>();

            _MockStorage.SetupStorage(_mockSubconInvoicePackingListRepository);
            _MockStorage.SetupStorage(_mockSubconInvoicePackingListItemRepository);
        }

        private GarmentSubconInvoicePackingListController CreateGarmentSubconInvoicePackingListController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconInvoicePackingListController controller = (GarmentSubconInvoicePackingListController)Activator.CreateInstance(typeof(GarmentSubconInvoicePackingListController), _MockServiceProvider.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        [Fact]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconInvoicePackingListController();

            _mockSubconInvoicePackingListRepository
               .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns(new List<SubconInvoicePackingListReadModel>().AsQueryable());

            Guid SubconInvoicePackingListGuid = Guid.NewGuid();
            _mockSubconInvoicePackingListRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<SubconInvoicePackingListReadModel>>()))
                .Returns(new List<SubconInvoicePackingList>()
                {
                    new SubconInvoicePackingList(SubconInvoicePackingListGuid, "", "",DateTimeOffset.Now,new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", "", 1,1,"","", new Guid(),"")
                });

            Guid SubconInvoicePackingListItemGuid = Guid.NewGuid();
           // SubconInvoicePackingListItem garmentSubconDeliveryLetterOutItem = new SubconInvoicePackingListItem(Guid.NewGuid(), SubconDeliveryLetterOutGuid, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "");
            SubconInvoicePackingListItem SubconInvoicePackingListItem = new SubconInvoicePackingListItem(Guid.NewGuid(), SubconInvoicePackingListGuid,  "", DateTimeOffset.Now, new Domain.Shared.ValueObjects.ProductId(1), "productCode", "productName", "productRemark", "designColor", 1, new Domain.Shared.ValueObjects.UomId(1), "uomUnit", 1,1,1,1);

            _mockSubconInvoicePackingListItemRepository
                .Setup(s => s.Query)
                .Returns(new List<SubconInvoicePackingListItemReadModel>() {
                    SubconInvoicePackingListItem.GetReadModel()
                }.AsQueryable());

            //_mockSubconInvoicePackingListItemRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<SubconInvoicePackingListItemReadModel>>()))
            //    .Returns(new List<SubconInvoicePackingListItem>()
            //    {
            //        new SubconInvoicePackingListItem(Guid.NewGuid(), SubconInvoicePackingListGuid,  "", DateTimeOffset.Now, new Domain.Shared.ValueObjects.ProductId(1), "productCode", "productName", "productRemark", "designColor", 1, new Domain.Shared.ValueObjects.UomId(1), "uomUnit", 1,1)
            //    });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconInvoicePackingListController();

            //Guid SubconInvoicePackingListGuid = Guid.NewGuid();
            //_mockSubconInvoicePackingListRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<SubconInvoicePackingListReadModel>>()))
            //    .Returns(new List<SubconInvoicePackingList>()
            //    {
            //        new SubconInvoicePackingList(SubconInvoicePackingListGuid, "", "",DateTimeOffset.Now,new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", "", 1,1,"")
            //    });

            //Guid SubconInvoicePackingListItemGuid = Guid.NewGuid();
            //// SubconInvoicePackingListItem garmentSubconDeliveryLetterOutItem = new SubconInvoicePackingListItem(Guid.NewGuid(), SubconDeliveryLetterOutGuid, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "");
            //SubconInvoicePackingListItem SubconInvoicePackingListItem = new SubconInvoicePackingListItem(Guid.NewGuid(), SubconInvoicePackingListGuid, "", DateTimeOffset.Now, new Domain.Shared.ValueObjects.ProductId(1), "productCode", "productName", "productRemark", "designColor", 1, new Domain.Shared.ValueObjects.UomId(1), "uomUnit", 1, 1);

            //_mockSubconInvoicePackingListItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<SubconInvoicePackingListItemReadModel>() {
            //        SubconInvoicePackingListItem.GetReadModel()
            //    }.AsQueryable());

            _mockSubconInvoicePackingListRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<SubconInvoicePackingListReadModel, bool>>>()))
               .Returns(new List<SubconInvoicePackingList>()
               {
                    //new GarmentSubconDeliveryLetterOut(Guid.NewGuid(), null,null,new Guid(),"","",DateTimeOffset.Now,1,"","",1,"", false,"","")
                    new SubconInvoicePackingList(Guid.NewGuid(), "", "",DateTimeOffset.Now,new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", "", 1,1,"","", new Guid(),"")
               });

            _mockSubconInvoicePackingListItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<SubconInvoicePackingListItemReadModel, bool>>>()))
                .Returns(new List<SubconInvoicePackingListItem>()
                {
                    //new GarmentSubconDeliveryLetterOutItem(Guid.NewGuid(),new Guid(),1,new Domain.Shared.ValueObjects.ProductId(1),"code","name","remark","color",1,new Domain.Shared.ValueObjects.UomId(1),"unit",new Domain.Shared.ValueObjects.UomId(1),"unit","fabType",new Guid(),"","","")
                    new SubconInvoicePackingListItem(Guid.NewGuid(), new Guid(), "", DateTimeOffset.Now, new Domain.Shared.ValueObjects.ProductId(1), "productCode", "productName", "productRemark", "designColor", 1, new Domain.Shared.ValueObjects.UomId(1), "uomUnit", 1, 1,1,1)
                });



            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconInvoicePackingListController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconInvoicePackingListCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconInvoicePackingListCommand>()));
        }



        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconInvoicePackingListController();
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSubconInvoicePackingListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubconInvoicePackingList(subconDeliveryLetterOutGuid, "", "", DateTimeOffset.Now, new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", "", 1, 1, "","", new Guid(), ""));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSubconInvoicePackingListCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconInvoicePackingListController();
            var SubconInvoicePackingListGuid = Guid.NewGuid();

            //_mockSubconInvoicePackingListRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<SubconInvoicePackingListReadModel, bool>>>()))
            //    .Returns(new List<SubconInvoicePackingList>()
            //    {
            //       new SubconInvoicePackingList(SubconInvoicePackingListGuid, "", "",DateTimeOffset.Now,new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", "", 1,1,"")
            //    });


            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconInvoicePackingListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubconInvoicePackingList(SubconInvoicePackingListGuid, "InvoiceNo", "", DateTimeOffset.Now, new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", "", 1, 1, "","", new Guid(), ""));

            // Act
            var result = await unitUnderTest.Delete(SubconInvoicePackingListGuid.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconInvoicePackingListController();
            //Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid SubconInvoicePackingListGuid = Guid.NewGuid();
            Guid SubconInvoicePackingListitemGuid = Guid.NewGuid();
            _mockSubconInvoicePackingListRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<SubconInvoicePackingListReadModel, bool>>>()))
               .Returns(new List<SubconInvoicePackingList>()
               {
                    //new GarmentSubconDeliveryLetterOut(Guid.NewGuid(), null,null,new Guid(),"","",DateTimeOffset.Now,1,"","",1,"", false,"","")
                    new SubconInvoicePackingList(SubconInvoicePackingListGuid, "", "",DateTimeOffset.Now,new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", "", 1,1,"","", new Guid(),"")
               });

            _mockSubconInvoicePackingListItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<SubconInvoicePackingListItemReadModel, bool>>>()))
                .Returns(new List<SubconInvoicePackingListItem>()
                {
                    //new GarmentSubconDeliveryLetterOutItem(Guid.NewGuid(),new Guid(),1,new Domain.Shared.ValueObjects.ProductId(1),"code","name","remark","color",1,new Domain.Shared.ValueObjects.UomId(1),"unit",new Domain.Shared.ValueObjects.UomId(1),"unit","fabType",new Guid(),"","","")
                    new SubconInvoicePackingListItem(SubconInvoicePackingListitemGuid, SubconInvoicePackingListGuid, "", DateTimeOffset.Now, new Domain.Shared.ValueObjects.ProductId(1), "productCode", "productName", "productRemark", "designColor", 1, new Domain.Shared.ValueObjects.UomId(1), "uomUnit", 1, 1,1,1)
                });

            //_mockSewingInItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingInItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString());

            // Assert
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }


    }
}
