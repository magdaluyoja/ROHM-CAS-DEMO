USE [ROHM_CAS_DEMO]
GO
/****** Object:  StoredProcedure [dbo].[GET_UserData]    Script Date: 3/28/2019 9:27:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================

-- =============================================
CREATE PROCEDURE [dbo].[GET_UserDetails](
	 @ID Int
) AS
BEGIN TRANSACTION

	SET NOCOUNT ON
		DECLARE @query VARCHAR(200)

		BEGIN
			SELECT @OldUserID = UserID, @OldPassword = Password FROM UserMaster WHERE ID = @ID AND IsDeleted = 0
		END

		EXECUTE(@query)

COMMIT TRANSACTION

