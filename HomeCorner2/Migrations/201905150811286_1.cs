namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Houses", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Houses", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Houses", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Houses", "AddressNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Houses", "PostalCode", c => c.String(nullable: false, maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Houses", "PostalCode", c => c.Int(nullable: false));
            AlterColumn("dbo.Houses", "AddressNumber", c => c.String());
            AlterColumn("dbo.Houses", "Address", c => c.String());
            AlterColumn("dbo.Houses", "Description", c => c.String());
            AlterColumn("dbo.Houses", "Title", c => c.String());
        }
    }
}
