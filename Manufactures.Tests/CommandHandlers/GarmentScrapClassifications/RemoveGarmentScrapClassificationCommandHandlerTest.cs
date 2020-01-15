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
	public class RemoveGarmentScrapClassificationCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentScrapClassificationRepository> _mockGarmentScrapClassificationRepository;

		public RemoveGarmentScrapClassificationCommandHandlerTest()
		{
			_mockGarmentScrapClassificationRepository = CreateMock<IGarmentScrapClassificationRepository>();
			_MockStorage.SetupStorage(_mockGarmentScrapClassificationRepository);
		}


		private RemoveGarmentScrapClassificationCommandHandler CreateRemoveGarmentScrapClassificationCommandHandler()
		{
			return new RemoveGarmentScrapClassificationCommandHandler(_MockStorage.Object);
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			RemoveGarmentScrapClassificationCommandHandler unitUnderTest = CreateRemoveGarmentScrapClassificationCommandHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid identity = Guid.NewGuid();

			RemoveGarmentScrapClassificationCommand removeGarmentAvalComponentCommand = new RemoveGarmentScrapClassificationCommand(avalComponentGuid);

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
			var result = await unitUnderTest.Handle(removeGarmentAvalComponentCommand, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
