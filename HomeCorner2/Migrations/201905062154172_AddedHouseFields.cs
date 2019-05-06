namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHouseFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Houses", "AddressNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Houses", "AddressNumber");
        }
    }
}
