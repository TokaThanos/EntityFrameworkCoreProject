using EntityFrameworkCore.Api.Tests.Utilities.TestFactory;
using EntityFrameworkCore.Api.Tests.Utilities.TestServices;
using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Domain.Enums;
using FluentAssertions;
using System.Net.Http.Json;

namespace EntityFrameworkCore.Api.Tests.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class MatchesIntegrationTests
    {
        private readonly DockerWebApplicationTestFactory _factory;
        private readonly HttpClient _client;
        private readonly FakeDataGenerator _fakeDataService;

        public MatchesIntegrationTests(DockerWebApplicationTestFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _fakeDataService = new Utilities.TestServices.FakeDataGenerator();
        }

        [Fact]
        public async Task PostMatch_Then_GetById_ShouldReturn_TheSameMatchData()
        {
            // Arrange
            var homeTeam = _fakeDataService.GetTeamCreateDtoFake();
            var awayTeam = _fakeDataService.GetTeamCreateDtoFake();

            var homeTeamResponse = await _client.PostAsJsonAsync(HttpHelper.teamsRequestUrl, homeTeam);
            var awayTeamResponse = await _client.PostAsJsonAsync(HttpHelper.teamsRequestUrl, awayTeam);

            var matchData = new MatchCreateDto
            {
                HomeTeamName = homeTeam!.TeamName,
                AwayTeamName = awayTeam!.TeamName,
                TicketPrice = 22,
                Date = DateTime.UtcNow.Date
            };

            // Act
            var postResponse = await _client.PostAsJsonAsync(HttpHelper.matchesRequestUrl, matchData);
            var createdMatch = await postResponse.Content.ReadFromJsonAsync<MatchReadDto>();
            var matchId = createdMatch!.Id;

            var getRespone = await _client.GetAsync($"{HttpHelper.matchesRequestUrl}/{matchId}");
            var matchInfo = await getRespone.Content.ReadFromJsonAsync<MatchReadInfoDto>();

            // Assert
            matchInfo.Should().NotBeNull();
            matchInfo.HomeTeamName.Should().Be(homeTeam.TeamName);
            matchInfo.AwayTeamName.Should().Be(awayTeam.TeamName);
            matchInfo.Status.Should().Be(MatchStatus.Scheduled);
        }
    }
}
