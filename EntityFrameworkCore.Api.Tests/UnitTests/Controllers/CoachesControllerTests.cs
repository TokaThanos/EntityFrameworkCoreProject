using EntityFrameworkCore.Api.Controllers;
using EntityFrameworkCore.Application.Coaches.Commands;
using EntityFrameworkCore.Application.Coaches.Queries;
using EntityFrameworkCore.Application.Dtos;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EntityFrameworkCore.Api.Tests.UnitTests.Controllers
{
    public class CoachesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CoachesController _coachesController;

        public CoachesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _coachesController = new CoachesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CoachesController_GetCoaches_ReturnsExpectedResult()
        {
            // Arrange
            var expectedOutput = new List<CoachReadDto>
            {
                new CoachReadDto { Id = 1, Name = "Coach A" },
                new CoachReadDto { Id = 2, Name = "Coach B" }
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetCoachesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _coachesController.GetCoaches();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var responseValue = okResult.Value as IEnumerable<CoachReadDto>;
            responseValue.Should().NotBeNull();
            responseValue.Should().HaveCount(2);
            responseValue.Should().BeAssignableTo<IEnumerable<CoachReadDto>>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public async Task CoachesController_GetCoach_ReturnsExpectedResult(int id)
        {
            // Arrange
            var expectedOutput = id > 0 ? new CoachReadInfoDto { CoachName = "Coach", TeamName = "Team" } : null;

            _mediatorMock
                .Setup(mediator => mediator.Send<CoachReadInfoDto?>(It.IsAny<GetCoachByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _coachesController.GetCoach(id);

            // Assert
            if (id > 0)
            {
                var okResult = result.Result as OkObjectResult;
                okResult.Should().NotBeNull();
                okResult.Value.Should().BeOfType<CoachReadInfoDto>();
            }
            else
            {
                result.Result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task CoachesController_PostCoach_ReturnsExpectedResult()
        {
            // Arrange
            var requestInput = new CoachCreateDto
            {
                CoachName = "Coach"
            };

            var expectedOutput = new CoachReadDto
            {
                Id = 1,
                Name = "Coach"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateCoachCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _coachesController.PostCoach(requestInput);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(_coachesController.GetCoach));
            createdAtActionResult.Value.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public async Task CoachesController_PostCoach_ReturnsBadRequest()
        {
            // Arrange
            var requestInput = new CoachCreateDto
            {
                CoachName = "Coach"
            };

            var exceptionMessage = "Coach name can't be null";

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateCoachCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _coachesController.PostCoach(requestInput);

            // Assert
            var actionResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task CoachesController_DeleteCoach_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteCoachCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _coachesController.DeleteCoach(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock
                .Verify(mediator => mediator.Send(It.Is<DeleteCoachCommand>(
                    command => command.Id == id),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CoachesController_PutCoach_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;
            var requestInput = new CoachCreateDto
            {
                CoachName = "Coach"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateCoachCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _coachesController.PutCoach(id, requestInput);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock
                .Verify(mediator => mediator.Send(It.Is<UpdateCoachCommand>(
                    command => command.Id == id && command.CoachCreateDto == requestInput),
                    It.IsAny<CancellationToken>()),Times.Once);
        }

        [Fact]
        public async Task MatchesController_PutMatch_ReturnsNotFound()
        {
            // Arrange
            int id = 0;
            var requestInput = new CoachCreateDto
            {
                CoachName = "Coach"
            };
            var exceptionMessage = $"Match with Id {id} not found!";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateCoachCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

            // Act
            var result = await _coachesController.PutCoach(id, requestInput);

            // Assert
            var actionResult = result.Should().BeOfType<NotFoundObjectResult>().Which;
            actionResult.StatusCode.Should().Be(404);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task CoachesController_PutMatch_ReturnsBadRequest()
        {
            // Arrange
            int id = 0;
            var requestInput = new CoachCreateDto
            {
                CoachName = "Coach"
            };
            var exceptionMessage = "Coach Name can't be null";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateCoachCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _coachesController.PutCoach(id, requestInput);

            // Assert
            var actionResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be(exceptionMessage);
        }
    }
}
