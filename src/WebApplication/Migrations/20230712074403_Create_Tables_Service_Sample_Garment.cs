using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Create_Tables_Service_Sample_Garment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleCuttings",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SampleNo = table.Column<string>(maxLength: 25, nullable: true),
                    SampleType = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    SampleDate = table.Column<DateTimeOffset>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(nullable: true),
                    BuyerName = table.Column<string>(nullable: true),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(nullable: true),
                    QtyPacking = table.Column<int>(nullable: false),
                    NettWeight = table.Column<double>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleCuttings", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleExpenditureGoods",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleExpenditureGoodNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSampleExpenditureGoodDate = table.Column<DateTimeOffset>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(nullable: true),
                    BuyerName = table.Column<string>(nullable: true),
                    QtyPacking = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(nullable: true),
                    NettWeight = table.Column<double>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleExpenditureGoods", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleFabricWashes",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleFabricWashNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSampleFabricWashDate = table.Column<DateTimeOffset>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    IsUsed = table.Column<bool>(nullable: false),
                    QtyPacking = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(nullable: true),
                    NettWeight = table.Column<double>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleFabricWashes", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleSewings",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleSewingNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSampleSewingDate = table.Column<DateTimeOffset>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(nullable: true),
                    BuyerName = table.Column<string>(nullable: true),
                    QtyPacking = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(nullable: true),
                    NettWeight = table.Column<double>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleSewings", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleShrinkagePanels",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleShrinkagePanelNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSampleShrinkagePanelDate = table.Column<DateTimeOffset>(nullable: false),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    IsUsed = table.Column<bool>(nullable: false),
                    QtyPacking = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(nullable: true),
                    NettWeight = table.Column<double>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleShrinkagePanels", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleCuttingItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 255, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 500, nullable: true),
                    ServiceSampleCuttingId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleCuttingItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleCuttingItems_GarmentServiceSampleCuttings_ServiceSampleCuttingId",
                        column: x => x.ServiceSampleCuttingId,
                        principalTable: "GarmentServiceSampleCuttings",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleExpenditureGoodItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleExpenditureGoodId = table.Column<Guid>(nullable: false),
                    FinishedGoodStockId = table.Column<Guid>(nullable: false),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleExpenditureGoodItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleExpenditureGoodItems_GarmentServiceSampleExpenditureGoods_ServiceSampleExpenditureGoodId",
                        column: x => x.ServiceSampleExpenditureGoodId,
                        principalTable: "GarmentServiceSampleExpenditureGoods",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleFabricWashItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleFabricWashId = table.Column<Guid>(nullable: false),
                    UnitExpenditureNo = table.Column<string>(maxLength: 25, nullable: true),
                    ExpenditureDate = table.Column<DateTimeOffset>(nullable: false),
                    UnitSenderId = table.Column<int>(nullable: false),
                    UnitSenderCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitSenderName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitRequestId = table.Column<int>(nullable: false),
                    UnitRequestCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitRequestName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleFabricWashItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleFabricWashItems_GarmentServiceSampleFabricWashes_ServiceSampleFabricWashId",
                        column: x => x.ServiceSampleFabricWashId,
                        principalTable: "GarmentServiceSampleFabricWashes",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleSewingItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleSewingId = table.Column<Guid>(nullable: false),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 25, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(nullable: true),
                    UnitName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleSewingItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleSewingItems_GarmentServiceSampleSewings_ServiceSampleSewingId",
                        column: x => x.ServiceSampleSewingId,
                        principalTable: "GarmentServiceSampleSewings",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleShrinkagePanelItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleShrinkagePanelId = table.Column<Guid>(nullable: false),
                    UnitExpenditureNo = table.Column<string>(maxLength: 25, nullable: true),
                    ExpenditureDate = table.Column<DateTimeOffset>(nullable: false),
                    UnitSenderId = table.Column<int>(nullable: false),
                    UnitSenderCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitSenderName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitRequestId = table.Column<int>(nullable: false),
                    UnitRequestCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitRequestName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleShrinkagePanelItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleShrinkagePanelItems_GarmentServiceSampleShrinkagePanels_ServiceSampleShrinkagePanelId",
                        column: x => x.ServiceSampleShrinkagePanelId,
                        principalTable: "GarmentServiceSampleShrinkagePanels",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleCuttingDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    ServiceSampleCuttingItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleCuttingDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleCuttingDetails_GarmentServiceSampleCuttingItems_ServiceSampleCuttingItemId",
                        column: x => x.ServiceSampleCuttingItemId,
                        principalTable: "GarmentServiceSampleCuttingItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleFabricWashDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleFabricWashItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductRemark = table.Column<string>(maxLength: 1000, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(maxLength: 100, nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleFabricWashDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleFabricWashDetails_GarmentServiceSampleFabricWashItems_ServiceSampleFabricWashItemId",
                        column: x => x.ServiceSampleFabricWashItemId,
                        principalTable: "GarmentServiceSampleFabricWashItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleSewingDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleSewingItemId = table.Column<Guid>(nullable: false),
                    SewingInId = table.Column<Guid>(nullable: false),
                    SewingInItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 2000, nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleSewingDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleSewingDetails_GarmentServiceSampleSewingItems_ServiceSampleSewingItemId",
                        column: x => x.ServiceSampleSewingItemId,
                        principalTable: "GarmentServiceSampleSewingItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleShrinkagePanelDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ServiceSampleShrinkagePanelItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductRemark = table.Column<string>(maxLength: 1000, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleShrinkagePanelDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleShrinkagePanelDetails_GarmentServiceSampleShrinkagePanelItems_ServiceSampleShrinkagePanelItemId",
                        column: x => x.ServiceSampleShrinkagePanelItemId,
                        principalTable: "GarmentServiceSampleShrinkagePanelItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSampleCuttingSizes",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    CuttingInId = table.Column<Guid>(nullable: false),
                    CuttingInDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Color = table.Column<string>(maxLength: 2000, nullable: true),
                    ServiceSampleCuttingDetailId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSampleCuttingSizes", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSampleCuttingSizes_GarmentServiceSampleCuttingDetails_ServiceSampleCuttingDetailId",
                        column: x => x.ServiceSampleCuttingDetailId,
                        principalTable: "GarmentServiceSampleCuttingDetails",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleCuttingDetails_ServiceSampleCuttingItemId",
                table: "GarmentServiceSampleCuttingDetails",
                column: "ServiceSampleCuttingItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleCuttingItems_ServiceSampleCuttingId",
                table: "GarmentServiceSampleCuttingItems",
                column: "ServiceSampleCuttingId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleCuttings_SampleNo",
                table: "GarmentServiceSampleCuttings",
                column: "SampleNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleCuttingSizes_ServiceSampleCuttingDetailId",
                table: "GarmentServiceSampleCuttingSizes",
                column: "ServiceSampleCuttingDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleExpenditureGoodItems_ServiceSampleExpenditureGoodId",
                table: "GarmentServiceSampleExpenditureGoodItems",
                column: "ServiceSampleExpenditureGoodId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleExpenditureGoods_ServiceSampleExpenditureGoodNo",
                table: "GarmentServiceSampleExpenditureGoods",
                column: "ServiceSampleExpenditureGoodNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleFabricWashDetails_ServiceSampleFabricWashItemId",
                table: "GarmentServiceSampleFabricWashDetails",
                column: "ServiceSampleFabricWashItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleFabricWashes_ServiceSampleFabricWashNo",
                table: "GarmentServiceSampleFabricWashes",
                column: "ServiceSampleFabricWashNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleFabricWashItems_ServiceSampleFabricWashId",
                table: "GarmentServiceSampleFabricWashItems",
                column: "ServiceSampleFabricWashId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleSewingDetails_ServiceSampleSewingItemId",
                table: "GarmentServiceSampleSewingDetails",
                column: "ServiceSampleSewingItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleSewingItems_ServiceSampleSewingId",
                table: "GarmentServiceSampleSewingItems",
                column: "ServiceSampleSewingId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleSewings_ServiceSampleSewingNo",
                table: "GarmentServiceSampleSewings",
                column: "ServiceSampleSewingNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleShrinkagePanelDetails_ServiceSampleShrinkagePanelItemId",
                table: "GarmentServiceSampleShrinkagePanelDetails",
                column: "ServiceSampleShrinkagePanelItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleShrinkagePanelItems_ServiceSampleShrinkagePanelId",
                table: "GarmentServiceSampleShrinkagePanelItems",
                column: "ServiceSampleShrinkagePanelId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSampleShrinkagePanels_ServiceSampleShrinkagePanelNo",
                table: "GarmentServiceSampleShrinkagePanels",
                column: "ServiceSampleShrinkagePanelNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentServiceSampleCuttingSizes");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleExpenditureGoodItems");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleFabricWashDetails");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleSewingDetails");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleShrinkagePanelDetails");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleCuttingDetails");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleExpenditureGoods");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleFabricWashItems");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleSewingItems");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleShrinkagePanelItems");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleCuttingItems");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleFabricWashes");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleSewings");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleShrinkagePanels");

            migrationBuilder.DropTable(
                name: "GarmentServiceSampleCuttings");
        }
    }
}
