using EntityFrameworkCore.Api.Controllers;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Matches.Commands;
using EntityFrameworkCore.Application.Matches.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EntityFrameworkCore.Api.Tests.UnitTests.Controllers
{
    public class MatchesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly MatchesController _controller;

        public MatchesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new MatchesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task MatchesController_GetMatches_ReturnsExpectedResult()
        {
            // Arrange
            var expectedOutput = new List<MatchReadDto>
            {
                new MatchReadDto { Id = 1, HomeTeamName = "Team A", AwayTeamName = "Team B" },
                new MatchReadDto { Id = 2, HomeTeamName = "Team C", AwayTeamName = "Team B" },
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetMatchesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _controller.GetMatches();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var responseValue = okResult.Value as IEnumerable<MatchReadDto>;
            responseValue.Should().NotBeNull();
            responseValue.Should().HaveCount(2);
            responseValue.Should().BeAssignableTo<IEnumerable<MatchReadDto>>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public async Task MatchesController_GetMatch_ReturnsExpectedResult(int id)
        {
            // Arrange
            var exampleOutput = new MatchReadInfoDto
            {
                HomeTeamName = "Team A",
                AwayTeamName = "Team B",
                Status = Domain.Enums.MatchStatus.Scheduled,
            };

            var expectedOutput = id > 0 ? exampleOutput : null;

            _mediatorMock
                .Setup(mediator => mediator.Send<MatchReadInfoDto?>(It.IsAny<GetMatchByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _controller.GetMatch(id);

            // Assert
            if (id > 0)
            {
                var okResult = result.Result as OkObjectResult;
                okResult.Should().NotBeNull();
                okResult.Value.Should().NotBeNull();
                okResult.Value.Should().BeEquivalentTo(expectedOutput);
            }
            else
            {
                result.Result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task MatchesController_PostMatch_ReturnsExpectedResult()
        {
            // Arrange
            var requestInput = new MatchCreateDto
            {
                AwayTeamName = "Team B",
                HomeTeamName = "Team A",
                TicketPrice = 20.00M,
            };

            var expectedOutput = new MatchReadDto
            {
                Id = 1,
                HomeTeamName = "Team A",
                AwayTeamName = "Team B"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateMatchCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _controller.PostMatch(requestInput);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(_controller.GetMatch));
            createdAtActionResult.Value.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public async Task MatchesController_PostMatch_ReturnsBadRequest()
        {
            // Arrange
            var requestInput = new MatchCreateDto
            {
                HomeTeamName = "",
                AwayTeamName = ""
            };

            var exceptionMessage = "Invalid teams!";

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateMatchCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.PostMatch(requestInput);

            // Assert
            var actionResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task MatchesController_PutMatch_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;
            var requestInput = new MatchUpdateDto
            {
                HomeTeamName = "Team B",
                AwayTeamName = "Team A",
                TicketPrice = 25,
                Date = DateTime.UtcNow
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateMatchCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutMatch(id, requestInput);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock
                .Verify(mediator => mediator.Send(It.Is<UpdateMatchCommand>(
                    command => command.Id == id && command.MatchUpdateDto == requestInput), 
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MatchesController_PutMatch_ReturnsNotFound()
        {
            // Arrange
            int id = 0;
            var exceptionMessage = $"Match with Id {id} not found!";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateMatchCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

            // Act
            var result = await _controller.PutMatch(id, new MatchUpdateDto());

            // Assert
            var actionResult = result.Should().BeOfType<NotFoundObjectResult>().Which;
            actionResult.StatusCode.Should().Be(404);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task MatchesController_PutMatch_ReturnsBadRequest()
        {
            // Arrange
            int id = 0;
            var exceptionMessage = "Invalid team!";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateMatchCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.PutMatch(id, new MatchUpdateDto());

            // Assert
            var actionResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task MatchesController_DeleteMatch_ReturnsExpectedResult()
        {
            // Arrange
            int id = 1;
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteMatchCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteMatch(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock
                .Verify(mediator => mediator.Send(It.Is<DeleteMatchCommand>(
                    command => command.Id == id),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
