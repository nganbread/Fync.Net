ALTER PROCEDURE [dbo].[Folder_Update]
    @Id [uniqueidentifier],
    @Name [nvarchar](max),
    @ModifiedDate [datetime],
    @ParentId [uniqueidentifier],
	@OwnerId [int],
	@Deleted [bit]
AS
BEGIN
    UPDATE [dbo].[Folder]
    SET 
		[Name] = @Name, 
		[ModifiedDate] = @ModifiedDate, 
		[ParentId] = @ParentId,
		[OwnerId] = @OwnerId,
		[Deleted] = @Deleted
    WHERE ([Id] = @Id)
END
GO


