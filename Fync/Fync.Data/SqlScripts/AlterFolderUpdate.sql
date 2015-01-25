ALTER PROCEDURE [dbo].[Folder_Update]
    @Id [uniqueidentifier],
    @Name [nvarchar](max),
    @DateCreated [datetime],
    @ParentId [uniqueidentifier]
AS
BEGIN
    UPDATE [dbo].[Folder]
    SET [Name] = @Name, [DateCreated] = @DateCreated, [ParentId] = @ParentId
    WHERE ([Id] = @Id)
END
GO


