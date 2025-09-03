using Bogus;
using EntityFrameworkCore.Application.Dtos;

namespace EntityFrameworkCore.Api.Tests.Utilities.TestServices
{
    internal class FakeDataGenerator
    {
        private string[] mascots = 
        { 
            "Lions", "Tigers", "Warriors", "Eagles", "Wolves",
            "Panthers", "Bulls", "Falcons", "Sharks", "Hawks",
            "Raiders", "Titans", "Rovers", "Strikers", "United", "FC" 
        };
        private Faker<TeamCreateDto> GetTeamCreateDtoFaker()
        {
            return new Faker<TeamCreateDto>()
                .RuleFor(team => team.TeamName, (faker, team) => $"{faker.Address.City()} {faker.PickRandom(mascots)}")
                .RuleFor(team => team.CoachName, (faker, team) => faker.Name.FullName());
        }

        private Faker<TeamUpdateDto> GetTeamUpdateDtoFaker()
        {
            return new Faker<TeamUpdateDto>()
                .RuleFor(team => team.CoachName, (faker, team) => faker.Name.FullName())
                .RuleFor(team => team.LeagueName, (faker, team) => faker.Address.Country());
        }

        public TeamCreateDto GetTeamCreateDtoFake()
        {
            var teamFaker = GetTeamCreateDtoFaker();
            return teamFaker.Generate();
        }

        public TeamUpdateDto GetTeamUpdateDtoFake()
        {
            var teamFaker = GetTeamUpdateDtoFaker();
            return teamFaker.Generate();
        }
    }
}
