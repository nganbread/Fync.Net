ALTER PROCEDURE [dbo].[Folder_Delete]
    @Id [uniqueidentifier],
    @Parent_Id [uniqueidentifier]
AS
BEGIN
    DELETE [dbo].[Folder]
    WHERE (([Id] = @Id) AND (([Parent_Id] = @Parent_Id) OR ([Parent_Id] IS NULL AND @Parent_Id IS NULL)))
END
GO


