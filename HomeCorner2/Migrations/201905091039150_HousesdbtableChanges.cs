namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HousesdbtableChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Houses", new[] { "Owner_Id" });
            DropColumn("dbo.Houses", "OwnerId");
            RenameColumn(table: "dbo.Houses", name: "Owner_Id", newName: "OwnerId");
            AlterColumn("dbo.Houses", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Houses", "OwnerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Houses", new[] { "OwnerId" });
            AlterColumn("dbo.Houses", "OwnerId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Houses", name: "OwnerId", newName: "Owner_Id");
            AddColumn("dbo.Houses", "OwnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Houses", "Owner_Id");
        }
    }
}
