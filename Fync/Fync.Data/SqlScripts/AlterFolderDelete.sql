ALTER PROCEDURE [dbo].[Folder_Delete]
    @Id [uniqueidentifier]
AS
BEGIN
    DELETE [dbo].[Folder]
    WHERE (([Id] = @Id))
END
GO


