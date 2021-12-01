using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleRequest.CommandHandler;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleRequest
{
    public class PostGarmentSampleRequestCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleRequestRepository> _mockSampleRequestRepository;

        public PostGarmentSampleRequestCommandHandlerTest()
        {
            _mockSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();

            _MockStorage.SetupStorage(_mockSampleRequestRepository);
        }
        private PostGarmentSampleRequestCommandHandler CreatePostGarmentSampleRequestCommandHandler()
        {
            return new PostGarmentSampleRequestCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SampleRequestGuid = Guid.NewGuid();
            List<string> SampleRequestGuids = new List<string>() { SampleRequestGuid.ToString() };
            PostGarmentSampleRequestCommandHandler unitUnderTest = CreatePostGarmentSampleRequestCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PostGarmentSampleRequestCommand RemoveGarmentSampleRequestCommand = new PostGarmentSampleRequestCommand(SampleRequestGuids, true);

            GarmentSampleRequest garmentSampleRequest = new GarmentSampleRequest(
                SampleRequestGuid, null, null, null, null, DateTimeOffset.Now, new BuyerId(1), "", "", new GarmentComodityId(1), null, null, "", "", DateTimeOffset.Now, "", "", "", false, false);

            _mockSampleRequestRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestReadModel>()
                {
                    garmentSampleRequest.GetReadModel()
                }.AsQueryable());

            _mockSampleRequestRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequest>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequest>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSampleRequestCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
