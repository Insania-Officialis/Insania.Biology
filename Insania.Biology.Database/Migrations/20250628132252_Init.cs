using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Biology.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "insania_biology");

            migrationBuilder.CreateTable(
                name: "c_races",
                schema: "insania_biology",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false, comment: "Описание"),
                    max_age = table.Column<int>(type: "integer", nullable: true, comment: "Максимальный возраст в циклах"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_races", x => x.id);
                    table.UniqueConstraint("AK_c_races_alias", x => x.alias);
                },
                comment: "Расы");

            migrationBuilder.CreateTable(
                name: "c_nations",
                schema: "insania_biology",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false, comment: "Описание"),
                    language_for_personal_names = table.Column<string>(type: "text", nullable: false, comment: "Язык для имён"),
                    race_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор расы"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_nations", x => x.id);
                    table.UniqueConstraint("AK_c_nations_alias", x => x.alias);
                    table.ForeignKey(
                        name: "FK_c_nations_c_races_race_id",
                        column: x => x.race_id,
                        principalSchema: "insania_biology",
                        principalTable: "c_races",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Нации");

            migrationBuilder.CreateIndex(
                name: "IX_c_nations_race_id",
                schema: "insania_biology",
                table: "c_nations",
                column: "race_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "c_nations",
                schema: "insania_biology");

            migrationBuilder.DropTable(
                name: "c_races",
                schema: "insania_biology");
        }
    }
}
