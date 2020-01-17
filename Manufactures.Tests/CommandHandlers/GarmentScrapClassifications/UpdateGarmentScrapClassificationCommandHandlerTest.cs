using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapClassifications.CommandHandler;
using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.ReadModels;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentScrapClassifications
{
	public class UpdateGarmentScrapClassificationCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentScrapClassificationRepository> _mockGarmentScrapClassificationRepository;
		public UpdateGarmentScrapClassificationCommandHandlerTest()
		{
			_mockGarmentScrapClassificationRepository = CreateMock<IGarmentScrapClassificationRepository>();
			_MockStorage.SetupStorage(_mockGarmentScrapClassificationRepository);
		}


		private UpdateGarmentScrapClassificationCommandHandler CreateUpdateGarmentScrapClassificationCommandHandler()
		{
			return new UpdateGarmentScrapClassificationCommandHandler(_MockStorage.Object);
		}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			Guid identity = Guid.NewGuid();
			UpdateGarmentScrapClassificationCommandHandler unitUnderTest = CreateUpdateGarmentScrapClassificationCommandHandler();
			CancellationToken cancellationToken = CancellationToken.None;
			UpdateGarmentScrapClassificationCommand updateGarmentScrapClassificationCommand = new UpdateGarmentScrapClassificationCommand()
			{
				Code = "codes",
				Name="names",
				Description="desss"
			};
			updateGarmentScrapClassificationCommand.SetIdentity(identity);

			_mockGarmentScrapClassificationRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentScrapClassificationReadModel>()
				{
					new GarmentScrapClassificationReadModel(identity)
				}.AsQueryable());

			_MockStorage
				.Setup(x => x.Save())
				.Verifiable();

			// Act
			var result = await unitUnderTest.Handle(updateGarmentScrapClassificationCommand, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
