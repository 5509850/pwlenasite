USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bGETDevices]    Script Date: 30-Jan-17 1:09:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bGETDevices]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @MasterID BIGINT
	SET @MasterID = 0
    -- Insert statements for procedure here
	SELECT TOP (1) @MasterID = MasterID FROM [bMaster] where [chatID] = @chatID
	if (@MasterID <> 0)
		begin			
		SELECT TOP (50) [bDeviceMasterPair].[DeviceID] as 'id', [bDevice].[Name] AS 'name'
		FROM [bDeviceMasterPair]
		inner join [bDevice]
		ON [bDevice].DeviceID = [bDeviceMasterPair].DeviceID
		where [bDeviceMasterPair].isActive = 1 AND [bDeviceMasterPair].[MasterID] = @MasterID
		order by [bDevice].[Name]
		end
END

GO

