namespace Fync.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class root : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Folder", new[] { "ParentId" });
            CreateTable(
                "dbo.RootFolder",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Folder", "RootFolderEntity_Id", c => c.Guid());
            AlterColumn("dbo.Folder", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Folder", "ParentId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Folder", "ParentId");
            CreateIndex("dbo.Folder", "RootFolderEntity_Id");
            AddForeignKey("dbo.Folder", "RootFolderEntity_Id", "dbo.RootFolder", "Id");
            AlterStoredProcedure(
                "dbo.Folder_Insert",
                p => new
                    {
                        ParentId = p.Guid(),
                        Name = p.String(),
                        LastModified = p.DateTime(),
                        RootFolderEntity_Id = p.Guid(),
                    },
                body:
                    @"DECLARE @generated_keys table([Id] uniqueidentifier)
                      INSERT [dbo].[Folder]([ParentId], [Name], [LastModified], [RootFolderEntity_Id])
                      OUTPUT inserted.[Id] INTO @generated_keys
                      VALUES (@ParentId, @Name, @LastModified, @RootFolderEntity_Id)
                      
                      DECLARE @Id uniqueidentifier
                      SELECT @Id = t.[Id]
                      FROM @generated_keys AS g JOIN [dbo].[Folder] AS t ON g.[Id] = t.[Id]
                      WHERE @@ROWCOUNT > 0
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Folder] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Folder_Update",
                p => new
                    {
                        Id = p.Guid(),
                        ParentId = p.Guid(),
                        Name = p.String(),
                        LastModified = p.DateTime(),
                        RootFolderEntity_Id = p.Guid(),
                    },
                body:
                    @"UPDATE [dbo].[Folder]
                      SET [ParentId] = @ParentId, [Name] = @Name, [LastModified] = @LastModified, [RootFolderEntity_Id] = @RootFolderEntity_Id
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Folder_Delete",
                p => new
                    {
                        Id = p.Guid(),
                        RootFolderEntity_Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[Folder]
                      WHERE (([Id] = @Id) AND (([RootFolderEntity_Id] = @RootFolderEntity_Id) OR ([RootFolderEntity_Id] IS NULL AND @RootFolderEntity_Id IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Folder", "RootFolderEntity_Id", "dbo.RootFolder");
            DropIndex("dbo.Folder", new[] { "RootFolderEntity_Id" });
            DropIndex("dbo.Folder", new[] { "ParentId" });
            AlterColumn("dbo.Folder", "ParentId", c => c.Guid());
            AlterColumn("dbo.Folder", "Name", c => c.String());
            DropColumn("dbo.Folder", "RootFolderEntity_Id");
            DropTable("dbo.RootFolder");
            CreateIndex("dbo.Folder", "ParentId");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
