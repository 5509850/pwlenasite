USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bSendCommand]    Script Date: 30-Jan-17 1:09:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bSendCommand]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT,
	@commandID INT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
declare @DeviceID bigint
set @DeviceID = 0;

SELECT TOP (1)  @DeviceID = [data] FROM [bNowStatus] where chatID = @chatID AND statusID = 3
if (@DeviceID <> 0)
	begin
		INSERT INTO [dbo].[bQueueCommand]
           ([chatID]
           ,[DeviceID]
           ,[commandID]
           ,[dataCreate]
           ,[isActive])
     VALUES
           (@chatID
           ,@DeviceID
           ,@commandID
           ,GETDATE()
           ,1)
		   select @@IDENTITY as 'i'
	end
	
    -- Insert statements for procedure here
	
END

GO

