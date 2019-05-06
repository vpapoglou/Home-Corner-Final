namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UploadPhotos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Houses", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Houses", "ImageName");
        }
    }
}
