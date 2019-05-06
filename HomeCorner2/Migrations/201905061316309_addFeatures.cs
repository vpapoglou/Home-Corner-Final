namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFeatures : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Features (Id, Feature) VALUES (0, 'Garden')");
            Sql("INSERT INTO Features (Id, Feature) VALUES (1, 'Pool')");
            Sql("INSERT INTO Features (Id, Feature) VALUES (2, 'Parking')");
            Sql("INSERT INTO Features (Id, Feature) VALUES (3, 'View to the Sea')");
            Sql("INSERT INTO Features (Id, Feature) VALUES (5, 'View to the Mountain')");
            Sql("INSERT INTO Features (Id, Feature) VALUES (6, 'Sound Insulation')");
            Sql("INSERT INTO Features (Id, Feature) VALUES (7, 'Air Conditioning')");
        }
        
        public override void Down()
        {
        }
    }
}
