using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.ExcelTemplates;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
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
    public class GarmentServiceSampleShrinkpagePanelControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentServiceSampleShrinkagePanelRepository> _mockGarmentServiceSampleShrinkagePanelRepository;
        private Mock<IGarmentServiceSampleShrinkagePanelItemRepository> _mockGarmentServiceSampleShrinkagePanelItemRepository;
        private Mock<IGarmentServiceSampleShrinkagePanelDetailRepository> _mockServiceSampleShrinkagePanelDetailRepository;

        public GarmentServiceSampleShrinkpagePanelControllerTests() : base()
        {
            _mockGarmentServiceSampleShrinkagePanelRepository = CreateMock<IGarmentServiceSampleShrinkagePanelRepository>();
            _mockGarmentServiceSampleShrinkagePanelItemRepository = CreateMock<IGarmentServiceSampleShrinkagePanelItemRepository>();
            _mockServiceSampleShrinkagePanelDetailRepository = CreateMock<IGarmentServiceSampleShrinkagePanelDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentServiceSampleShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSampleShrinkagePanelItemRepository);
            _MockStorage.SetupStorage(_mockServiceSampleShrinkagePanelDetailRepository);
        }

        private GarmentServiceSampleShrinkagePanelController CreateGarmentServiceSampleShrinkagePanelController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentServiceSampleShrinkagePanelController controller = (GarmentServiceSampleShrinkagePanelController)Activator.CreateInstance(typeof(GarmentServiceSampleShrinkagePanelController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();

            _mockGarmentServiceSampleShrinkagePanelRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentServiceSampleShrinkagePanelReadModel>().AsQueryable());

            Guid serviceSampleShrinkagePanelGuid = Guid.NewGuid();
            _mockGarmentServiceSampleShrinkagePanelRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleShrinkagePanelReadModel>>()))
                .Returns(new List<GarmentServiceSampleShrinkagePanel>()
                {
                    new GarmentServiceSampleShrinkagePanel(serviceSampleShrinkagePanelGuid,null,  DateTimeOffset.Now,null, false, 0, null, 0, 0)
                });

            Guid serviceSampleShrinkagePanelItemGuid = Guid.NewGuid();
            GarmentServiceSampleShrinkagePanelItem garmentServiceSampleShrinkagePanelItem = new GarmentServiceSampleShrinkagePanelItem(serviceSampleShrinkagePanelItemGuid, serviceSampleShrinkagePanelGuid, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null);

            _mockGarmentServiceSampleShrinkagePanelItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSampleShrinkagePanelItemReadModel>()
                {
                    garmentServiceSampleShrinkagePanelItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();
            Guid serviceSampleShrinkagePanelGuid = Guid.NewGuid();
            _mockGarmentServiceSampleShrinkagePanelRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleShrinkagePanelReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleShrinkagePanel>()
                {
                    new GarmentServiceSampleShrinkagePanel(serviceSampleShrinkagePanelGuid,null, DateTimeOffset.Now, null,false, 0, null, 0, 0)
                });

            Guid serviceSampleShrinkagePanelItemGuid = Guid.NewGuid();
            _mockGarmentServiceSampleShrinkagePanelItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleShrinkagePanelItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleShrinkagePanelItem>()
                {
                    new GarmentServiceSampleShrinkagePanelItem(serviceSampleShrinkagePanelItemGuid, serviceSampleShrinkagePanelGuid, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null)
                });
            _mockServiceSampleShrinkagePanelDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSampleShrinkagePanelDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSampleShrinkagePanelDetail>()
                {
                    new GarmentServiceSampleShrinkagePanelDetail(new Guid(), serviceSampleShrinkagePanelItemGuid, new ProductId(1), null, null, null, null, 1, new UomId(1), null)
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
            var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();
            Guid serviceSampleShrinkagePanelGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSampleShrinkagePanelCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleShrinkagePanel(serviceSampleShrinkagePanelGuid, null, DateTimeOffset.Now,null, false, 0, null, 0, 0));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSampleShrinkagePanelCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();
            Guid serviceSampleShrinkagePanelGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSampleShrinkagePanelCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSampleShrinkagePanelCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();
            Guid serviceSampleShrinkagePanelGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentServiceSampleShrinkagePanelCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleShrinkagePanel(serviceSampleShrinkagePanelGuid, null, DateTimeOffset.Now,null, false, 0, null, 0, 0));
            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentServiceSampleShrinkagePanelCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();
            Guid serviceSampleShrinkagePanelGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentServiceSampleShrinkagePanelCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSampleShrinkagePanel(serviceSampleShrinkagePanelGuid, null, DateTimeOffset.Now,null, false, 0, null,0, 0));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task GetComplete_Return_Success()
        //{
        //    var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();
        //    Guid id = Guid.NewGuid();
        //    _mockGarmentServiceSampleShrinkagePanelRepository
        //      .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //      .Returns(new List<GarmentServiceSampleShrinkagePanelReadModel>().AsQueryable());

        //    _mockGarmentServiceSampleShrinkagePanelRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleShrinkagePanelReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleShrinkagePanel>()
        //        {
        //            new GarmentServiceSampleShrinkagePanel(id, null, DateTimeOffset.Now,null, false, 0, null, 0, 0)
        //        });

        //    GarmentServiceSampleShrinkagePanelItem garmentServiceSampleShrinkagePanelItem = new GarmentServiceSampleShrinkagePanelItem(id, id, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null);
        //    GarmentServiceSampleShrinkagePanelDetail garmentServiceSampleShrinkagePanelDetail = new GarmentServiceSampleShrinkagePanelDetail(new Guid(), new Guid(), new ProductId(1), null, null, null, "ColorD", 1, new UomId(1), null);
            
        //    _mockGarmentServiceSampleShrinkagePanelItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleShrinkagePanelItemReadModel>()
        //        {
        //            garmentServiceSampleShrinkagePanelItem.GetReadModel()
        //        }.AsQueryable());

        //    _mockServiceSampleShrinkagePanelDetailRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSampleShrinkagePanelDetailReadModel>() {
        //            garmentServiceSampleShrinkagePanelDetail.GetReadModel()
        //        }.AsQueryable());

        //    _mockGarmentServiceSampleShrinkagePanelItemRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleShrinkagePanelItemReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleShrinkagePanelItem>()
        //        {
        //            new GarmentServiceSampleShrinkagePanelItem(id, id,  null, DateTimeOffset.Now,new UnitSenderId(1),null,null, new UnitRequestId(1), null, null)
        //        });
        //    _mockServiceSampleShrinkagePanelDetailRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSampleShrinkagePanelDetailReadModel>>()))
        //        .Returns(new List<GarmentServiceSampleShrinkagePanelDetail>()
        //        {
        //            new GarmentServiceSampleShrinkagePanelDetail(id, id, new ProductId(1), null, null, null, "ColorD", 1, new UomId(1), null)
        //        });

        //    // Act
        //    var orderData = new
        //    {
        //        ServiceSampleShrinkagePanelDate = "desc",
        //    };

        //    string order = JsonConvert.SerializeObject(orderData);
        //    var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

        //    // Assert
        //    GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        //}

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleServiceSampleShrinkagePanelsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXls(DateTime.Now, DateTime.Now, "token");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetXLS_Return_InternalServerError()
        {
            var unitUnderTest = CreateGarmentServiceSampleShrinkagePanelController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleServiceSampleShrinkagePanelsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXls(DateTime.Now, DateTime.Now, "token");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }
    }
}
