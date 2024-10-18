using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MobileStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ram = table.Column<int>(type: "int", nullable: false),
                    Hard = table.Column<int>(type: "int", nullable: false),
                    Camera = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Galleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Galleries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductToShops",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductToShops", x => new { x.ProductId, x.ShopId });
                    table.ForeignKey(
                        name: "FK_ProductToShops_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductToShops_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "CreatedAt", "ImageFileName", "IsActive", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(1453), "samsung.jpg", true, "سامسونگ", "samsung" },
                    { 2, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(1466), "nokia.jpg", true, "نوکیا", "nokia" },
                    { 3, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(1468), "xiaomi.jpg", true, "شیائومی", "xiaomi" },
                    { 4, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(1470), "apple.jpg", true, "اپل", "apple" },
                    { 5, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(1472), "huawei.jpg", true, "هوآوی", "huawei" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2010), "admin@gmail.com", false, "ادمین", "81-DC-9B-DB-52-D0-4D-C2-00-36-DB-D8-31-3E-D0-55", "admin" },
                    { 2, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2058), "ali@gmail.com", false, "علی", "81-DC-9B-DB-52-D0-4D-C2-00-36-DB-D8-31-3E-D0-55", "seller" },
                    { 3, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2083), "tara@gmail.com", false, "تارا", "81-DC-9B-DB-52-D0-4D-C2-00-36-DB-D8-31-3E-D0-55", "client" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "Camera", "CreatedAt", "Description", "Hard", "Ram", "Title" },
                values: new object[,]
                {
                    { 1, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2133), "", 64, 4, "گوشی موبایل سامسونگ مدل Galaxy A14" },
                    { 2, 3, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2138), "", 256, 8, "گوشی موبایل شیائومی مدل Redmi 12" },
                    { 3, 5, 48, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2141), "", 128, 8, "گوشی موبایل هوآوی مدل nova Y71" },
                    { 4, 3, 108, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2144), "", 256, 8, "گوشی موبایل شیائومی مدل Redmi Note 13 4G" },
                    { 5, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2219), "", 64, 4, "گوشی موبایل سامسونگ مدل Galaxy A05" },
                    { 6, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2223), "", 64, 4, "گوشی موبایل سامسونگ مدل Galaxy M13" },
                    { 7, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2226), "", 128, 4, "گوشی موبایل سامسونگ مدل Galaxy A15" },
                    { 8, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2230), "", 128, 6, "گوشی موبایل سامسونگ مدل Galaxy A05s" },
                    { 9, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2233), "", 64, 4, "گوشی موبایل سامسونگ مدل Galaxy A23 " },
                    { 10, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2237), "", 128, 6, "گوشی موبایل سامسونگ مدل Galaxy A25" },
                    { 11, 1, 48, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2241), "", 128, 8, "گوشی موبایل سامسونگ مدل Galaxy A34 5G " },
                    { 12, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2244), "", 128, 8, "گوشی موبایل سامسونگ مدل Galaxy A35" },
                    { 13, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2247), "", 128, 8, "گوشی موبایل سامسونگ مدل Galaxy A54 5G" },
                    { 14, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2251), "", 256, 8, "گوشی موبایل سامسونگ مدل Galaxy A55" },
                    { 15, 1, 12, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2254), "", 256, 8, "گوشی موبایل سامسونگ مدل Galaxy S21 FE 5G" },
                    { 16, 1, 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2257), "", 256, 8, "گوشی موبایل سامسونگ مدل Galaxy S23 FE" },
                    { 17, 1, 200, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2260), "", 256, 12, "گوشی موبایل سامسونگ مدل Galaxy S23 Ultra" },
                    { 18, 1, 200, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2264), "", 512, 12, "گوشی موبایل سامسونگ مدل Galaxy S24 Ultra" },
                    { 19, 3, 108, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2267), "", 256, 8, "گوشی موبایل شیائومی مدل Redmi Note 12S" },
                    { 20, 3, 108, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2270), "", 256, 8, "گوشی موبایل شیائومی مدل Redmi Note 13 5G" },
                    { 21, 3, 108, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2274), "", 256, 12, "گوشی موبایل شیائومی مدل Redmi Note 12 Pro 5G" },
                    { 22, 3, 64, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2277), "", 256, 12, "گوشی موبایل شیائومی مدل Redmi Note 12 Turbo 5G" },
                    { 23, 3, 64, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2280), "", 256, 12, "گوشی موبایل شیائومی مدل Poco F5 5G" }
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreatedAt", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(1703), "موبایل404", 1 },
                    { 2, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(1706), "علی شاپ", 2 }
                });

            migrationBuilder.InsertData(
                table: "Galleries",
                columns: new[] { "Id", "CreatedAt", "ImageFileName", "ProductId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2322), "samsung-a14-1.jpg", 1 },
                    { 2, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2325), "samsung-a14-2.jpg", 1 },
                    { 3, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2327), "huawei-nova-y71-1.jpg", 3 },
                    { 4, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2328), "huawei-nova-y71-2.jpg", 3 },
                    { 5, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2330), "huawei-nova-y71-3.jpg", 3 },
                    { 6, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2331), "xiaomi-redmi-note-13-1.jpg", 4 },
                    { 7, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2333), "Galaxy-A05-1.jpg", 5 },
                    { 8, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2334), "Galaxy-A05-2.jpg", 5 },
                    { 9, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2335), "Galaxy-M13-1.jpg", 6 },
                    { 10, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2337), "Galaxy-M13-2.jpg", 6 },
                    { 11, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2338), "Galaxy-M13-3.jpg", 6 },
                    { 12, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2339), "Galaxy-A15-1.jpg", 7 },
                    { 13, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2341), "Galaxy-A15-2.jpg", 7 },
                    { 14, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2342), "Galaxy-A15-3.jpg", 7 },
                    { 15, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2343), "Galaxy-A05s-1.jpg", 8 },
                    { 16, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2344), "Galaxy-A05s-2.jpg", 8 },
                    { 17, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2346), "Galaxy-A05s-3.jpg", 8 },
                    { 18, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2347), "Galaxy-A23-1.jpg", 9 },
                    { 19, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2348), "Galaxy-A23-2.jpg", 9 },
                    { 20, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2349), "Galaxy-A23-3.jpg", 9 },
                    { 21, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2351), "Galaxy-A25-1.jpg", 10 },
                    { 22, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2352), "Galaxy-A25-2.jpg", 10 },
                    { 23, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2353), "Galaxy-A25-3.jpg", 10 },
                    { 24, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2354), "Galaxy-A34-1.jpg", 11 },
                    { 25, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2356), "Galaxy-A34-2.jpg", 11 },
                    { 26, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2357), "Galaxy-A35-1.jpg", 12 },
                    { 27, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2358), "Galaxy-A35-2.jpg", 12 },
                    { 28, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2359), "Galaxy-A54-1.jpg", 13 },
                    { 29, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2361), "Galaxy-A54-2.jpg", 13 },
                    { 30, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2363), "Galaxy-A55-1.jpg", 14 },
                    { 31, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2364), "Galaxy-A55-2.jpg", 14 },
                    { 32, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2365), "Galaxy-A55-3.jpg", 14 },
                    { 33, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2370), "Galaxy-S21-1.jpg", 15 },
                    { 34, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2371), "Galaxy-S23-1.jpg", 16 },
                    { 35, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2373), "Galaxy-S23-2.jpg", 16 },
                    { 36, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2374), "Galaxy-S23-Ultra-1.jpg", 17 },
                    { 37, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2376), "Galaxy-S24-Ultra-1.jpg", 18 },
                    { 38, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2377), "Galaxy-S24-Ultra-2.jpg", 18 },
                    { 39, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2378), "Galaxy-S24-Ultra-3.jpg", 18 },
                    { 40, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2380), "Redmi-Note-12S-1.jpg", 19 },
                    { 41, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2381), "Redmi-Note-12S-2.jpg", 19 },
                    { 42, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2383), "Redmi-Note-13-1.jpg", 20 },
                    { 43, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2384), "Redmi-Note-13-2.jpg", 20 },
                    { 44, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2385), "Redmi-Note-13-3.jpg", 20 },
                    { 45, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2387), "Redmi-Note-12-1.jpg", 21 },
                    { 46, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2388), "Redmi-Note-12-Turbo-1.jpg", 22 },
                    { 47, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2390), "Redmi-Note-12-Turbo-2.jpg", 22 },
                    { 48, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2391), "Redmi-Note-12-Turbo-3.jpg", 22 },
                    { 49, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2393), "Poco-F5-1.jpg", 23 },
                    { 50, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2394), "Poco-F5-2.jpg", 23 },
                    { 51, new DateTime(2024, 10, 13, 12, 7, 14, 416, DateTimeKind.Local).AddTicks(2395), "Poco-F5-3.jpg", 23 }
                });

            migrationBuilder.InsertData(
                table: "ProductToShops",
                columns: new[] { "ProductId", "ShopId", "Price" },
                values: new object[,]
                {
                    { 1, 1, 6000000 },
                    { 2, 1, 6900000 },
                    { 2, 2, 6950000 },
                    { 3, 1, 6200000 },
                    { 4, 1, 8840000 },
                    { 5, 1, 8740000 },
                    { 6, 1, 5699000 },
                    { 7, 1, 6079000 },
                    { 8, 1, 6470000 },
                    { 9, 1, 7399000 },
                    { 10, 1, 9549000 },
                    { 10, 2, 9549000 },
                    { 11, 1, 12999000 },
                    { 12, 1, 15287000 },
                    { 13, 1, 16990000 },
                    { 14, 1, 20980000 },
                    { 15, 1, 21150000 },
                    { 16, 1, 25850000 },
                    { 17, 1, 57950000 },
                    { 19, 1, 8530000 },
                    { 20, 1, 11540000 },
                    { 20, 2, 11440000 },
                    { 21, 1, 12600000 },
                    { 22, 1, 18799000 },
                    { 23, 1, 17999000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Slug",
                table: "Brands",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductId",
                table: "Comments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_ProductId",
                table: "Galleries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopId",
                table: "Orders",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductToShops_ShopId",
                table: "ProductToShops",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_UserId",
                table: "Shops",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Galleries");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductToShops");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
