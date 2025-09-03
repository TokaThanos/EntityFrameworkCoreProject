using EntityFrameworkCore.Api.Tests.Utilities.TestFactory;
using EntityFrameworkCore.Api.Tests.Utilities.TestServices;
using EntityFrameworkCore.Application.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EntityFrameworkCore.Api.Tests.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class TeamsIntegrationTests
    {
        private readonly DockerWebApplicationTestFactory _factory;
        private readonly HttpClient _client;
        private readonly FakeDataGenerator _fakeDataService;

        public TeamsIntegrationTests(DockerWebApplicationTestFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _fakeDataService = new Utilities.TestServices.FakeDataGenerator();
        }

        [Fact]
        public async Task PostTeam_Then_GetById_ShouldReturn_TheSameTeamData()
        {
            // Arrange
            var teamData = _fakeDataService.GetTeamCreateDtoFake();

            // Act
            var postResponse = await _client.PostAsJsonAsync(HttpHelper.teamsRequestUrl, teamData);
            var createdTeam = await postResponse.Content.ReadFromJsonAsync<TeamReadDto>();
            var teamId = createdTeam!.Id;

            var getResponse = await _client.GetAsync($"{HttpHelper.teamsRequestUrl}/{teamId}");
            var teamInfo = await getResponse.Content.ReadFromJsonAsync<TeamReadInfoDto>();

            // Assert
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createdTeam.Should().NotBeNull();
            createdTeam!.Id.Should().BeGreaterThan(0);
            teamInfo!.TeamName.Should().Be(teamData.TeamName);
            teamInfo.CoachName.Should().Be(teamData.CoachName);
        }

        [Fact]
        public async Task PostTeam_Then_PutTeam_ShouldReturn_NotFound()
        {
            // Arrange
            var teamData = _fakeDataService.GetTeamCreateDtoFake();
            var updateTeamData = _fakeDataService.GetTeamUpdateDtoFake();

            // Act
            var postResponse = await _client.PostAsJsonAsync(HttpHelper.teamsRequestUrl, teamData);
            var createdTeam = await postResponse.Content.ReadFromJsonAsync<TeamReadDto>();
            var teamId = createdTeam!.Id;

            var putResponse = await _client.PutAsJsonAsync($"{HttpHelper.teamsRequestUrl}/{teamId}", updateTeamData);

            // Assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostTeam_Then_DeleteTeam_ShouldReturn_ExpectedResult()
        {
            // Arrange
            var teamData = _fakeDataService.GetTeamCreateDtoFake();

            // Act
            var postResponse = await _client.PostAsJsonAsync(HttpHelper.teamsRequestUrl, teamData);
            var createdTeam = await postResponse.Content.ReadFromJsonAsync<TeamReadDto>();
            var teamId = createdTeam!.Id;

            var deleteResponse = await _client.DeleteAsync($"{HttpHelper.teamsRequestUrl}/{teamId}");
            
            var getResponseAfterDelete = await _client.GetAsync($"{HttpHelper.teamsRequestUrl}/{teamId}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            getResponseAfterDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
