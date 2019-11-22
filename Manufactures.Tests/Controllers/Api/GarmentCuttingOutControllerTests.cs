using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
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
using Manufactures.Domain.GarmentSewingDOs;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentCuttingOutControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentCuttingOutRepository> _mockGarmentCuttingOutRepository;
        private Mock<IGarmentCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
        private Mock<IGarmentCuttingOutDetailRepository> _mockGarmentCuttingOutDetailRepository;
        private Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
        private Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
        private Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
        private Mock<IGarmentSewingDORepository> _mockGarmentSewingDORepository;
        private Mock<IGarmentSewingDOItemRepository> _mockGarmentSewingDOItemRepository;

        public GarmentCuttingOutControllerTests() : base()
        {
            _mockGarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockGarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockGarmentCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();
            _mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockGarmentSewingDORepository = CreateMock<IGarmentSewingDORepository>();
            _mockGarmentSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingDORepository);
            _MockStorage.SetupStorage(_mockGarmentSewingDOItemRepository);
        }

        private GarmentCuttingOutController CreateGarmentCuttingOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentCuttingOutController controller = (GarmentCuttingOutController)Activator.CreateInstance(typeof(GarmentCuttingOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentCuttingOutController();

            _mockGarmentCuttingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingOutReadModel>().AsQueryable());

            Guid cuttingOutGuid = Guid.NewGuid();
            _mockGarmentCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutReadModel>>()))
                .Returns(new List<GarmentCuttingOut>()
                {
                    new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null)
                });

            Guid cuttingOutItemGuid = Guid.NewGuid();
            GarmentCuttingOutItem garmentCuttingOutItem = new GarmentCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null, 1);
            _mockGarmentCuttingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>()
                {
                    garmentCuttingOutItem.GetReadModel()
                }.AsQueryable());

            _mockGarmentCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutItemReadModel>>()))
                .Returns(new List<GarmentCuttingOutItem>()
                {
                    new GarmentCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null, 1)
                });

            GarmentCuttingOutDetail garmentCuttingOutDetail = new GarmentCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), null, null, 1, 1, new UomId(1), null, 1, 1);
            _mockGarmentCuttingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>()
                {
                    garmentCuttingOutDetail.GetReadModel()
                }.AsQueryable());

            _mockGarmentCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutDetailReadModel>>()))
                .Returns(new List<GarmentCuttingOutDetail>()
                {
                    new GarmentCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), null, null, 1, 1, new UomId(1), null, 1, 1)
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _mockGarmentCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOut>()
                {
                    new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null)
                });

            Guid cuttingOutItemGuid = Guid.NewGuid();
            _mockGarmentCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOutItem>()
                {
                    new GarmentCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null, 1)
                });

            _mockGarmentCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOutDetail>()
                {
                    new GarmentCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), null, null, 1, 1, new UomId(1), null, 1, 1)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentCuttingOutCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentCuttingOutCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();

            //_mockGarmentSewingDORepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingDOReadModel>()
            //    {
            //        new GarmentSewingDOReadModel(Guid.NewGuid())
            //    }.AsQueryable());

            _mockGarmentSewingDORepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOReadModel, bool>>>()))
               .Returns(new List<GarmentSewingDO>()
               {
                    new GarmentSewingDO(Guid.NewGuid(), null, Guid.NewGuid(), new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new GarmentComodityId(1), null, null, DateTimeOffset.Now)
               });

            _mockGarmentSewingDOItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOItemReadModel, bool>>>()))
               .Returns(new List<GarmentSewingDOItem>()
               {
                    new GarmentSewingDOItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0, 0)
               });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}