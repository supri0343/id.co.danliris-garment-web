using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentServiceSubconSewingControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentServiceSubconSewingRepository> _mockGarmentServiceSubconSewingRepository;
        private Mock<IGarmentServiceSubconSewingItemRepository> _mockGarmentServiceSubconSewingItemRepository;

        public GarmentServiceSubconSewingControllerTests() : base()
        {
            _mockGarmentServiceSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockGarmentServiceSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentServiceSubconSewingRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSubconSewingItemRepository);
        }

        private GarmentServiceSubconSewingController CreateGarmentServiceSubconSewingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentServiceSubconSewingController controller = (GarmentServiceSubconSewingController)Activator.CreateInstance(typeof(GarmentServiceSubconSewingController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentServiceSubconSewingController();

            _mockGarmentServiceSubconSewingRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentServiceSubconSewingReadModel>().AsQueryable());

            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _mockGarmentServiceSubconSewingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconSewingReadModel>>()))
                .Returns(new List<GarmentServiceSubconSewing>()
                {
                    new GarmentServiceSubconSewing(serviceSubconSewingGuid,null,new BuyerId(1),null,null,new UnitDepartmentId(1),null,null,"RONo",null,new GarmentComodityId(1),null,null,DateTimeOffset.Now,true)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid serviceSubconSewingItemGuid = Guid.NewGuid();
            GarmentServiceSubconSewingItem garmentServiceSubconSewingItem = new GarmentServiceSubconSewingItem(serviceSubconSewingItemGuid, serviceSubconSewingGuid, sewingInGuid, sewingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null);

            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingItemReadModel>()
                {
                    garmentServiceSubconSewingItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _mockGarmentServiceSubconSewingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewing>()
                {
                    new GarmentServiceSubconSewing(serviceSubconSewingGuid,null,new BuyerId(1),null,null,new UnitDepartmentId(1),null,null,"RONo",null,new GarmentComodityId(1),null,null,DateTimeOffset.Now,true)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid serviceSubconSewingItemGuid = Guid.NewGuid();
            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewingItem>()
                {
                    new GarmentServiceSubconSewingItem(serviceSubconSewingItemGuid, serviceSubconSewingGuid, sewingInGuid, sewingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSubconSewingCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSubconSewingCommand>()));
        }
    }
}
