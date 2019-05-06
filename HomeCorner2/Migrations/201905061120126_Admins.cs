namespace HomeCorner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Admins : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            INSERT[dbo].[AspNetUsers]
            ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES(N'18265a1f-d879-4332-a545-5873747f3cd9', N'vpapoglou@gmail.com', 0, N'AJ9ICwdQw7YLurIw2bstpczlxkjTeKDzEnVI7+X5RpSb1ie0r0EgHjERZbe7idIsJg==', N'68a585e1-0c84-4da7-bcfd-d92d4b30e41f', NULL, 0, 0, NULL, 1, 0, N'vpapoglou@gmail.com')
            GO
            INSERT[dbo].[AspNetUsers]
            ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES(N'93eeec89-714b-462d-a97e-1c27857381c0', N'kate_admin@mail.com', 0, N'AIMxd9cQ0T71AJgp/0hf9+odV775nHZ+j94Tbk+ISyF+y4dST97Orm8DXYgILmqonw==', N'64c41e92-bce4-48fd-aa3e-5f83511df730', NULL, 0, 0, NULL, 1, 0, N'kate_admin@mail.com')
            GO  
            INSERT[dbo].[AspNetRoles]([Id], [Name]) VALUES(N'936aaba8-1a40-41cc-a450-f7f035b1bbbb', N'CanManageHouses')
            GO
            INSERT[dbo].[AspNetUserRoles]
            ([UserId], [RoleId]) VALUES(N'18265a1f-d879-4332-a545-5873747f3cd9', N'936aaba8-1a40-41cc-a450-f7f035b1bbbb')
            GO
            INSERT[dbo].[AspNetUserRoles]
            ([UserId], [RoleId]) VALUES(N'93eeec89-714b-462d-a97e-1c27857381c0', N'936aaba8-1a40-41cc-a450-f7f035b1bbbb')
            GO
            ");
        }
        
        public override void Down()
        {
        }
    }
}
