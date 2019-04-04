USE ROHM_CAS_DEMO
GO
/****** Object:  StoredProcedure [dbo].[INSERT_UserData]    Script Date: 3/27/2019 2:10:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[INSERT_UserData](
	@UserID			VARCHAR(20)
   ,@Password		VARCHAR(200)
   ,@FirstName		VARCHAR(50)
   ,@MiddleName		VARCHAR(50)
   ,@LastName		VARCHAR(50)
  -- ,@WorkstationName VARCHAR(50)
   ,@AccessType		Int
   ,@REPIDiv		VARCHAR(10)
   --,@Photo			nvarchar(MAX)
   --,@IsDeleted		Bit
   ,@CreateID		VARCHAR(20)
   --,@CreateDate		DATETIME
   ,@UpdateID		VARCHAR(20)
   --,@UpdateDate		DATETIME
   --,@WorkstationName VARCHAR(50)
   --,@IPAddress VARCHAR(20)
   ,@Return			Bit OUTPUT
   ,@Message		VARCHAR(50) OUTPUT
) AS
BEGIN TRANSACTION
	
	SET NOCOUNT ON
	
	--DECLARE @ParentLotNo VARCHAR(15)
	--DECLARE @Type VARCHAR(9)
	--DECLARE @ProdCode VARCHAR(20)
	--DECLARE @ROHMProdCode VARCHAR(9)
	--DECLARE @LotQty BIGINT

				BEGIN
					IF EXISTS(SELECT UserID FROM [dbo].[User] WHERE UserID = @UserID AND IsDeleted = 0)
						BEGIN
							SET @Return = 0
							SET @Message = 'User ID is already in the database.'
						END
					ELSE
						BEGIN
				
							INSERT INTO [dbo].[User]
								(
										UserID
									,	Password
									,	FirstName
									,	MiddleName
									,	LastName
									,	AccessType
									,	REPIDiv
									,	WorkstationName
									,	IsDeleted
									,	CreateID
									,	CreateDate
									,	UpdateID
									,	UpdateDate

								)
								SELECT
									@UserID, 
									@Password,
									@FirstName,
									@MiddleName,
									@LastName,
									@AccessType,
									@REPIDiv,
									'n/a',
									0,
									@CreateID,
									GETDATE(),
									@UpdateID,
									GETDATE()

							SET @Return = 1
							
							SET @Message = 'Successfully Created!'
						END
				END	
COMMIT TRANSACTION
