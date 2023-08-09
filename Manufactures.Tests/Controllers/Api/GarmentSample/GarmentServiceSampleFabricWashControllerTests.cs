using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.Queries;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
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

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentServiceSampleFabricWashControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentServiceSampleFabricWashRepository> _mockGarmentServiceSampleFabricWashRepository;
        private Mock<IGarmentServiceSampleFabricWashItemRepository> _mockGarmentServiceSampleFabricWashItemRepository;
        private Mock<IGarmentServiceSampleFabricWashDetailRepository> _mockServiceSampleFabricWashDetailRepository;

        public GarmentServiceSampleFabricWashControllerTests() : base()
        {
            _mockGarmentServiceSampleFabricWashRepository = CreateMock<IGarmentServiceSampleFabricWashRepository>();
            _mockGarmentServiceSampleFabricWashItemRepository = CreateMock<IGarmentServiceSampleFabricWashItemRepository>();
            _mockServiceSampleFabricWashDetailRepository = CreateMock<IGarmentServiceSampleFabricWashDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentServiceSampleFabricWashRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSampleFabricWashItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleFabricWashDetailRepository);
        }

        private GarmentServiceSampleFabricWashController CreateGarmentServiceSampleFabricWashController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentServiceSampleFabricWashController controller = (GarmentServiceSampleFabricWashController)Activator.CreateInstance(typeof(GarmentServiceSampleFabricWashController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentServiceSampleFabricWashController();

            _mockGarmentServiceSampleFabricWashRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentServiceSampleFabricWashReadModel>().AsQueryable());

            Guid serviceSampleFabricWashGuid = Guid.NewGuid();
            _mockGarmentServiceSampleFabricWashRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleFabricWashReadModel>>()))
                .Returns(new List<GarmentServiceSampleFabricWash>()
                {
                    new GarmentServiceSampleFabricWash(serviceSampleFabricWashGuid,null,  DateTimeOffset.Now, "",false, 0, null, 0, 0)
                });

            Guid serviceSampleFabricWashItemGuid = Guid.NewGuid();
            GarmentServiceSampleFabricWashItem garmentServiceSampleFabricWashItem = new GarmentServiceSampleFabricWashItem(serviceSampleFabricWashItemGuid, serviceSampleFabricWashGuid, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null);

            _mockGarmentServiceSampleFabricWashItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleFabricWashItemReadModel>()
                {
                    garmentServiceSampleFabricWashItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentServiceSampleFabricWashController();
            Guid serviceSampleFabricWashGuid = Guid.NewGuid();
            _mockGarmentServiceSampleFabricWashRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleFabricWashReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleFabricWash>()
                {
                    new GarmentServiceSampleFabricWash(serviceSampleFabricWashGuid,null, DateTimeOffset.Now, "",false, 0, null, 0, 0)
                });

            Guid serviceSampleFabricWashItemGuid = Guid.NewGuid();
            _mockGarmentServiceSampleFabricWashItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleFabricWashItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleFabricWashItem>()
                {
                    new GarmentServiceSampleFabricWashItem(serviceSampleFabricWashItemGuid, serviceSampleFabricWashGuid, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null)
                });
            _mockServiceSampleFabricWashDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleFabricWashDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleFabricWashDetail>()
                {
                    new GarmentServiceSampleFabricWashDetail(new Guid(), serviceSampleFabricWashItemGuid, new ProductId(1), null, null, null, null, 1, new UomId(1), null)
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
            var unitUnderTest = CreateGarmentServiceSampleFabricWashController();
            Guid serviceSampleFabricWashGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSampleFabricWashCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleFabricWash(serviceSampleFabricWashGuid, null, DateTimeOffset.Now, "", false, 0, null, 0, 0));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSampleFabricWashCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleFabricWashController();
            Guid serviceSampleFabricWashGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSampleFabricWashCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSampleFabricWashCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleFabricWashController();
            Guid serviceSampleFabricWashGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentServiceSampleFabricWashCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleFabricWash(serviceSampleFabricWashGuid, null, DateTimeOffset.Now, "", false, 0, null, 0, 0));
            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentServiceSampleFabricWashCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleFabricWashController();
            Guid serviceSampleFabricWashGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentServiceSampleFabricWashCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleFabricWash(serviceSampleFabricWashGuid, null, DateTimeOffset.Now, "", false, 0, null, 0, 0));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task GetComplete_Return_Success()
        //{
        //    var unitUnderTest = CreateGarmentServiceSampleFabricWashController();
        //    Guid id = Guid.NewGuid();
        //    _mockGarmentServiceSampleFabricWashRepository
        //      .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //      .Returns(new List<GarmentServiceSampleFabricWashReadModel>().AsQueryable());


        //    _mockGarmentServiceSampleFabricWashRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleFabricWashReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleFabricWash>()
        //        {
        //            new GarmentServiceSampleFabricWash(id, null, DateTimeOffset.Now, "",false, 0, null, 0, 0)
        //        });

        //    GarmentServiceSampleFabricWashItem garmentServiceSampleFabricWashItem = new GarmentServiceSampleFabricWashItem(id, id, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null);
        //    GarmentServiceSampleFabricWashDetail garmentServiceSampleFabricWashDetail = new GarmentServiceSampleFabricWashDetail(new Guid(), new Guid(), new ProductId(1), null, null, null, "ColorD", 1, new UomId(1), null);

        //    _mockGarmentServiceSampleFabricWashItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleFabricWashItemReadModel>()
        //        {
        //            garmentServiceSampleFabricWashItem.GetReadModel()
        //        }.AsQueryable());

        //    _mockServiceSampleFabricWashDetailRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleFabricWashDetailReadModel>() {
        //            garmentServiceSampleFabricWashDetail.GetReadModel()
        //        }.AsQueryable());

        //    _mockGarmentServiceSampleFabricWashItemRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleFabricWashItemReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleFabricWashItem>()
        //        {
        //            new GarmentServiceSampleFabricWashItem(id, id,  null, DateTimeOffset.Now,new UnitSenderId(1),null,null, new UnitRequestId(1), null, null)
        //        });
        //    _mockServiceSampleFabricWashDetailRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleFabricWashDetailReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleFabricWashDetail>()
        //        {
        //            new GarmentServiceSampleFabricWashDetail(id, id, new ProductId(1), null, null, null, "ColorD", 1, new UomId(1), null)
        //        });

        //    // Act
        //    var orderData = new
        //    {
        //        ServiceSampleFabricWashDate = "desc",
        //    };

        //    string order = JsonConvert.SerializeObject(orderData);
        //    var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

        //    // Assert
        //    GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        //}

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentServiceSampleFabricWashController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsServiceSampleFabricWashQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXls(DateTime.Now, DateTime.Now, "token", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetXLS_Return_InternalServerError()
        {
            var unitUnderTest = CreateGarmentServiceSampleFabricWashController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsServiceSampleFabricWashQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXls(DateTime.Now, DateTime.Now, "token", 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }
    }
}
