USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bGETMenuDevice]    Script Date: 30-Jan-17 1:09:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bGETMenuDevice]
	-- Add the parameters for the stored procedure here
	@DeviceID BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT        TOP (20) bDeviceMenu.DeviceMenuID AS 'id', bDeviceMenu.Name AS 'name'
	FROM            bDeviceMenu INNER JOIN bDevice ON bDeviceMenu.TypeDeviceID = bDevice.TypeDeviceID
	WHERE        (bDeviceMenu.isActive = 1) AND (bDevice.DeviceID = 2)
	ORDER BY bDeviceMenu.OrderPlace
END

GO

