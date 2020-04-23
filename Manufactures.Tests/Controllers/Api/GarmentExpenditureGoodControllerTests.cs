using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
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

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentExpenditureGoodControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockGarmentExpenditureGoodRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockGarmentExpenditureGoodItemRepository;

        public GarmentExpenditureGoodControllerTests() : base()
        {
            _mockGarmentExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
            _mockGarmentExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockGarmentExpenditureGoodItemRepository);
        }

        private GarmentExpenditureGoodController CreateGarmentExpenditureGoodController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentExpenditureGoodController controller = (GarmentExpenditureGoodController)Activator.CreateInstance(typeof(GarmentExpenditureGoodController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1,0, new UomId(1), null, null, 1, 1);
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>()
                {
                    garmentExpenditureGoodItem.GetReadModel()
                }.AsQueryable());

            
            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodItemReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGoodItem>()
                {
                    new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1,0, new UomId(1), null, null, 1, 1)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodItemReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGoodItem>()
                {
                    new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), "size", 1,0, new UomId(1), null, "desc", 1, 1)
                });

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString(), "buyerCode");

            // Assert
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }



        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentExpenditureGoodCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false)
                );

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentExpenditureGoodCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false)
                );

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
