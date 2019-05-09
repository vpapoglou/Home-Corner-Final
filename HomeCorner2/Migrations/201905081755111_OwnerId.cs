namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnerId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Houses", "OwnerId", "dbo.Customers");
            DropIndex("dbo.Houses", new[] { "OwnerId" });
            AddColumn("dbo.Houses", "Customer_Id", c => c.Int());
            AlterColumn("dbo.Houses", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Houses", "OwnerId");
            CreateIndex("dbo.Houses", "Customer_Id");
            AddForeignKey("dbo.Houses", "Customer_Id", "dbo.Customers", "Id");
            AddForeignKey("dbo.Houses", "OwnerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Houses", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Houses", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Houses", new[] { "Customer_Id" });
            DropIndex("dbo.Houses", new[] { "OwnerId" });
            AlterColumn("dbo.Houses", "OwnerId", c => c.Int(nullable: false));
            DropColumn("dbo.Houses", "Customer_Id");
            CreateIndex("dbo.Houses", "OwnerId");
            AddForeignKey("dbo.Houses", "OwnerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
    }
}
