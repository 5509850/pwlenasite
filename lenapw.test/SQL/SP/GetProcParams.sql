USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[GetProcParams]    Script Date: 30-Jan-17 1:10:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetProcParams]	
	@Name NVarchar(200) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT p.name AS ParameterName, 
	CASE t.name 
	WHEN  'int' THEN '56' 
	WHEN  'datetime' THEN '58' 
	WHEN 'smalldatetime' THEN '58' 
	WHEN  'uniqueidentifier' THEN '36' 
	WHEN  'money' THEN '60' 
	WHEN  'decimal' THEN '108' 
	WHEN  'varchar' THEN '167' 
	WHEN 'nvarchar' THEN '167' 
	WHEN 'char' THEN '167' 	
	ELSE '167' 
END AS 'ParameterType'
FROM sys.parameters AS p 
	JOIN sys.types AS t 
	ON t.user_type_id = p.user_type_id 
	WHERE object_id = OBJECT_ID(@Name) 

END

GO

