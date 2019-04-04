USE [ROHM_CAS_DEMO]
GO
/****** Object:  StoredProcedure [dbo].[DELETE_UserData]    Script Date: 3/28/2019 4:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================

-- =============================================
CREATE PROCEDURE [dbo].[DELETE_UserData](
	@ID				Int
   ,@UpdateID		VARCHAR(20)
   ,@UpdateDate		DATETIME
   ,@Return			Bit OUTPUT
   ,@Message		VARCHAR(50) OUTPUT
) AS
BEGIN TRANSACTION
	SET NOCOUNT ON
		BEGIN
			UPDATE [dbo].[User]
				SET IsDeleted = 1,
					UpdateID = @UpdateID,
					UpdateDate = @UpdateDate
				WHERE ID = @ID
			SET @Return = 1
			SET @Message = 'User was successfully deleted!';
		END
COMMIT TRANSACTION
