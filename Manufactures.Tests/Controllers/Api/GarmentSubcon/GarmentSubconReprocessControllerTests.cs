using Barebone.Tests;
using FluentAssertions;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
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

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentSubconReprocessControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSubconReprocessRepository> _mockGarmentSubconReprocessRepository;
        private Mock<IGarmentSubconReprocessItemRepository> _mockGarmentSubconReprocessItemRepository;
        private Mock<IGarmentSubconReprocessDetailRepository> _mockSubconReprocessDetailRepository;

        public GarmentSubconReprocessControllerTests() : base()
        {
            _mockGarmentSubconReprocessRepository = CreateMock<IGarmentSubconReprocessRepository>();
            _mockGarmentSubconReprocessItemRepository = CreateMock<IGarmentSubconReprocessItemRepository>();
            _mockSubconReprocessDetailRepository = CreateMock<IGarmentSubconReprocessDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentSubconReprocessRepository);
            _MockStorage.SetupStorage(_mockGarmentSubconReprocessItemRepository);
            _MockStorage.SetupStorage(_mockSubconReprocessDetailRepository);
        }

        private GarmentSubconReprocessController CreateGarmentSubconReprocessController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconReprocessController controller = (GarmentSubconReprocessController)Activator.CreateInstance(typeof(GarmentSubconReprocessController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSubconReprocessController();

            _mockGarmentSubconReprocessRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconReprocessReadModel>().AsQueryable());

            Guid SubconReprocessGuid = Guid.NewGuid();
            _mockGarmentSubconReprocessRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconReprocessReadModel>>()))
                .Returns(new List<GarmentSubconReprocess>()
                {
                    new GarmentSubconReprocess(SubconReprocessGuid,"no", "SUBCON JASA KOMPONEN", DateTimeOffset.Now)
                });
            //, new UnitDepartmentId(1),null,null
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid SubconReprocessItemGuid = Guid.NewGuid();
            GarmentSubconReprocessItem garmentSubconReprocessItem = new GarmentSubconReprocessItem(SubconReprocessItemGuid, SubconReprocessGuid, Guid.NewGuid(), "no", Guid.NewGuid(), Guid.NewGuid(), "no", Guid.NewGuid(),"ro","art", new GarmentComodityId(1), null, null, new BuyerId(1), null, null);

            //_mockGarmentSubconReprocessItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSubconReprocessItemReadModel>()
            //    {
            //        garmentSubconReprocessItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentSubconReprocessController();
            Guid SubconReprocessGuid = Guid.NewGuid();
            _mockGarmentSubconReprocessRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconReprocessReadModel, bool>>>()))
                .Returns(new List<GarmentSubconReprocess>()
                {
                    new GarmentSubconReprocess(SubconReprocessGuid,"no", "SUBCON JASA KOMPONEN", DateTimeOffset.Now)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid SubconReprocessItemGuid = Guid.NewGuid();
            _mockGarmentSubconReprocessItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconReprocessItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconReprocessItem>()
                {
                    new GarmentSubconReprocessItem(SubconReprocessItemGuid, SubconReprocessGuid, Guid.NewGuid(), "no", Guid.NewGuid(), Guid.NewGuid(), "no", Guid.NewGuid(),"ro","art", new GarmentComodityId(1), null, null, new BuyerId(1), null, null)
                });
            _mockSubconReprocessDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconReprocessDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSubconReprocessDetail>()
                {
                    new GarmentSubconReprocessDetail(SubconReprocessItemGuid, SubconReprocessItemGuid, new SizeId(1),null,1,1, new UomId(1), null, "color",  new Guid(), new Guid(), new Guid(), "ColorD", new UnitDepartmentId(1), null, null, null)
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
            var unitUnderTest = CreateGarmentSubconReprocessController();
            Guid SubconReprocessGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconReprocessCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconReprocess(SubconReprocessGuid, "no", "SUBCON JASA KOMPONEN", DateTimeOffset.Now));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSubconReprocessCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconReprocessController();
            Guid SubconReprocessGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconReprocessCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconReprocessCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconReprocessController();
            Guid SubconReprocessGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSubconReprocessCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconReprocess(SubconReprocessGuid, "no", "SUBCON JASA KOMPONEN", DateTimeOffset.Now));
            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSubconReprocessCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconReprocessController();
            Guid SubconReprocessGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconReprocessCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconReprocess(SubconReprocessGuid, "no", "SUBCON JASA KOMPONEN", DateTimeOffset.Now));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            var unitUnderTest = CreateGarmentSubconReprocessController();
            Guid SubconReprocessGuid = Guid.NewGuid();
            Guid SubconReprocessItemGuid = Guid.NewGuid();
            _mockGarmentSubconReprocessRepository
              .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new List<GarmentSubconReprocessReadModel>().AsQueryable());


            _mockGarmentSubconReprocessRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconReprocessReadModel>>()))
                .Returns(new List<GarmentSubconReprocess>()
                {
                    new GarmentSubconReprocess(SubconReprocessGuid,"no", "SUBCON JASA KOMPONEN", DateTimeOffset.Now)
                });

            GarmentSubconReprocessItem garmentSubconReprocessItem = new GarmentSubconReprocessItem(SubconReprocessItemGuid, SubconReprocessGuid, Guid.NewGuid(), "no", Guid.NewGuid(), Guid.NewGuid(), "no", Guid.NewGuid(), "ro", "art", new GarmentComodityId(1), null, null, new BuyerId(1), null, null);
            GarmentSubconReprocessDetail garmentSubconReprocessDetail = new GarmentSubconReprocessDetail(SubconReprocessItemGuid, SubconReprocessItemGuid, new SizeId(1), null, 1, 1, new UomId(1), null, "color", new Guid(), new Guid(), new Guid(), "ColorD", new UnitDepartmentId(1), null, null, null);
            //id, id, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1),
            _mockGarmentSubconReprocessItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconReprocessItemReadModel>()
                {
                    garmentSubconReprocessItem.GetReadModel()
                }.AsQueryable());

            _mockSubconReprocessDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconReprocessDetailReadModel>() {
                    garmentSubconReprocessDetail.GetReadModel()
                }.AsQueryable());

            _mockGarmentSubconReprocessItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconReprocessItemReadModel>>()))
                .Returns(new List<GarmentSubconReprocessItem>()
                {
                    new GarmentSubconReprocessItem(SubconReprocessItemGuid, SubconReprocessGuid, Guid.NewGuid(), "no", Guid.NewGuid(), Guid.NewGuid(), "no", Guid.NewGuid(),"ro","art", new GarmentComodityId(1), null, null, new BuyerId(1), null, null)
                });
            _mockSubconReprocessDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconReprocessDetailReadModel>>()))
                .Returns(new List<GarmentSubconReprocessDetail>()
                {
                    new GarmentSubconReprocessDetail(SubconReprocessItemGuid, SubconReprocessItemGuid, new SizeId(1),null,1,1, new UomId(1), null, "color",  new Guid(), new Guid(), new Guid(), "ColorD", new UnitDepartmentId(1), null, null, null)
                });

            // Act
            var orderData = new
            {
                ReprocessNo = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetItem_Return_Success()
        {
            var unitUnderTest = CreateGarmentSubconReprocessController();
            Guid id = Guid.NewGuid();
            Guid SubconReprocessItemGuid = Guid.NewGuid();
            Guid SubconReprocessGuid = Guid.NewGuid();
            _mockGarmentSubconReprocessItemRepository
              .Setup(s => s.ReadItem(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new List<GarmentSubconReprocessItemReadModel>().AsQueryable());
            GarmentSubconReprocessItem garmentSubconReprocessItem = new GarmentSubconReprocessItem(SubconReprocessItemGuid, SubconReprocessGuid, Guid.NewGuid(), "no", Guid.NewGuid(), Guid.NewGuid(), "no", Guid.NewGuid(), "ro", "art", new GarmentComodityId(1), null, null, new BuyerId(1), null, null);
            GarmentSubconReprocessDetail garmentSubconReprocessDetail = new GarmentSubconReprocessDetail(SubconReprocessItemGuid, SubconReprocessItemGuid, new SizeId(1), null, 1, 1, new UomId(1), null, "color", new Guid(), new Guid(), new Guid(), "ColorD", new UnitDepartmentId(1), null, null, null);
            //id, id, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1),
            //_mockGarmentSubconReprocessItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSubconReprocessItemReadModel>()
            //    {
            //        garmentSubconReprocessItem.GetReadModel()
            //    }.AsQueryable());

            _mockSubconReprocessDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconReprocessDetailReadModel>() {
                    garmentSubconReprocessDetail.GetReadModel()
                }.AsQueryable());

            _mockGarmentSubconReprocessItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconReprocessItemReadModel>>()))
                .Returns(new List<GarmentSubconReprocessItem>()
                {
                    new GarmentSubconReprocessItem(SubconReprocessItemGuid, SubconReprocessGuid, Guid.NewGuid(), "no", Guid.NewGuid(), Guid.NewGuid(), "no", Guid.NewGuid(),"ro","art", new GarmentComodityId(1), null, null, new BuyerId(1), null, null)
                });
            _mockSubconReprocessDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconReprocessDetailReadModel>>()))
                .Returns(new List<GarmentSubconReprocessDetail>()
                {
                    new GarmentSubconReprocessDetail(SubconReprocessItemGuid, SubconReprocessItemGuid, new SizeId(1),null,1,1, new UomId(1), null, "color",  new Guid(), new Guid(), new Guid(), "ColorD", new UnitDepartmentId(1), null, null, null)
                });

            // Act
            var orderData = new
            {
                article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetItems(1, 25, order, new List<string>(), "", "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPDF_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconReprocessController();
            Guid SubconReprocessGuid = Guid.NewGuid();
            _mockGarmentSubconReprocessRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconReprocessReadModel, bool>>>()))
                .Returns(new List<GarmentSubconReprocess>()
                {
                    new GarmentSubconReprocess(SubconReprocessGuid,"no", "SUBCON JASA KOMPONEN", DateTimeOffset.Now)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid SubconReprocessItemGuid = Guid.NewGuid();
            _mockGarmentSubconReprocessItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconReprocessItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconReprocessItem>()
                {
                    new GarmentSubconReprocessItem(SubconReprocessItemGuid, SubconReprocessGuid, Guid.NewGuid(), "no", Guid.NewGuid(), Guid.NewGuid(), "no", Guid.NewGuid(),"ro","art", new GarmentComodityId(1), null, null, new BuyerId(1), null, null)
                });
            _mockSubconReprocessDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconReprocessDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSubconReprocessDetail>()
                {
                    new GarmentSubconReprocessDetail(SubconReprocessItemGuid, SubconReprocessItemGuid, new SizeId(1),null,1,1, new UomId(1), null, "color",  new Guid(), new Guid(), new Guid(), "ColorD", new UnitDepartmentId(1), null, null, null)
                });

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString());

            // Assert
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }
    }
}