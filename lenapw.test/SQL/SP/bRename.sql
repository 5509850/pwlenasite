USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bRename]    Script Date: 30-Jan-17 1:09:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[bRename]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT,	
	@name nvarchar(50)
AS
BEGIN	
	SET NOCOUNT ON;    

declare @DeviceID bigint
set @DeviceID = 0;



SELECT TOP (1)  @DeviceID = [data] FROM [bNowStatus] where chatID = @chatID AND statusID = 4
if (@DeviceID <> 0)
	begin	

		UPDATE [dbo].[bNowStatus] SET  [statusID] = 3,	[name] = @name 	,[dateCreate] = GETDATE() WHERE chatID = @chatID;
		UPDATE [dbo].[bDevice] 	SET [Name] = @name	WHERE DeviceID = @DeviceID
		select 1 as 'd', 'Device has been successfully renamed' as 'm' -----------OK
    end	
	else 
		begin 
		select 0 as 'd', 'Device not found' as 'm' end
END

GO

