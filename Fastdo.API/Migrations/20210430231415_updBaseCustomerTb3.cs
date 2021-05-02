using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class updBaseCustomerTb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Areas_AreaId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_Id",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacies_Customers_CustomerId",
                table: "Pharmacies");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Customers_CustomerId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_Customers_CustomerId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "BaseCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_AreaId",
                table: "BaseCustomer",
                newName: "IX_BaseCustomer_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseCustomer",
                table: "BaseCustomer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseCustomer_Areas_AreaId",
                table: "BaseCustomer",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseCustomer_AspNetUsers_Id",
                table: "BaseCustomer",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacies_BaseCustomer_CustomerId",
                table: "Pharmacies",
                column: "CustomerId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_BaseCustomer_CustomerId",
                table: "Stocks",
                column: "CustomerId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_BaseCustomer_CustomerId",
                table: "TechnicalSupportQuestions",
                column: "CustomerId",
                principalTable: "BaseCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseCustomer_Areas_AreaId",
                table: "BaseCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseCustomer_AspNetUsers_Id",
                table: "BaseCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacies_BaseCustomer_CustomerId",
                table: "Pharmacies");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_BaseCustomer_CustomerId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSupportQuestions_BaseCustomer_CustomerId",
                table: "TechnicalSupportQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseCustomer",
                table: "BaseCustomer");

            migrationBuilder.RenameTable(
                name: "BaseCustomer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_BaseCustomer_AreaId",
                table: "Customers",
                newName: "IX_Customers_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Areas_AreaId",
                table: "Customers",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_Id",
                table: "Customers",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacies_Customers_CustomerId",
                table: "Pharmacies",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Customers_CustomerId",
                table: "Stocks",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSupportQuestions_Customers_CustomerId",
                table: "TechnicalSupportQuestions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
