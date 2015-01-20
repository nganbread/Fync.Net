ALTER PROCEDURE [dbo].[Folder_Update]
    @Id [uniqueidentifier],
    @Name [nvarchar](max),
    @LastModified [datetime],
    @ParentId [uniqueidentifier]
AS
BEGIN
    UPDATE [dbo].[Folder]
    SET [Name] = @Name, [LastModified] = @LastModified, [ParentId] = @ParentId
    WHERE ([Id] = @Id)
END
GO


