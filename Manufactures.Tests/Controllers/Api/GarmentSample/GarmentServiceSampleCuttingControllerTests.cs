using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using System.Net;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using System.Threading;
using Newtonsoft.Json;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.Queries;
using System.IO;

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentServiceSampleCuttingControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentServiceSampleCuttingRepository> _mockGarmentServiceSampleCuttingRepository;
        private Mock<IGarmentServiceSampleCuttingItemRepository> _mockGarmentServiceSampleCuttingItemRepository;
        private Mock<IGarmentServiceSampleCuttingDetailRepository> _mockGarmentServiceSampleCuttingDetailRepository;
        private Mock<IGarmentServiceSampleCuttingSizeRepository> _mockGarmentServiceSampleCuttingSizeRepository;

        public GarmentServiceSampleCuttingControllerTests() : base()
        {
            _mockGarmentServiceSampleCuttingRepository = CreateMock<IGarmentServiceSampleCuttingRepository>();
            _mockGarmentServiceSampleCuttingItemRepository = CreateMock<IGarmentServiceSampleCuttingItemRepository>();
            _mockGarmentServiceSampleCuttingDetailRepository = CreateMock<IGarmentServiceSampleCuttingDetailRepository>();
            _mockGarmentServiceSampleCuttingSizeRepository= CreateMock<IGarmentServiceSampleCuttingSizeRepository>();

            _MockStorage.SetupStorage(_mockGarmentServiceSampleCuttingRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSampleCuttingItemRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSampleCuttingDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSampleCuttingSizeRepository);

        }

        private GarmentServiceSampleCuttingController CreateGarmentServiceSampleCuttingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentServiceSampleCuttingController controller = (GarmentServiceSampleCuttingController)Activator.CreateInstance(typeof(GarmentServiceSampleCuttingController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _mockGarmentServiceSampleCuttingRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentServiceSampleCuttingReadModel>().AsQueryable());

            Guid ServiceSampleCuttingGuid = Guid.NewGuid();
            _mockGarmentServiceSampleCuttingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleCuttingReadModel>>()))
                .Returns(new List<GarmentServiceSampleCutting>()
                {
                    new GarmentServiceSampleCutting(ServiceSampleCuttingGuid, null, null, new UnitDepartmentId(1), null, null,  DateTimeOffset.Now, false, new BuyerId(1), null, null, new UomId(1), null, 0, 0, 0, null)
                });

            //Guid ServiceSampleCuttingItemGuid = Guid.NewGuid();
            //GarmentServiceSampleCuttingItem garmentServiceSampleCuttingItem = new GarmentServiceSampleCuttingItem(Guid.Empty, ServiceSampleCuttingGuid, null, null, new GarmentComodityId(1), null, null);
            //_mockGarmentServiceSampleCuttingItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentServiceSampleCuttingItemReadModel>()
            //    {
            //        garmentServiceSampleCuttingItem.GetReadModel()
            //    }.AsQueryable());

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _mockGarmentServiceSampleCuttingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleCuttingReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleCutting>()
                {
                    new GarmentServiceSampleCutting(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, new UomId(1), null, 0, 0, 0, null)
                });

            _mockGarmentServiceSampleCuttingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleCuttingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleCuttingItem>()
                {
                    new GarmentServiceSampleCuttingItem(Guid.NewGuid(), Guid.NewGuid(),null,null,new GarmentComodityId(1),null,null)
                });

            _mockGarmentServiceSampleCuttingDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleCuttingDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleCuttingDetail>()
                {
                    new GarmentServiceSampleCuttingDetail(Guid.NewGuid(), Guid.NewGuid(),"",1)
                });

            _mockGarmentServiceSampleCuttingSizeRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleCuttingSizeReadModel, bool> >> ()))
                .Returns(new List<GarmentServiceSampleCuttingSize>()
                {
                    new GarmentServiceSampleCuttingSize(Guid.NewGuid(), new SizeId(1),null,1,new UomId(1),null,null,Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),new ProductId(1),null,null)
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
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSampleCuttingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleCutting(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, new UomId(1), null, 1, 1, 1, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSampleCuttingCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSampleCuttingCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSampleCuttingCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentServiceSampleCuttingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleCutting(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, new UomId(1), null, 0, 0, 0, null));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentServiceSampleCuttingCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {

            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentServiceSampleCuttingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleCutting(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, new UomId(1), null, 0, 0, 0, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        

        [Fact]
        public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid ServiceSampleCuttingItemGuid = Guid.NewGuid();
            Guid ServiceSampleCuttingGuid = Guid.NewGuid();
            Guid SampleCuttingDetailGuid = Guid.NewGuid();
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _mockGarmentServiceSampleCuttingRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentServiceSampleCuttingReadModel>() { new GarmentServiceSampleCuttingReadModel(ServiceSampleCuttingGuid) }
                .AsQueryable());

            _mockGarmentServiceSampleCuttingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleCuttingReadModel>>()))
                .Returns(new List<GarmentServiceSampleCutting>()
                {
                    new GarmentServiceSampleCutting(ServiceSampleCuttingGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, new UomId(1), null, 0, 0, 0, null)
                });

            _mockGarmentServiceSampleCuttingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleCuttingItemReadModel>()
                {
                    new GarmentServiceSampleCuttingItemReadModel(ServiceSampleCuttingItemGuid)
                }.AsQueryable());

            _mockGarmentServiceSampleCuttingItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleCuttingItemReadModel>>()))
                .Returns(new List<GarmentServiceSampleCuttingItem>()
                {
                    new GarmentServiceSampleCuttingItem(ServiceSampleCuttingItemGuid, ServiceSampleCuttingGuid,null,null,new GarmentComodityId(1),null,null)
                });

            _mockGarmentServiceSampleCuttingDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleCuttingDetailReadModel>()
                {
                    new GarmentServiceSampleCuttingDetailReadModel(SampleCuttingDetailGuid)
                }.AsQueryable());

            _mockGarmentServiceSampleCuttingDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleCuttingDetailReadModel>>()))
                .Returns(new List<GarmentServiceSampleCuttingDetail>()
                {
                    new GarmentServiceSampleCuttingDetail(SampleCuttingDetailGuid, ServiceSampleCuttingItemGuid,"",1)
                });

            _mockGarmentServiceSampleCuttingSizeRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleCuttingSizeReadModel>()
                {
                    new GarmentServiceSampleCuttingSizeReadModel(Guid.NewGuid())
                }.AsQueryable());

            _mockGarmentServiceSampleCuttingSizeRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleCuttingSizeReadModel>>()))
                .Returns(new List<GarmentServiceSampleCuttingSize>()
                {
                    new GarmentServiceSampleCuttingSize(Guid.NewGuid(), new SizeId(1),null,1,new UomId(1),null,null,SampleCuttingDetailGuid,Guid.NewGuid(),Guid.NewGuid(),new ProductId(1),null,null)
                });

            // Act
            var orderData = new
            {
                SampleNo = "desc",
            };

            string oder = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, oder, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task GetItem_Return_Success()
        //{
        //    Guid ServiceSampleCuttingItemGuid = Guid.NewGuid();
        //    Guid ServiceSampleCuttingGuid = Guid.NewGuid();
        //    Guid SampleCuttingDetailGuid = Guid.NewGuid();
        //    var unitUnderTest = CreateGarmentServiceSampleCuttingController();

        //    _mockGarmentServiceSampleCuttingItemRepository
        //      .Setup(s => s.ReadItem(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //      .Returns(new List<GarmentServiceSampleCuttingItemReadModel>().AsQueryable());
        //    _mockGarmentServiceSampleCuttingItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleCuttingItemReadModel>()
        //        {
        //            new GarmentServiceSampleCuttingItemReadModel(ServiceSampleCuttingItemGuid)
        //        }.AsQueryable());

        //    _mockGarmentServiceSampleCuttingItemRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleCuttingItemReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleCuttingItem>()
        //        {
        //            new GarmentServiceSampleCuttingItem(ServiceSampleCuttingItemGuid, ServiceSampleCuttingGuid,null,null,new GarmentComodityId(1),null,null)
        //        });

        //    _mockGarmentServiceSampleCuttingDetailRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleCuttingDetailReadModel>()
        //        {
        //            new GarmentServiceSampleCuttingDetailReadModel(SampleCuttingDetailGuid)
        //        }.AsQueryable());

        //    _mockGarmentServiceSampleCuttingDetailRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleCuttingDetailReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleCuttingDetail>()
        //        {
        //            new GarmentServiceSampleCuttingDetail(SampleCuttingDetailGuid, ServiceSampleCuttingItemGuid,"",1)
        //        });

        //    _mockGarmentServiceSampleCuttingSizeRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleCuttingSizeReadModel>()
        //        {
        //            new GarmentServiceSampleCuttingSizeReadModel(Guid.NewGuid())
        //        }.AsQueryable());

        //    _mockGarmentServiceSampleCuttingSizeRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleCuttingSizeReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleCuttingSize>()
        //        {
        //            new GarmentServiceSampleCuttingSize(Guid.NewGuid(), new SizeId(1),null,1,new UomId(1),null,null,SampleCuttingDetailGuid,Guid.NewGuid(),Guid.NewGuid(),new ProductId(1),null,null)
        //        });

        //    // Act
        //    var orderData = new
        //    {
        //        Id = "desc",
        //    };

        //    string order = JsonConvert.SerializeObject(orderData);
        //    var result = await unitUnderTest.GetItems(1, 25, order, new List<string>(), "", "{}");

        //    // Assert
        //    GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        //}
        //
        [Fact]
        public async Task GetXLSampleCuttingBehavior()
        {
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsServiceSampleCuttingQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act

            var result = await unitUnderTest.GetXlsMutation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLSSampleCuttingReturn_InternalServerError()
        {
            var unitUnderTest = CreateGarmentServiceSampleCuttingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsServiceSampleCuttingQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act

            var result = await unitUnderTest.GetXlsMutation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }
    }
}
