using EntityFrameworkCore.Api.Controllers;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Teams.Commands;
using EntityFrameworkCore.Application.Teams.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EntityFrameworkCore.Api.Tests.UnitTests.Controllers
{
    public class TeamsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TeamsController _teamsController;

        public TeamsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _teamsController = new TeamsController(_mediatorMock.Object);
        }
        [Fact]
        public async Task TeamsController_GetTeams_ReturnsExpectedResult()
        {
            // Arrange
            var expectedOutput = new List<TeamReadDto>
            {
                new TeamReadDto { Id = 1, Name = "Team A" },
                new TeamReadDto { Id = 2, Name = "Team B" }
            };

            _mediatorMock.
                Setup(mediator => mediator.Send(It.IsAny<GetTeamsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _teamsController.GetTeams();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var responseValue = okResult.Value as IEnumerable<TeamReadDto>;
            responseValue.Should().NotBeNull();
            responseValue.Should().HaveCount(2);
            responseValue.Should().BeAssignableTo<IEnumerable<TeamReadDto>>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public async Task TeamsController_GetTeam_ReturnsExpectedResult(int id)
        {
            // Arrange
            var expectedOutput = id > 0 ? new TeamReadInfoDto { TeamName = "Team", CoachName = "Coach" } : null;

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetTeamByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _teamsController.GetTeam(id);

            // Assert
            if (id > 0)
            {
                var okResult = result.Result as OkObjectResult;
                okResult.Should().NotBeNull();
                okResult.Value.Should().BeOfType<TeamReadInfoDto>();
            }
            else
            {
                result.Result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task TeamsController_PostTeam_ReturnsExpectedResult()
        {
            // Arrange
            var requestInput = new TeamCreateDto
            {
                TeamName = "Test Team",
                CoachName = "Test Coach",
                LeagueName = "Test League"
            };

            var expectedOutput = new TeamReadDto
            {
                Id = 1,
                Name = "Test Team"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateTeamCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOutput);

            // Act
            var result = await _teamsController.PostTeam(requestInput);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(_teamsController.GetTeam));
            createdAtActionResult.Value.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public async Task TeamsController_PostTeam_ReturnsBadRequest()
        {
            // Arrange
            var requestInput = new TeamCreateDto
            {
                TeamName = "Test Team",
                CoachName = "Test Coach",
                LeagueName = "Test League"
            };

            var exceptionMessage = $"League {requestInput.LeagueName} not found.";

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CreateTeamCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _teamsController.PostTeam(requestInput);

            // Assert
            var actionResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task TeamsController_DeleteTeam_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<DeleteTeamCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _teamsController.DeleteTeam(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock
                .Verify(mediator => mediator.Send(It.Is<DeleteTeamCommand>(
                    command => command.Id == id),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task TeamsController_PutTeam_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;
            var requestInput = new TeamUpdateDto
            {
                TeamName = "Updated Team",
                CoachName = "Updated Coach",
                LeagueName = "Updated League"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateTeamCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _teamsController.PutTeam(id, requestInput);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock
                .Verify(mediator => mediator.Send(It.Is<UpdateTeamCommand>(
                    command => command.Id == id),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task TeamsController_PutTeam_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            var requestInput = new TeamUpdateDto
            {
                TeamName = "Updated Team",
                CoachName = "Updated Coach",
                LeagueName = "Updated League"
            };

            var exceptionMessage = $"Team with ID {id} not found.";

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateTeamCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

            // Act
            var result = await _teamsController.PutTeam(id, requestInput);

            // Assert
            var actionResult = result.Should().BeOfType<NotFoundObjectResult>().Which;
            actionResult.StatusCode.Should().Be(404);
            actionResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async Task TeamsController_PutTeam_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var requestInput = new TeamUpdateDto
            {
                TeamName = "Updated Team",
                CoachName = "Updated Coach",
                LeagueName = "Updated League"
            };

            var exceptionMessage = $"League {requestInput.LeagueName} not found.";

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<UpdateTeamCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _teamsController.PutTeam(id, requestInput);

            // Assert
            var actionResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be(exceptionMessage);
        }
    }
}
