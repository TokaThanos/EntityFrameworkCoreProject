using EntityFrameworkCore.Api.Controllers;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Leagues.Commands;
using EntityFrameworkCore.Application.Leagues.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EntityFrameworkCore.Api.Tests.UnitTests.Controllers
{
    public class LeaguesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly LeaguesController _leaguesController;

        public LeaguesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _leaguesController = new LeaguesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task LeaguesController_GetLeagues_ReturnsExpectedResult()
        {
            // Arrange
            var expectedOutput = new List<LeagueReadDto>
            {
                new LeagueReadDto { Id = 1, Name = "League 1" },
                new LeagueReadDto { Id = 2, Name = "League 2" }
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetLeaguesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _leaguesController.GetLeagues();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var responseValue = okResult.Value as IEnumerable<LeagueReadDto>;
            responseValue.Should().NotBeNull();
            responseValue.Should().HaveCount(2);
            responseValue.Should().BeAssignableTo<IEnumerable<LeagueReadDto>>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public async Task LeaguesController_GetLeague_ReturnsExpectedResult(int id)
        {
            // Arrange
            var exampleOutput = new LeagueReadInfoDto
            {
                LeagueName = "Test League",
                Teams = new List<TeamReadDto>
                {
                    new TeamReadDto { Id = 101, Name = "Team 1" },
                    new TeamReadDto { Id = 102, Name = "Team 2" }
                }
            };

            var expectedOutput = id > 0 ? exampleOutput : null;

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetLeagueByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _leaguesController.GetLeague(id);

            // Assert
            if (id > 0)
            {
                var okResult = result.Result as OkObjectResult;
                okResult.Should().NotBeNull();
                okResult.Value.Should().BeOfType<LeagueReadInfoDto>();
                okResult.Value.Should().BeEquivalentTo(expectedOutput);
            }
            else
            {
                result.Result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task LeaguesController_PostLeague_ReturnsExpectedResult()
        {
            // Arrange
            var requestInput = new LeagueCreateDto
            {
                LeagueName = "League"
            };

            var expectedOutput = new LeagueReadDto
            {
                Id = 1,
                Name = "League"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateLeagueCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _leaguesController.PostLeague(requestInput);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(_leaguesController.GetLeague));
            createdAtActionResult.Value.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public async Task LeaguesController_PostLeague_ReturnsBadRequest()
        {
            // Arrange
            var requestInput = new LeagueCreateDto
            {
                LeagueName = ""
            };

            var exceptionMessage = "League name can't be null";

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateLeagueCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _leaguesController.PostLeague(requestInput);

            // Assert
            var actionResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task LeaguesController_DeleteLeague_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteLeagueCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _leaguesController.DeleteLeague(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock
                .Verify(mediator => mediator.Send(It.Is<DeleteLeagueCommand>(
                    command => command.Id == id),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task LeaguesController_PutLeague_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;
            var requestInput = new LeagueCreateDto
            {
                LeagueName = "League"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateLeagueCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _leaguesController.PutLeague(id, requestInput);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock
                .Verify(mediator => mediator.Send(It.Is<UpdateLeagueCommand>(
                    command => command.Id == id && command.LeagueCreateDto == requestInput),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task LeaguesController_PutLeague_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var requestInput = new LeagueCreateDto
            {
                LeagueName = "League"
            };
            var exceptionMessage = "League Name can't be null";

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateLeagueCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _leaguesController.PutLeague(id, requestInput);

            // Assert
            var actionResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task LeaguesController_PutLeague_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            var requestInput = new LeagueCreateDto
            {
                LeagueName = ""
            };
            var exceptionMessage = $"League with ID {id} not found.";

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateLeagueCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

            // Act
            var result = await _leaguesController.PutLeague(id, requestInput);

            // Assert
            var actionResult = result.Should().BeOfType<NotFoundObjectResult>().Which;
            actionResult.StatusCode.Should().Be(404);
            actionResult.Value.Should().Be(exceptionMessage);
        }
    }
}
