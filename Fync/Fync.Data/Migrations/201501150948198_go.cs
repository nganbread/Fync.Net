namespace Fync.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class go : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Folder",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        LastModified = c.DateTime(nullable: false),
                        ParentId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folder", t => t.ParentId)
                .Index(t => t.ParentId);

            SqlFile("SqlScripts/AddHierarchyNodeColumn.sql");

            CreateStoredProcedure(
                "dbo.Folder_Insert",
                p => new
                    {
                        Name = p.String(),
                        LastModified = p.DateTime(),
                        ParentId = p.Guid(),
                    },
                body:
                    @"DECLARE @generated_keys table([Id] uniqueidentifier)
                      INSERT [dbo].[Folder]([Name], [LastModified], [ParentId])
                      OUTPUT inserted.[Id] INTO @generated_keys
                      VALUES (@Name, @LastModified, @ParentId)
                      
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
                        Name = p.String(),
                        LastModified = p.DateTime(),
                        ParentId = p.Guid(),
                    },
                body:
                    @"UPDATE [dbo].[Folder]
                      SET [Name] = @Name, [LastModified] = @LastModified, [ParentId] = @ParentId
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

            SqlFile("SqlScripts/AlterFolderInsert.sql");
            SqlFile("SqlScripts/AlterFolderUpdate.sql");
            SqlFile("SqlScripts/AlterFolderDelete.sql");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Folder_Delete");
            DropStoredProcedure("dbo.Folder_Update");
            DropStoredProcedure("dbo.Folder_Insert");
            DropForeignKey("dbo.Folder", "ParentId", "dbo.Folder");
            DropIndex("dbo.Folder", new[] { "ParentId" });
            DropTable("dbo.Folder");
        }
    }
}
