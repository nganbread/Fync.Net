ALTER PROCEDURE [dbo].[Folder_Insert]
    @Name [nvarchar](max),
    @LastModified [datetime],
    @ParentId [uniqueidentifier]
AS
BEGIN
	
	DECLARE @generated_keys table([Id] uniqueidentifier)

	IF(@ParentId IS NULL)
		BEGIN
		
			INSERT [dbo].[Folder]([Name], [LastModified], [ParentId], [HierarchyNode])
			OUTPUT inserted.[Id] INTO @generated_keys
			VALUES (@Name, @LastModified, @ParentId, hierarchyid::GetRoot())
        
		END
	ELSE
		BEGIN
			-- get the parent node
			DECLARE @parentHierarchyNode hierarchyid
			SELECT @parentHierarchyNode = [HierarchyNode]
			FROM [dbo].[Folder]
			WHERE [Id] = @ParentId

			-- find the largest child of the parent
			DECLARE @largestChildHierarchyNode hierarchyid
			SELECT @largestChildHierarchyNode = Max([HierarchyNode])
			FROM [dbo].[Folder]
			WHERE [HierarchyNode].GetAncestor(1) = @parentHierarchyNode

			INSERT [dbo].[Folder]([Name], [LastModified], [ParentId], [HierarchyNode])
			OUTPUT inserted.[Id] INTO @generated_keys
			VALUES (@Name, @LastModified, @ParentId, @parentHierarchyNode.GetDescendant(@largestChildHierarchyNode, NULL))
    	
		END
	                  
    DECLARE @Id uniqueidentifier
    SELECT @Id = t.[Id]
    FROM @generated_keys AS g JOIN [dbo].[Folder] AS t ON g.[Id] = t.[Id]
    WHERE @@ROWCOUNT > 0
                      
    SELECT t0.[Id]
    FROM [dbo].[Folder] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO


