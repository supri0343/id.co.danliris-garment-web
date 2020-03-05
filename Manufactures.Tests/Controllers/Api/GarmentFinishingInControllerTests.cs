using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
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
    public class GarmentFinishingInControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockSewingOutItemRepository;

        public GarmentFinishingInControllerTests() : base()
        {
            _mockFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
        }

        private GarmentFinishingInController CreateGarmentFinishingInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentFinishingInController controller = (GarmentFinishingInController)Activator.CreateInstance(typeof(GarmentFinishingInController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentFinishingInController();

            _mockFinishingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentFinishingInReadModel>().AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingInReadModel>>()))
                .Returns(new List<GarmentFinishingIn>()
                {
                    new GarmentFinishingIn(Guid.NewGuid(), null,null , new UnitDepartmentId(1), null, null, "RONo",null,new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null, 0,null)
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>()
                {
                    new GarmentFinishingInItemReadModel(Guid.NewGuid())
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
            var unitUnderTest = CreateGarmentFinishingInController();

            _mockFinishingInRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingInReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingIn>()
                {
                    new GarmentFinishingIn(Guid.NewGuid(), null,null , new UnitDepartmentId(1), null, null, "RONo",null,new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null, 0,null)
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingInItem>()
                {
                    new GarmentFinishingInItem(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(), Guid.Empty,new SizeId(1), null, new ProductId(1), null, null, null, 1,1,new UomId(1),null, null,1,1)
                });

            //_mockSewingOutItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingOutItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingIn(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentFinishingInCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingIn(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
