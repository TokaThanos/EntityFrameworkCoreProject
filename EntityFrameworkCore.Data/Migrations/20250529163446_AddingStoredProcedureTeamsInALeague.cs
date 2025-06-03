using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingStoredProcedureTeamsInALeague : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetTeamsFromLeague
                @LeagueId INT
                AS
                BEGIN
                    SELECT t.Name AS TeamName, l.Name AS LeagueName
                    FROM Leagues l 
                    JOIN Teams t
                    ON t.LeagueId = l.Id
                    WHERE l.Id = @LeagueId;
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS sp_GetTeamsFromLeague;");
        }
    }
}
