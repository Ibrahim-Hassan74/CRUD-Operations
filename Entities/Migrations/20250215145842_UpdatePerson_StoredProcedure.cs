using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class UpdatePerson_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_UpdatePerson = @"
                CREATE PROCEDURE [dbo].[UpdatePerson]
                (
                @PersonID uniqueidentifier,
                @PersonName NVARCHAR(50),
                @Email NVARCHAR(50),    
                @DateOfBirth DATETIME2(7),
                @Gender NVARCHAR(10),   
                @CountryID uniqueidentifier,
                @Address NVARCHAR(200),
                @ReciveNewsLetters BIT
                )
                AS  
                BEGIN
                    UPDATE [dbo].[Persons]
                    SET PersonName = @PersonName,
                        Email = @Email,
                        DateOfBirth = @DateOfBirth, 
                        Gender = @Gender,
                        CountryID = @CountryID,
                        Address = @Address, 
                        ReciveNewsLetters = @ReciveNewsLetters
                    WHERE PersonID = @PersonID
                END
            ";
            migrationBuilder.Sql(sp_UpdatePerson);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_UpdatePerson = @"
                DROP PROCEDURE [dbo].[UpdatePerson]
            ";
            migrationBuilder.Sql(sp_UpdatePerson);
        }
    }
}
