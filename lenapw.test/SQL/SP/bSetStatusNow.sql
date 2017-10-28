USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bSetStatusNow]    Script Date: 30-Jan-17 1:09:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bSetStatusNow]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT,
	@statusID INT,	
	@data nvarchar(50),
	@slaveName nvarchar(50)
AS
BEGIN

	declare @existDtatus int
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT @existDtatus = count(*) FROM [bNowStatus] where chatID = @chatID
	if (@existDtatus <> 0)
		begin
			UPDATE [bNowStatus]
			SET 
			 [statusID] = @statusID
			,[data] = @data
			,[name] = @slaveName
			,[dateCreate] = GETDATE()
			WHERE chatID = @chatID
		end
	else
		begin
			INSERT INTO [dbo].[bNowStatus]
           ([chatID]
           ,[statusID]
           ,[data]
           ,[name]
           ,[dateCreate])
     VALUES
           (@chatID
           ,@statusID
           ,@data
           ,@slaveName
           ,GETDATE())
		end		
END

GO

