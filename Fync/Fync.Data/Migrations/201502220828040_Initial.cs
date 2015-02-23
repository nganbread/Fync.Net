namespace Fync.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Folder",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        ParentId = c.Guid(),
                        Name = c.String(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .ForeignKey("dbo.Folder", t => t.ParentId)
                .Index(t => t.OwnerId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RootFolderId = c.Guid(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateStoredProcedure(
                "dbo.Folder_Insert",
                p => new
                    {
                        OwnerId = p.Int(),
                        ParentId = p.Guid(),
                        Name = p.String(),
                        ModifiedDate = p.DateTime(),
                        Deleted = p.Boolean(),
                    },
                body:
                    @"DECLARE @generated_keys table([Id] uniqueidentifier)
                      INSERT [dbo].[Folder]([OwnerId], [ParentId], [Name], [ModifiedDate], [Deleted])
                      OUTPUT inserted.[Id] INTO @generated_keys
                      VALUES (@OwnerId, @ParentId, @Name, @ModifiedDate, @Deleted)
                      
                      DECLARE @Id uniqueidentifier
                      SELECT @Id = t.[Id]
                      FROM @generated_keys AS g JOIN [dbo].[Folder] AS t ON g.[Id] = t.[Id]
                      WHERE @@ROWCOUNT > 0
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Folder] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Folder_Update",
                p => new
                    {
                        Id = p.Guid(),
                        OwnerId = p.Int(),
                        ParentId = p.Guid(),
                        Name = p.String(),
                        ModifiedDate = p.DateTime(),
                        Deleted = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Folder]
                      SET [OwnerId] = @OwnerId, [ParentId] = @ParentId, [Name] = @Name, [ModifiedDate] = @ModifiedDate, [Deleted] = @Deleted
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Folder_Delete",
                p => new
                    {
                        Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[Folder]
                      WHERE ([Id] = @Id)"
            );

            SqlResource("Fync.Data.SqlScripts.AddHierarchyNodeColumn.sql");
            SqlResource("Fync.Data.SqlScripts.AlterFolderInsert.sql");
            SqlResource("Fync.Data.SqlScripts.AlterFolderUpdate.sql");
            SqlResource("Fync.Data.SqlScripts.AlterFolderDelete.sql");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Folder_Delete");
            DropStoredProcedure("dbo.Folder_Update");
            DropStoredProcedure("dbo.Folder_Insert");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Folder", "ParentId", "dbo.Folder");
            DropForeignKey("dbo.Folder", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Folder", new[] { "ParentId" });
            DropIndex("dbo.Folder", new[] { "OwnerId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Folder");
        }
    }
}
