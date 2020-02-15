using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents;
using Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class GarmentPreparingControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;
		
		public GarmentPreparingControllerTests() : base()
        {
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);

		}

        private GarmentPreparingController CreateGarmentPreparingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentPreparingController controller = (GarmentPreparingController)Activator.CreateInstance(typeof(GarmentPreparingController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentPreparingReadModel>().AsQueryable());

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingReadModel>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, false)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingItemReadModel>>()))
                .Returns(new List<GarmentPreparingItem>()
                {
                    new GarmentPreparingItem(Guid.NewGuid(), 0, new ProductId(1), null, null, null, 0, new UomId(1), null, null, 0, 0, Guid.NewGuid())
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>()
                {
                    new GarmentPreparingItemReadModel(Guid.NewGuid())
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
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, false)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingItemReadModel, bool>>>()))
                .Returns(new List<GarmentPreparingItem>()
                {
                    new GarmentPreparingItem(Guid.NewGuid(), 0, new ProductId(1), null, null, null, 0, new UomId(1), null, null, 0, 0, Guid.NewGuid())
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
            var unitUnderTest = CreateGarmentPreparingController();

            PlaceGarmentPreparingCommand command = new PlaceGarmentPreparingCommand();
            command.UENId = 1;

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>());

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentPreparingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, false));

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, false)
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentPreparingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, false));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetLoaderByRO_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentPreparingReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.GetLoaderByRO(It.IsAny<string>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

		[Fact]
		public async Task GetMonitoringBehavior()
		{
			var unitUnderTest = CreateGarmentPreparingController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringPrepareQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringPrepareListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoring(1,DateTime.Now,DateTime.Now,1,25,"{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetXLSPrepareBehavior()
		{
			var unitUnderTest = CreateGarmentPreparingController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsPrepareQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
		}
	}
}