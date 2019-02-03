namespace Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        TaxRate = c.Decimal(nullable: false, precision: 4, scale: 2),
                        SupCategoryId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.SupCategoryId)
                .Index(t => t.SupCategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductName = c.String(nullable: false, maxLength: 100),
                        ProductType = c.Int(nullable: false),
                        SalesPrice = c.Decimal(nullable: false, precision: 7, scale: 2),
                        BuyPrice = c.Decimal(nullable: false, precision: 7, scale: 2),
                        UnitInStock = c.Double(nullable: false),
                        LastPriceUpdateDate = c.DateTime(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        SupProductId = c.Guid(),
                        Barcode = c.String(nullable: false, maxLength: 20),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.SupProductId)
                .Index(t => t.CategoryId)
                .Index(t => t.SupProductId)
                .Index(t => t.Barcode, unique: true);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Id2 = c.Guid(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 8, scale: 4),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 3, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.Id2 })
                .ForeignKey("dbo.Orders", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Id2, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Id2);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupProductId", "dbo.Products");
            DropForeignKey("dbo.Invoices", "Id2", "dbo.Products");
            DropForeignKey("dbo.Invoices", "Id", "dbo.Orders");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "SupCategoryId", "dbo.Categories");
            DropIndex("dbo.Invoices", new[] { "Id2" });
            DropIndex("dbo.Invoices", new[] { "Id" });
            DropIndex("dbo.Products", new[] { "Barcode" });
            DropIndex("dbo.Products", new[] { "SupProductId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Categories", new[] { "SupCategoryId" });
            DropTable("dbo.Orders");
            DropTable("dbo.Invoices");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
