using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Commands;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingDOs.ValueObjects;
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
    public class GarmentSewingDOControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSewingDORepository> _mockGarmentSewingDORepository;
        private Mock<IGarmentSewingDOItemRepository> _mockGarmentSewingDOItemRepository;

        public GarmentSewingDOControllerTests() : base()
        {
            _mockGarmentSewingDORepository = CreateMock<IGarmentSewingDORepository>();
            _mockGarmentSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSewingDORepository);
            _MockStorage.SetupStorage(_mockGarmentSewingDOItemRepository);
        }

        private GarmentDeliveryReturnController CreateGarmentDeliveryReturnController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentDeliveryReturnController controller = (GarmentDeliveryReturnController)Activator.CreateInstance(typeof(GarmentDeliveryReturnController), _MockServiceProvider.Object);
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

        //[Fact]
        //public async Task Get_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentDeliveryReturnController();

        //    _mockGarmentDeliveryReturnRepository
        //        .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
        //        .Returns(new List<GarmentDeliveryReturnReadModel>().AsQueryable());

        //    _mockGarmentDeliveryReturnRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentDeliveryReturnReadModel>>()))
        //        .Returns(new List<GarmentDeliveryReturn>()
        //        {
        //            new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false)
        //        });

        //    _mockGarmentDeliveryReturnItemRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentDeliveryReturnItemReadModel>>()))
        //        .Returns(new List<GarmentDeliveryReturnItem>()
        //        {
        //            new GarmentDeliveryReturnItem(Guid.NewGuid(), Guid.NewGuid(), 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null)
        //        });

        //    _mockGarmentDeliveryReturnItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentDeliveryReturnItemReadModel>()
        //        {
        //            new GarmentDeliveryReturnItemReadModel(Guid.NewGuid())
        //        }.AsQueryable());

        //    // Act
        //    var result = await unitUnderTest.Get();

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        //[Fact]
        //public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentDeliveryReturnController();

        //    _mockGarmentDeliveryReturnRepository
        //        .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
        //        .Returns(new List<GarmentDeliveryReturn>()
        //        {
        //            new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false)
        //        });

        //    _mockGarmentDeliveryReturnItemRepository
        //        .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnItemReadModel, bool>>>()))
        //        .Returns(new List<GarmentDeliveryReturnItem>()
        //        {
        //            new GarmentDeliveryReturnItem(Guid.NewGuid(), Guid.NewGuid(), 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null)
        //        });

        //    // Act
        //    var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        //[Fact]
        //public async Task Post_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentDeliveryReturnController();

        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<PlaceGarmentDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

        //    // Act
        //    var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentDeliveryReturnCommand>());

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        //[Fact]
        //public async Task Put_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentDeliveryReturnController();

        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<UpdateGarmentDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

        //    // Act
        //    var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentDeliveryReturnCommand());

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        //[Fact]
        //public async Task Delete_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentDeliveryReturnController();

        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<RemoveGarmentDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

        //    // Act
        //    var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}
    }
}