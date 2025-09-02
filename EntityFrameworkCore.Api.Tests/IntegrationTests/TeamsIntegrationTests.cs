using EntityFrameworkCore.Api.Tests.Utilities.TestFactory;
using EntityFrameworkCore.Api.Tests.Utilities.TestServices;
using EntityFrameworkCore.Application.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace EntityFrameworkCore.Api.Tests.IntegrationTests
{
    public class TeamsIntegrationTests : IClassFixture<DockerWebApplicationTestFactory>
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
            var fakeTeam = _fakeDataService.GetTeam();

            // Act
            var postResponse = await _client.PostAsJsonAsync(HttpHelper.teamsRequestUrl, fakeTeam);
            var createdTeam = await postResponse.Content.ReadFromJsonAsync<TeamReadDto>();
            var teamId = createdTeam!.Id;


            var getResponse = await _client.GetAsync($"{HttpHelper.teamsRequestUrl}/{teamId}");
            var teamInfo = await getResponse.Content.ReadFromJsonAsync<TeamReadInfoDto>();

            // Assert
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createdTeam.Should().NotBeNull();
            createdTeam!.Id.Should().BeGreaterThan(0);
            teamInfo!.TeamName.Should().Be(fakeTeam.TeamName);
            teamInfo.CoachName.Should().Be(fakeTeam.CoachName);
        }
    }
}
