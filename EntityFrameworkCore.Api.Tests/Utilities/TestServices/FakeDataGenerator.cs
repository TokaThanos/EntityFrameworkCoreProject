using Bogus;
using EntityFrameworkCore.Application.Dtos;

namespace EntityFrameworkCore.Api.Tests.Utilities.TestServices
{
    internal class FakeDataGenerator
    {
        private string[] mascots = { "Lions", "Tigers", "Warriors", "Eagles", "Wolves", "United", "FC" };
        private Faker<TeamCreateDto> GetTeamFaker()
        {
            return new Faker<TeamCreateDto>()
                .RuleFor(team => team.TeamName, (faker, team) => $"{faker.Address.City()} {faker.PickRandom(mascots)}")
                .RuleFor(team => team.CoachName, (faker, team) => faker.Name.FullName());
        }

        public TeamCreateDto GetTeam()
        {
            var teamFaker = GetTeamFaker();
            return teamFaker.Generate();
        }
    }
}
