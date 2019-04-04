USE [ROHM_CAS_DEMO]
GO
/****** Object:  StoredProcedure [dbo].[UPDATE_UserData]    Script Date: 3/28/2019 3:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UPDATE_UserData](
	@ID				Int
   ,@UserID			VARCHAR(20)
   ,@Password		VARCHAR(200)
   ,@FirstName		VARCHAR(50)
   ,@MiddleName		VARCHAR(50)
   ,@LastName		VARCHAR(50)
    ,@AccessType		Int
   ,@REPIDiv		VARCHAR(20)
   ,@UpdateID		VARCHAR(20)
   ,@UpdateDate		DATETIME
   ,@Return			Bit OUTPUT
   ,@Message		VARCHAR(50) OUTPUT
) AS

BEGIN TRANSACTION

	SET NOCOUNT ON
		DECLARE @one VARCHAR(50)
			BEGIN
				
				UPDATE [dbo].[User]
					SET UserID = @UserID,
						Password = @Password,
						FirstName = @FirstName,
						MiddleName = @MiddleName,
						LastName = @LastName,
						WorkstationName = 'n/a',
						AccessType = @AccessType,
						REPIDiv = @REPIDiv,			
					--	AccessModules = @AccessModules,
					--	Position = @Position,
					--	PassUpdateDate = @PassUpdateDate,
						--PrinterName = @PrinterName,
						--WorkstationName = @WorkstationName,
						--IPAddress = @IPAddress,
						UpdateID = @UpdateID,
						UpdateDate = @UpdateDate
					WHERE ID = @ID

				SET @Return = 1
							
				SET @Message = 'Successfully Updated User Details!'
			END
COMMIT TRANSACTION