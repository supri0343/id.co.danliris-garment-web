using Barebone.Tests;
using Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsByRONo;
using Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsDynamic;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
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
    public class GarmentSewingOutControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentSewingOutRepository> _mockGarmentSewingOutRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
        private readonly Mock<IGarmentSewingOutDetailRepository> _mockGarmentSewingOutDetailRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;

        public GarmentSewingOutControllerTests() : base()
        {
            _mockGarmentSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();
            _mockGarmentSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();
            _mockGarmentSewingOutDetailRepository = CreateMock<IGarmentSewingOutDetailRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingOutDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
        }

        private GarmentSewingOutController CreateGarmentSewingOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSewingOutController controller = (GarmentSewingOutController)Activator.CreateInstance(typeof(GarmentSewingOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSewingOutController();

            _mockGarmentSewingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSewingOutReadModel>().AsQueryable());

            Guid sewingOutGuid = Guid.NewGuid();
            _mockGarmentSewingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingOutReadModel>>()))
                .Returns(new List<GarmentSewingOut>()
                {
                    new GarmentSewingOut(sewingOutGuid, null,new BuyerId(1),null,null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            GarmentSewingOutItem garmentSewingOutItem = new GarmentSewingOutItem(sewingOutItemGuid, sewingOutGuid, sewingInGuid, sewingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1,1,1);
            _mockGarmentSewingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutItemReadModel>()
                {
                    garmentSewingOutItem.GetReadModel()
                }.AsQueryable());

            GarmentSewingOutDetail garmentSewingOutDetail = new GarmentSewingOutDetail(Guid.NewGuid(), sewingOutItemGuid, new SizeId(1), null, 1, new UomId(1), null);
            _mockGarmentSewingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutDetailReadModel>()
                {
                    garmentSewingOutDetail.GetReadModel()
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
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _mockGarmentSewingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSewingOut>()
                {
                    new GarmentSewingOut(sewingOutGuid, null,new BuyerId(1),null,null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            _mockGarmentSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingOutItem>()
                {
                    new GarmentSewingOutItem(sewingOutItemGuid, sewingOutGuid, sewingInGuid, sewingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1,1,1)
                });

            _mockGarmentSewingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSewingOutDetail>()
                {
                    new GarmentSewingOutDetail(Guid.NewGuid(), sewingOutItemGuid, new SizeId(1), null, 1, new UomId(1), null)
                });

            //_mockSewingInItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingInItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSewingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSewingOut(sewingOutGuid, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSewingOutCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSewingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSewingOut(sewingOutGuid, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSewingOutCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSewingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSewingOut(sewingOutGuid, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetByRONo_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetGarmentSewingOutsByRONoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSewingOutsByRONoViewModel(new List<GarmentSewingOutByRONoDto>()));

            // Act
            var result = await unitUnderTest.GetLoaderByRO(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetDynamic_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetGarmentSewingOutsDynamicQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSewingOutsDynamicViewModel(1, new List<dynamic>()));

            // Act
            var result = await unitUnderTest.GetDynamic();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
