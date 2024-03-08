using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BurgerRoyale.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "PaymentRequestId",
                table: "Orders");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("02a0b895-e092-47ce-94c0-e98286182b6b"), 3, "Sorvete de diversos sabores", "Sorvete", 7m },
                    { new Guid("30baa9a0-7c40-4352-b246-bf7497b6702b"), 2, "Copo 400 ml", "Refrigerante", 6m },
                    { new Guid("39d429d4-0ec7-46a4-b42d-976451956e12"), 1, "Porção de fritas.", "Fritas", 6.9m },
                    { new Guid("3c61a088-de0b-41f1-b1ad-8b60c4a9a7b8"), 0, "Hambúrguer de carne bovina.", "Burger Tradicional", 19.2m },
                    { new Guid("50923883-cb40-4066-b339-614ec0363614"), 1, "Porção de fritas pequena.", "Fritas Pequena", 4.9m },
                    { new Guid("73269e58-bb76-433a-846f-1ac7ea9170d5"), 3, "Sundae de diversos sabores", "Sundae", 7m },
                    { new Guid("a5fad99b-b768-4a85-9267-0b68bea75ee0"), 0, "Hambúrguer de carne bovina com o dobro de cheddar.", "Burger Duplo Cheddar", 23.9m },
                    { new Guid("a86a32f1-fec8-42b0-a24b-20e04323131e"), 0, "Hambúrguer de carne bovina com o dobro de bacon.", "Burger Duplo Bacon", 22.9m },
                    { new Guid("b367c8ca-d026-45fa-bb71-cf99051f3671"), 1, "Porção de fritas grande.", "Fritas Grande", 8.9m },
                    { new Guid("ed494baa-563b-4822-8ebf-e600eb82efdc"), 2, "500 ml com ou sem gás", "Água", 4m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("02a0b895-e092-47ce-94c0-e98286182b6b"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30baa9a0-7c40-4352-b246-bf7497b6702b"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("39d429d4-0ec7-46a4-b42d-976451956e12"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("3c61a088-de0b-41f1-b1ad-8b60c4a9a7b8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50923883-cb40-4066-b339-614ec0363614"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("73269e58-bb76-433a-846f-1ac7ea9170d5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a5fad99b-b768-4a85-9267-0b68bea75ee0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a86a32f1-fec8-42b0-a24b-20e04323131e"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b367c8ca-d026-45fa-bb71-cf99051f3671"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("ed494baa-563b-4822-8ebf-e600eb82efdc"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentRequestId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

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
    }
}
