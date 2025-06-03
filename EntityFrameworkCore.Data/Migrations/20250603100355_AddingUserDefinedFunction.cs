using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserDefinedFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION fn_GetCoachNameByTeamId (@TeamId INT)
                RETURNS VARCHAR(50)
                AS
                BEGIN
                    DECLARE @CoachName VARCHAR(50)

                    SELECT @CoachName = c.Name
                    FROM Teams t
                    JOIN Coaches c ON t.CoachId = c.Id
                    WHERE t.Id = @TeamId

                    RETURN @CoachName
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS fn_GetCoachNameByTeamId;");
        }
    }
}
