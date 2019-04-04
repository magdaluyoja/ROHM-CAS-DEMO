USE [ROHM_CAS_DEMO]
GO
/****** Object:  StoredProcedure [dbo].[INSERT_Modulemaster]    Script Date: 3/27/2019 2:41:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[INSERT_Modulemaster](
	@UserID			VARCHAR(50)
   ,@ModuleID		Int
   ,@CreateID		VARCHAR(50)
   ,@UpdateID		VARCHAR(50)
  
   ,@Return			Bit OUTPUT
   ,@Message		VARCHAR(50) OUTPUT
) AS
BEGIN TRANSACTION
	SET NOCOUNT ON
		BEGIN
			INSERT INTO [dbo].[mUserAccessModules](
						UserID
					,	ModuleID				
					,	IsDeleted
					,	CreateID
					,	CreateDate
					,	UpdateID
					,	UpdateDate
				)
				SELECT
					@UserID, 
					@ModuleID,								
					0,
					@CreateID,
					GETDATE(),
					@UpdateID,
					GETDATE()
			SET @Return = 1
			SET @Message = 'Successfully Created!'
		END	
COMMIT TRANSACTION
