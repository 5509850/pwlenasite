USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bGetSlaveStatus]    Script Date: 30-Jan-17 1:09:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bGetSlaveStatus]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT        TOP (1) bDeviceSetting.CallOn as 'call', 
						  bDeviceSetting.battarey as 'batt', 
						  bDeviceSetting.lastGeoTime as 'lastGeoTime', 
						  bDeviceSetting.lastUpdate AS 'last', 
						  bDeviceSetting.GEOon as 'geo', 						  
                         bDeviceStatus.[name] AS 'statusdevice'
			FROM            bDeviceSetting INNER JOIN
                         bNowStatus ON bDeviceSetting.DeviceID = bNowStatus.[data] INNER JOIN
                         bDeviceStatus ON bDeviceSetting.deviceStatusID = bDeviceStatus.deviceStatusID
						 where [dbo].[bNowStatus].chatID = @chatID
END

GO

