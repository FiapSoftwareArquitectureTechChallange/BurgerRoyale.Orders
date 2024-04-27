using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BurgerRoyale.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("0cbf684b-be68-4a28-bd57-77ac9066ce55"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("2e168b17-5e08-4049-bcbd-248ced33f901"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("3453ea67-2380-4d99-b6fb-07c2f1f39439"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40d5632e-83c5-4cad-afe8-a0a5ef6b21aa"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("6668ab9b-b0e1-448d-80a5-ec9abf7bed36"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9344aef2-5f25-45d3-9fc1-76e70c432ebf"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("98f1d726-6650-490d-9b0e-82c2a773210f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("ad7bc119-0b84-48fe-9d4b-943c1598263f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c59afddd-23ad-4e56-bc48-c6b76a2813bf"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d8d71926-fa7b-4381-ac1f-23e4fe76d1f6"));

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("3cd19be6-8e13-423e-a203-87f7a31c61c5"), 1, "Porção de fritas.", "Fritas", 6.9m },
                    { new Guid("47850b52-17d0-4f2c-8f3c-413abb1d976b"), 0, "Hambúrguer de carne bovina com o dobro de cheddar.", "Burger Duplo Cheddar", 23.9m },
                    { new Guid("8049bb07-1c09-4ce6-aaa4-a8475f46a0e6"), 0, "Hambúrguer de carne bovina.", "Burger Tradicional", 19.2m },
                    { new Guid("95ca0d02-3783-4b2b-a805-bea43f5a8ec1"), 0, "Hambúrguer de carne bovina com o dobro de bacon.", "Burger Duplo Bacon", 22.9m },
                    { new Guid("9b34391e-296f-4005-bbb1-721374f2516a"), 2, "500 ml com ou sem gás", "Água", 4m },
                    { new Guid("cb3d778f-a648-49c6-8460-a8aa26144219"), 2, "Copo 400 ml", "Refrigerante", 6m },
                    { new Guid("e82dfd7b-64c6-4121-ad0b-a9027ee0773c"), 1, "Porção de fritas grande.", "Fritas Grande", 8.9m },
                    { new Guid("e94c8cb4-e7bf-4a39-b6f8-5ea07090fd03"), 3, "Sundae de diversos sabores", "Sundae", 7m },
                    { new Guid("f1de9802-a0b0-4b9f-a814-8e8622e67ce7"), 1, "Porção de fritas pequena.", "Fritas Pequena", 4.9m },
                    { new Guid("f5271c70-e7bd-4896-9eef-c19132756500"), 3, "Sorvete de diversos sabores", "Sorvete", 7m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("3cd19be6-8e13-423e-a203-87f7a31c61c5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("47850b52-17d0-4f2c-8f3c-413abb1d976b"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8049bb07-1c09-4ce6-aaa4-a8475f46a0e6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("95ca0d02-3783-4b2b-a805-bea43f5a8ec1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9b34391e-296f-4005-bbb1-721374f2516a"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("cb3d778f-a648-49c6-8460-a8aa26144219"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e82dfd7b-64c6-4121-ad0b-a9027ee0773c"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e94c8cb4-e7bf-4a39-b6f8-5ea07090fd03"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f1de9802-a0b0-4b9f-a814-8e8622e67ce7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f5271c70-e7bd-4896-9eef-c19132756500"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("0cbf684b-be68-4a28-bd57-77ac9066ce55"), 1, "Porção de fritas pequena.", "Fritas Pequena", 4.9m },
                    { new Guid("2e168b17-5e08-4049-bcbd-248ced33f901"), 0, "Hambúrguer de carne bovina.", "Burger Tradicional", 19.2m },
                    { new Guid("3453ea67-2380-4d99-b6fb-07c2f1f39439"), 3, "Sorvete de diversos sabores", "Sorvete", 7m },
                    { new Guid("40d5632e-83c5-4cad-afe8-a0a5ef6b21aa"), 3, "Sundae de diversos sabores", "Sundae", 7m },
                    { new Guid("6668ab9b-b0e1-448d-80a5-ec9abf7bed36"), 1, "Porção de fritas grande.", "Fritas Grande", 8.9m },
                    { new Guid("9344aef2-5f25-45d3-9fc1-76e70c432ebf"), 2, "Copo 400 ml", "Refrigerante", 6m },
                    { new Guid("98f1d726-6650-490d-9b0e-82c2a773210f"), 2, "500 ml com ou sem gás", "Água", 4m },
                    { new Guid("ad7bc119-0b84-48fe-9d4b-943c1598263f"), 0, "Hambúrguer de carne bovina com o dobro de cheddar.", "Burger Duplo Cheddar", 23.9m },
                    { new Guid("c59afddd-23ad-4e56-bc48-c6b76a2813bf"), 0, "Hambúrguer de carne bovina com o dobro de bacon.", "Burger Duplo Bacon", 22.9m },
                    { new Guid("d8d71926-fa7b-4381-ac1f-23e4fe76d1f6"), 1, "Porção de fritas.", "Fritas", 6.9m }
                });
        }
    }
}
