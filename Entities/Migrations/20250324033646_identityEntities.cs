﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class identityEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TIN",
                table: "Persons");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("12e15727-d369-49a9-8b13-bc22e9362179"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("012107df-862f-4f16-ba94-e5c16886f005"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("29339209-63f5-492f-8459-754943c74abf"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"));

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryID",
                table: "Persons",
                column: "CountryID");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TIN",
                table: "Persons",
                sql: "len([TaxIdentificationNumber]) = 8");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Countries_CountryID",
                table: "Persons",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "CountryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Countries_CountryID",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Persons_CountryID",
                table: "Persons");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_TIN",
                table: "Persons");

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), "China" },
                    { new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"), "Philippines" },
                    { new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"), "China" },
                    { new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"), "Thailand" },
                    { new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"), "Palestinian Territory" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReciveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("012107df-862f-4f16-ba94-e5c16886f005"), "413 Sachtjen Way", new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), new DateTime(1990, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "hmosco8@tripod.com", "Male", "Hansiain", false },
                    { new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"), "2 Warrior Avenue", new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"), new DateTime(1990, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "mconachya@va.gov", "Female", "Minta", false },
                    { new Guid("29339209-63f5-492f-8459-754943c74abf"), "57449 Brown Way", new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), new DateTime(1983, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "mjarrell6@wisc.edu", "Male", "Maddy", false },
                    { new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"), "97570 Raven Circle", new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"), new DateTime(1988, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "mlingfoot5@netvibes.com", "Male", "Mitchael", false },
                    { new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"), "50467 Holy Cross Crossing", new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"), new DateTime(1995, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "ttregona4@stumbleupon.com", "Gender", "Tani", false },
                    { new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"), "9334 Fremont Street", new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"), new DateTime(1987, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "vklussb@nationalgeographic.com", "Female", "Verene", false },
                    { new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"), "4 Stuart Drive", new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), new DateTime(1998, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "pretchford7@virginia.edu", "Female", "Pegeen", false },
                    { new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"), "4 Parkside Point", new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"), new DateTime(1989, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "mwebsdale0@people.com.cn", "Female", "Marguerite", false },
                    { new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"), "6 Morningstar Circle", new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"), new DateTime(1990, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ushears1@globo.com", "Female", "Ursa", false },
                    { new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"), "73 Heath Avenue", new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"), new DateTime(1995, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "fbowsher2@howstuffworks.com", "Male", "Franchot", false },
                    { new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"), "484 Clarendon Court", new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"), new DateTime(1997, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "lwoodwing9@wix.com", "Male", "Lombard", false },
                    { new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"), "83187 Merry Drive", new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), new DateTime(1987, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "asarvar3@dropbox.com", "Male", "Angie", false }
                });

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TIN",
                table: "Persons",
                sql: "len([TIN]) = 8");
        }
    }
}
