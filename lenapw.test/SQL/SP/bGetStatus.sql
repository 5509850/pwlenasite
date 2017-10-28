USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[nGetStatus]    Script Date: 30-Jan-17 1:10:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bGetStatus]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT TOP (1) [statusID] AS 's' ,[data] AS 'd' ,[name] AS 'n' FROM [bNowStatus] where chatID = @chatID
END

GO

