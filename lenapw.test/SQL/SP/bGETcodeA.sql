USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bGETcodeA]    Script Date: 30-Jan-17 1:08:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bGETcodeA]	
	@random INT,
	@hash nvarchar(200),
	@TypeDevice INT	
AS
BEGIN	
	SET NOCOUNT ON;    
declare @exist int
declare @notuniq int
select @notuniq = count(*) FROM bDevice where [codeA] = @random AND (DATEDIFF (SECOND, [timeCodeA], GETDATE()) < 120)
if (@notuniq <> 0)
 begin
	SELECT -2 AS 'a'
 end
else
	BEGIN
select @exist = count(*) FROM bDevice where AndroidIDmacHash = @hash

  if (@exist <> 0)  
	begin 
		--'EXIST'
		UPDATE top(1) [bDevice] SET [codeA] = @random, [timeCodeA] = GETDATE()  WHERE [AndroidIDmacHash] = @hash AND [codeB] IS NULL	
	end
   else
   begin 
		--'NOT EXIST'
		INSERT INTO [bDevice]
           ([Name]
           ,[TypeDeviceID]
           ,[dataCreate]
           ,[AndroidIDmacHash]
           ,[isActive]
           ,[codeA]
		   ,[timeCodeA])
     VALUES
           ('Android 1'
           ,@TypeDevice           
           ,GETDATE()
           ,@hash
           ,0
           ,@random
		   ,GETDATE())
	end

SELECT top(1) codeA AS 'a' from [bDevice] WHERE [AndroidIDmacHash] = @hash
	END
END

GO

