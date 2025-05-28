using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingTeamsLeaguesAndCoachesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW vw_TeamsAndLeagues AS
                SELECT t.Name AS TeamName, l.Name AS LeagueName
                FROM TEAMS AS t 
                LEFT JOIN LEAGUES AS l
                ON t.LeagueId = l.Id" 
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW vw_TeamsAndLeagues");
        }
    }
}
