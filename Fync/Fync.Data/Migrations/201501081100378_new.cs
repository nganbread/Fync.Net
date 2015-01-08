namespace Fync.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
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
                        Parent_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folder", t => t.Parent_Id)
                .Index(t => t.Parent_Id);

            SqlFile("SqlScripts/AddHierarchyNodeColumn.sql");


            CreateStoredProcedure(
                "dbo.Folder_Insert",
                p => new
                    {
                        Name = p.String(),
                        LastModified = p.DateTime(),
                        Parent_Id = p.Guid(),
                    },
                body:
                    @"DECLARE @generated_keys table([Id] uniqueidentifier)
                      INSERT [dbo].[Folder]([Name], [LastModified], [Parent_Id])
                      OUTPUT inserted.[Id] INTO @generated_keys
                      VALUES (@Name, @LastModified, @Parent_Id)
                      
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
                        Parent_Id = p.Guid(),
                    },
                body:
                    @"UPDATE [dbo].[Folder]
                      SET [Name] = @Name, [LastModified] = @LastModified, [Parent_Id] = @Parent_Id
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


            SqlFile("SqlScripts/CreateFolderInsert.sql");
            SqlFile("SqlScripts/CreateFolderUpdate.sql");
            SqlFile("SqlScripts/CreateFolderDelete.sql");



        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Folder_Delete");
            DropStoredProcedure("dbo.Folder_Update");
            DropStoredProcedure("dbo.Folder_Insert");
            DropForeignKey("dbo.Folder", "Parent_Id", "dbo.Folder");
            DropIndex("dbo.Folder", new[] { "Parent_Id" });
            DropTable("dbo.Folder");
        }
    }
}
