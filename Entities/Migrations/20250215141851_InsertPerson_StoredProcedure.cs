using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertPerson_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                
                CREATE PROCEDURE [dbo].[InsertPerson]
                (
                @PersonID uniqueidentifier,
                @PersonName NVARCHAR(50),
                @Email NVARCHAR(50),
                @DateOfBirth DATETIME2(7),
                @Gender NVARCHAR(10),
                @CountryID uniqueidentifier,
                @Address NVARCHAR(200),
                @ReciveNewsLetters BIT)
                AS
                BEGIN
                    INSERT INTO [dbo].[Persons] (PersonID ,PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReciveNewsLetters)
                    VALUES (@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReciveNewsLetters)   
                END
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                DROP PROCEDURE [dbo].[InsertPerson]
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
