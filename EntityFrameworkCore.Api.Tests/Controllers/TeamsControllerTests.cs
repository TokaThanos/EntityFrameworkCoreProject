using EntityFrameworkCore.Api.Controllers;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Api.Tests.Controllers
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
        public async Task TeamsController_GetTeams_ReturnsOk()
        {
            // Arrange
            var expectedTeams = new List<TeamReadDto>
            {
                new TeamReadDto { Id = 1, Name = "Team A" },
                new TeamReadDto { Id = 2, Name = "Team B" }
            };
            _teamServiceMock.Setup(service => service.GetAllTeamsAsync())
                .ReturnsAsync(expectedTeams);

            // Act
            var result = await _teamsController.GetTeams();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var actualTeams = okResult.Value as IEnumerable<TeamReadDto>; ;
            actualTeams.Should().NotBeNull();
            actualTeams.Should().HaveCount(2);
            actualTeams.Should().BeAssignableTo<IEnumerable<TeamReadDto>>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public async Task TeamsController_GetTeam_ReturnsExpectedResult(int id)
        {
            // Arrange
            var expectedTeam = id > 0 ? new TeamReadInfoDto { TeamName = "Team", CoachName = "Coach" } : null;
            _teamServiceMock.Setup(service => service.GetTeamByIdAsync(id))
                .ReturnsAsync(expectedTeam);

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
    }
}
