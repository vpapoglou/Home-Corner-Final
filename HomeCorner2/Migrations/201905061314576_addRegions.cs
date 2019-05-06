namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRegions : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (0, 'Thessaloniki')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (1, 'Imathia')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (2, 'Kilkis')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (3, 'Pella')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (4, 'Pieria')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (5, 'Serres')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (6, 'Chalkidiki')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (7, 'Drama')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (8, 'Evros')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (9, 'Kavala')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (10, 'Rodopi')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (12, 'Xanthi')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (13, 'Florina')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (14, 'Grevena')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (15, 'Kastoria')");
            Sql("INSERT INTO Regions (RegionId, RegionName) VALUES (16, 'Kozani')");
        }
        
        public override void Down()
        {
        }
    }
}
