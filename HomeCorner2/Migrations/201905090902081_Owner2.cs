namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Owner2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Houses", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Houses", new[] { "OwnerId" });
            AddColumn("dbo.Houses", "Owner_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Houses", "OwnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Houses", "Owner_Id");
            AddForeignKey("dbo.Houses", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Houses", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Houses", new[] { "Owner_Id" });
            AlterColumn("dbo.Houses", "OwnerId", c => c.String(maxLength: 128));
            DropColumn("dbo.Houses", "Owner_Id");
            CreateIndex("dbo.Houses", "OwnerId");
            AddForeignKey("dbo.Houses", "OwnerId", "dbo.AspNetUsers", "Id");
        }
    }
}
