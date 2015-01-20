ALTER PROCEDURE [dbo].[Folder_Delete]
    @Id [uniqueidentifier],
    @ParentId [uniqueidentifier]
AS
BEGIN
    DELETE [dbo].[Folder]
    WHERE (([Id] = @Id) AND (([ParentId] = @ParentId) OR ([ParentId] IS NULL AND @ParentId IS NULL)))
END
GO


