﻿using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingIns.CommandHandlers;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSewingIns
{
    public class PlaceGarmentSewingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingInRepository> _mockSewingInRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;

        public PlaceGarmentSewingInCommandHandlerTests()
        {
            _mockSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
        }

        private PlaceGarmentSewingInCommandHandler CreatePlaceGarmentSewingInCommandHandler()
        {
            return new PlaceGarmentSewingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            PlaceGarmentSewingInCommandHandler unitUnderTest = CreatePlaceGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSewingInCommand placeGarmentSewingInCommand = new PlaceGarmentSewingInCommand()
            {

                RONo = "RONo",
                Article = "Article",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                LoadingId = Guid.NewGuid(),
                LoadingNo = "LoadingNo",
                SewingInDate = DateTimeOffset.Now,
                Comodity = new GarmentComodity(1, "ComodityCode", "ComodityName"),
                Items = new List<GarmentSewingInItemValueObject>
                {
                    new GarmentSewingInItemValueObject
                    {
                        LoadingItemId = Guid.NewGuid(),
                        Uom = new Uom(1, "UomUnit"),
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Size = new SizeValueObject(1,"SizeName"),
                        DesignColor = "DesignColor",
                        Color = "Color",
                        Quantity = 1,
                        RemainingQuantity = 1,
                    }
                },

            };

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>().AsQueryable());


            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}