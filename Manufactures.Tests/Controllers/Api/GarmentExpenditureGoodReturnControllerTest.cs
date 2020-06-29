using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentExpenditureGoodReturnControllerTest : BaseControllerUnitTest
    {

        private readonly Mock<IGarmentExpenditureGoodReturnRepository> _mockGarmentExpenditureGoodReturnRepository;
        private readonly Mock<IGarmentExpenditureGoodReturnItemRepository> _mockGarmentExpenditureGoodReturnItemRepository;

        public GarmentExpenditureGoodReturnControllerTest() : base()
        {
            _mockGarmentExpenditureGoodReturnRepository = CreateMock<IGarmentExpenditureGoodReturnRepository>();
            _mockGarmentExpenditureGoodReturnItemRepository = CreateMock<IGarmentExpenditureGoodReturnItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentExpenditureGoodReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentExpenditureGoodReturnItemRepository);
        }

        private GarmentExpenditureGoodReturnController CreateGarmentExpenditureGoodReturnController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentExpenditureGoodReturnController controller = (GarmentExpenditureGoodReturnController)Activator.CreateInstance(typeof(GarmentExpenditureGoodReturnController), _MockServiceProvider.Object);
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
    }
}
