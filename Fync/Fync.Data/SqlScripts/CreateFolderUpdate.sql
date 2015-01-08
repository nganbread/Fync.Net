ALTER PROCEDURE [dbo].[Folder_Update]
    @Id [uniqueidentifier],
    @Name [nvarchar](max),
    @LastModified [datetime],
    @Parent_Id [uniqueidentifier]
AS
BEGIN
    UPDATE [dbo].[Folder]
    SET [Name] = @Name, [LastModified] = @LastModified, [Parent_Id] = @Parent_Id
    WHERE ([Id] = @Id)
END
GO


