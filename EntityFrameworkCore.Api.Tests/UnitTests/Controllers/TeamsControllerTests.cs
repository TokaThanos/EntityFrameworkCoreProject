using EntityFrameworkCore.Api.Controllers;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EntityFrameworkCore.Api.Tests.UnitTests.Controllers
{
    public class TeamsControllerTests
    {
        private readonly TeamsController _teamsController;
        private readonly Mock<ITeamService> _teamServiceMock;

        public TeamsControllerTests()
        {
            _teamServiceMock = new Mock<ITeamService>();
            _teamsController = new TeamsController(_teamServiceMock.Object);
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

            _teamServiceMock.Setup(service => service.GetAllTeamsAsync())
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

            _teamServiceMock.Setup(service => service.GetTeamByIdAsync(id))
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

            _teamServiceMock.Setup(service => service.AddTeamAsync(requestInput))
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
        public async Task TeamsController_DeleteTeam_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;

            _teamServiceMock.Setup(service => service.DeleteTeamByIdAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _teamsController.DeleteTeam(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _teamServiceMock.Verify(service => service.DeleteTeamByIdAsync(id), Times.Once);
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

            _teamServiceMock.Setup(service => service.UpdateTeamAsync(id, requestInput))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _teamsController.PutTeam(id, requestInput);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _teamServiceMock.Verify(service => service.UpdateTeamAsync(id, requestInput), Times.Once);
        }
    }
}
