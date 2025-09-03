using EntityFrameworkCore.Api.Controllers;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EntityFrameworkCore.Api.Tests.UnitTests.Controllers
{
    public class LeaguesControllerTests
    {
        private readonly Mock<ILeagueService> _leagueServiceMock;
        private readonly LeaguesController _leaguesController;

        public LeaguesControllerTests()
        {
            _leagueServiceMock = new Mock<ILeagueService>();
            _leaguesController = new LeaguesController(_leagueServiceMock.Object);
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

            _leagueServiceMock.Setup(service => service.GetAllLeaguesAsync())
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

            _leagueServiceMock.Setup(service => service.GetLeagueByIdAsync(id))
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

            _leagueServiceMock.Setup(service => service.AddLeagueAsync(requestInput))
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
        public async Task LeaguesController_DeleteLeague_ReturnsExpectedResult()
        {
            // Arrange
            var id = 1;

            _leagueServiceMock.Setup(service => service.DeleteLeagueByIdAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _leaguesController.DeleteLeague(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _leagueServiceMock.Verify(service => service.DeleteLeagueByIdAsync(id), Times.Once);
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

            _leagueServiceMock.Setup(service => service.UpdateLeagueAsync(id, requestInput))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _leaguesController.PutLeague(id, requestInput);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _leagueServiceMock.Verify(service => service.UpdateLeagueAsync(id, requestInput), Times.Once);
        }
    }
}
