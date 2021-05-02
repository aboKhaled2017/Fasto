using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class resetInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminHistoryEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OccuredAt = table.Column<DateTime>(nullable: false),
                    IssuerId = table.Column<string>(nullable: true),
                    OperationType = table.Column<string>(nullable: true),
                    ToId = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminHistoryEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    SuperAreaId = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Areas_SuperAreaId",
                        column: x => x.SuperAreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    confirmCode = table.Column<string>(maxLength: 15, nullable: true),
                    willBeNewEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseDrugs",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Desc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseDrugs", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Complains",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    SuperAdminId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Admins_Admins_SuperAdminId",
                        column: x => x.SuperAdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PersPhone = table.Column<string>(nullable: false),
                    LandlinePhone = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    AreaId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    CustomerId = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    MgrName = table.Column<string>(nullable: false),
                    OwnerName = table.Column<string>(nullable: false),
                    LicenseImgSrc = table.Column<string>(nullable: false),
                    CommercialRegImgSrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    CustomerId = table.Column<string>(nullable: false),
                    MgrName = table.Column<string>(nullable: false),
                    OwnerName = table.Column<string>(nullable: false),
                    LicenseImgSrc = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CommercialRegImgSrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Stocks_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalSupportQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<string>(nullable: true),
                    AdminId = table.Column<string>(nullable: true),
                    RelatedToId = table.Column<Guid>(nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    SeenAt = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    AdminId1 = table.Column<string>(nullable: true),
                    BaseCustomerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSupportQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalSupportQuestions_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalSupportQuestions_Admins_AdminId1",
                        column: x => x.AdminId1,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalSupportQuestions_Customers_BaseCustomerId",
                        column: x => x.BaseCustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalSupportQuestions_TechnicalSupportQuestions_RelatedToId",
                        column: x => x.RelatedToId,
                        principalTable: "TechnicalSupportQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalSupportQuestions_Customers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LzDrugs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ConsumeType = table.Column<int>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    ValideDate = table.Column<DateTime>(nullable: false),
                    PriceType = table.Column<int>(nullable: false),
                    UnitType = table.Column<int>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    PharmacyId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LzDrugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LzDrugs_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StkDrugPackagesRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkDrugPackagesRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StkDrugPackagesRequests_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmaciesInStocks",
                columns: table => new
                {
                    PharmacyId = table.Column<string>(nullable: false),
                    StockId = table.Column<string>(nullable: false),
                    PharmacyReqStatus = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaciesInStocks", x => new { x.PharmacyId, x.StockId });
                    table.ForeignKey(
                        name: "FK_PharmaciesInStocks_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmaciesInStocks_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StkDrugs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Discount = table.Column<string>(nullable: false),
                    StockId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkDrugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StkDrugs_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StocksWithPharmaClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StockId = table.Column<string>(nullable: false),
                    ClassName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocksWithPharmaClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocksWithPharmaClasses_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LzDrugRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: false),
                    LzDrugId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LzDrugRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LzDrugRequests_LzDrugs_LzDrugId",
                        column: x => x.LzDrugId,
                        principalTable: "LzDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LzDrugRequests_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StkDrugInStkDrgPackagesRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StkDrugId = table.Column<Guid>(nullable: false),
                    StkDrugPackageId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkDrugInStkDrgPackagesRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StkDrugInStkDrgPackagesRequests_StkDrugs_StkDrugId",
                        column: x => x.StkDrugId,
                        principalTable: "StkDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StkDrugInStkDrgPackagesRequests_StkDrugPackagesRequests_StkDrugPackageId",
                        column: x => x.StkDrugPackageId,
                        principalTable: "StkDrugPackagesRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PharmaciesInStockClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: true),
                    StockClassId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaciesInStockClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmaciesInStockClasses_StocksWithPharmaClasses_StockClassId",
                        column: x => x.StockClassId,
                        principalTable: "StocksWithPharmaClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_SuperAdminId",
                table: "Admins",
                column: "SuperAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_SuperAreaId",
                table: "Areas",
                column: "SuperAreaId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AreaId",
                table: "Customers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugRequests_PharmacyId",
                table: "LzDrugRequests",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugRequests_LzDrugId_PharmacyId",
                table: "LzDrugRequests",
                columns: new[] { "LzDrugId", "PharmacyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LzDrugs_PharmacyId",
                table: "LzDrugs",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStockClasses_StockClassId",
                table: "PharmaciesInStockClasses",
                column: "StockClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStockClasses_PharmacyId_StockClassId",
                table: "PharmaciesInStockClasses",
                columns: new[] { "PharmacyId", "StockClassId" },
                unique: true,
                filter: "[PharmacyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStocks_StockId",
                table: "PharmaciesInStocks",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                column: "StkDrugPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkDrugId_StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                columns: new[] { "StkDrugId", "StkDrugPackageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugPackagesRequests_PharmacyId",
                table: "StkDrugPackagesRequests",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugs_StockId",
                table: "StkDrugs",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StocksWithPharmaClasses_StockId_ClassName",
                table: "StocksWithPharmaClasses",
                columns: new[] { "StockId", "ClassName" },
                unique: true,
                filter: "[ClassName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_AdminId",
                table: "TechnicalSupportQuestions",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_AdminId1",
                table: "TechnicalSupportQuestions",
                column: "AdminId1");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_BaseCustomerId",
                table: "TechnicalSupportQuestions",
                column: "BaseCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_RelatedToId",
                table: "TechnicalSupportQuestions",
                column: "RelatedToId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminHistoryEntries");

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
                name: "BaseDrugs");

            migrationBuilder.DropTable(
                name: "Complains");

            migrationBuilder.DropTable(
                name: "LzDrugRequests");

            migrationBuilder.DropTable(
                name: "PharmaciesInStockClasses");

            migrationBuilder.DropTable(
                name: "PharmaciesInStocks");

            migrationBuilder.DropTable(
                name: "StkDrugInStkDrgPackagesRequests");

            migrationBuilder.DropTable(
                name: "TechnicalSupportQuestions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "LzDrugs");

            migrationBuilder.DropTable(
                name: "StocksWithPharmaClasses");

            migrationBuilder.DropTable(
                name: "StkDrugs");

            migrationBuilder.DropTable(
                name: "StkDrugPackagesRequests");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
