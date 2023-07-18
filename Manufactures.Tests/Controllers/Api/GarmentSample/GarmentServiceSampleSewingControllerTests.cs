using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.Queries.GarmentSampleGarmentWashReport;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentServiceSampleSewingControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentServiceSampleSewingRepository> _mockGarmentServiceSampleSewingRepository;
        private Mock<IGarmentServiceSampleSewingItemRepository> _mockGarmentServiceSampleSewingItemRepository;
        private Mock<IGarmentServiceSampleSewingDetailRepository> _mockServiceSampleSewingDetailRepository;

        public GarmentServiceSampleSewingControllerTests() : base()
        {
            _mockGarmentServiceSampleSewingRepository = CreateMock<IGarmentServiceSampleSewingRepository>();
            _mockGarmentServiceSampleSewingItemRepository = CreateMock<IGarmentServiceSampleSewingItemRepository>();
            _mockServiceSampleSewingDetailRepository = CreateMock<IGarmentServiceSampleSewingDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentServiceSampleSewingRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSampleSewingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleSewingDetailRepository);
        }

        private GarmentServiceSampleSewingController CreateGarmentServiceSampleSewingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentServiceSampleSewingController controller = (GarmentServiceSampleSewingController)Activator.CreateInstance(typeof(GarmentServiceSampleSewingController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentServiceSampleSewingController();

            _mockGarmentServiceSampleSewingRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentServiceSampleSewingReadModel>().AsQueryable());

            Guid serviceSampleSewingGuid = Guid.NewGuid();
            _mockGarmentServiceSampleSewingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleSewingReadModel>>()))
                .Returns(new List<GarmentServiceSampleSewing>()
                {
                    new GarmentServiceSampleSewing(serviceSampleSewingGuid,null,  DateTimeOffset.Now, false, new BuyerId(1), null, null,0,null, 0, 0)
                });
            //, new UnitDepartmentId(1),null,null
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid serviceSampleSewingItemGuid = Guid.NewGuid();
            GarmentServiceSampleSewingItem garmentServiceSampleSewingItem = new GarmentServiceSampleSewingItem(serviceSampleSewingItemGuid, serviceSampleSewingGuid, null, null, new GarmentComodityId(1), null, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null);

            _mockGarmentServiceSampleSewingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleSewingItemReadModel>()
                {
                    garmentServiceSampleSewingItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentServiceSampleSewingController();
            Guid serviceSampleSewingGuid = Guid.NewGuid();
            _mockGarmentServiceSampleSewingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleSewingReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleSewing>()
                {
                    new GarmentServiceSampleSewing(serviceSampleSewingGuid,null, DateTimeOffset.Now, false, new BuyerId(1), null, null,0,null, 0, 0)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid serviceSampleSewingItemGuid = Guid.NewGuid();
            _mockGarmentServiceSampleSewingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleSewingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleSewingItem>()
                {
                    new GarmentServiceSampleSewingItem(serviceSampleSewingItemGuid, serviceSampleSewingGuid, null, null,new GarmentComodityId(1),null,null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null)
                });
            _mockServiceSampleSewingDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleSewingDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleSewingDetail>()
                {
                    new GarmentServiceSampleSewingDetail(serviceSampleSewingItemGuid, serviceSampleSewingItemGuid,  new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null)
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
            var unitUnderTest = CreateGarmentServiceSampleSewingController();
            Guid serviceSampleSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSampleSewingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleSewing(serviceSampleSewingGuid, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, 0, null, 0, 0));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSampleSewingCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleSewingController();
            Guid serviceSampleSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSampleSewingCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSampleSewingCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleSewingController();
            Guid serviceSampleSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentServiceSampleSewingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleSewing(serviceSampleSewingGuid, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, 0, null, 0, 0));
            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentServiceSampleSewingCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleSewingController();
            Guid serviceSampleSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentServiceSampleSewingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleSewing(serviceSampleSewingGuid, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, 0, null, 0, 0));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task GetComplete_Return_Success()
        //{
        //    var unitUnderTest = CreateGarmentServiceSampleSewingController();
        //    Guid id = Guid.NewGuid();

        //    GarmentServiceSampleSewing garmentServiceSampleSewing = new GarmentServiceSampleSewing(id, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, 0, null, 0, 0);
        //    GarmentServiceSampleSewingItem garmentServiceSampleSewingItem = new GarmentServiceSampleSewingItem(id, id,  null, null,new GarmentComodityId(1),null, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null);
        //    GarmentServiceSampleSewingDetail garmentServiceSampleSewingDetail = new GarmentServiceSampleSewingDetail(new Guid(), new Guid(), new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null);
        //    //id, id, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1),
            
        //     _mockGarmentServiceSampleSewingRepository
        //      .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //      .Returns(new List<GarmentServiceSampleSewingReadModel>().AsQueryable());

        //    _mockGarmentServiceSampleSewingRepository
        //      .Setup(s => s.Query)
        //      .Returns(new List<GarmentServiceSampleSewingReadModel>()
        //      {
        //          garmentServiceSampleSewing.GetReadModel()
        //      }.AsQueryable());

        //    _mockGarmentServiceSampleSewingRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleSewingReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleSewing>()
        //        {
        //            new GarmentServiceSampleSewing(id, null, DateTimeOffset.Now, false, new BuyerId(1), null, null,0,null, 0, 0)
        //        });

        //    _mockGarmentServiceSampleSewingItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleSewingItemReadModel>()
        //        {
        //            garmentServiceSampleSewingItem.GetReadModel()
        //        }.AsQueryable());

        //    _mockServiceSampleSewingDetailRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleSewingDetailReadModel>() {
        //            garmentServiceSampleSewingDetail.GetReadModel()
        //        }.AsQueryable());

        //    _mockGarmentServiceSampleSewingItemRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleSewingItemReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleSewingItem>()
        //        {
        //            new GarmentServiceSampleSewingItem(id, id,  null, null,new GarmentComodityId(1),null,null, new BuyerId(1), null, null,  new UnitDepartmentId(1), null, null)
        //        });

        //    _mockGarmentServiceSampleSewingItemRepository
        //        .Setup(s => s.ReadItem(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(new List<GarmentServiceSampleSewingItemReadModel>().AsQueryable());

        //    _mockServiceSampleSewingDetailRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleSewingDetailReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleSewingDetail>()
        //        {
        //            new GarmentServiceSampleSewingDetail(id, id,  new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null)
        //        });

        //    // Act
        //    var orderData = new
        //    {
        //        article = "desc",
        //    };

        //    string order = JsonConvert.SerializeObject(orderData);
        //    var result = await unitUnderTest.GetComplete(1, 25, order, "", "{}");

        //    // Assert
        //    GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        //}

        //[Fact]
        //public async Task GetItem_Return_Success()
        //{
        //    var unitUnderTest = CreateGarmentServiceSampleSewingController();
        //    Guid id = Guid.NewGuid();
        //    _mockGarmentServiceSampleSewingItemRepository
        //      .Setup(s => s.ReadItem(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //      .Returns(new List<GarmentServiceSampleSewingItemReadModel>().AsQueryable());
        //    GarmentServiceSampleSewingItem garmentServiceSampleSewingItem = new GarmentServiceSampleSewingItem(id, id, null, null, new GarmentComodityId(1), null, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null);
        //    GarmentServiceSampleSewingDetail garmentServiceSampleSewingDetail = new GarmentServiceSampleSewingDetail(new Guid(), new Guid(), new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null);
        //    //id, id, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1),
        //    _mockGarmentServiceSampleSewingItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleSewingItemReadModel>()
        //        {
        //            garmentServiceSampleSewingItem.GetReadModel()
        //        }.AsQueryable());

        //    _mockServiceSampleSewingDetailRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleSewingDetailReadModel>() {
        //            garmentServiceSampleSewingDetail.GetReadModel()
        //        }.AsQueryable());

        //    _mockGarmentServiceSampleSewingItemRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleSewingItemReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleSewingItem>()
        //        {
        //            new GarmentServiceSampleSewingItem(id, id,  null, null,new GarmentComodityId(1),null,null, new BuyerId(1), null, null,  new UnitDepartmentId(1), null, null)
        //        });
        //    _mockServiceSampleSewingDetailRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleSewingDetailReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleSewingDetail>()
        //        {
        //            new GarmentServiceSampleSewingDetail(id, id,  new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null)
        //        });

        //    // Act
        //    var orderData = new
        //    {
        //        article = "desc",
        //    };

        //    string order = JsonConvert.SerializeObject(orderData);
        //    var result = await unitUnderTest.GetItems(1, 25, order, "", "{}");

        //    // Assert
        //    GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        //}
        //
        [Fact]
        public async Task GetXLSampleSewingBehavior()
        {
            var unitUnderTest = CreateGarmentServiceSampleSewingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSampleGarmentWashReporQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act

            var result = await unitUnderTest.GetXlsSampleGarmentWashReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLSSampleSewingReturn_InternalServerError()
        {
            var unitUnderTest = CreateGarmentServiceSampleSewingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSampleGarmentWashReporQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act

            var result = await unitUnderTest.GetXlsSampleGarmentWashReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }
    }
}
