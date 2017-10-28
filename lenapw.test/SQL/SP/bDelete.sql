USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bDelete]    Script Date: 30-Jan-17 11:03:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bDelete]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT,	
	@name nvarchar(50)
AS
BEGIN	
	SET NOCOUNT ON;    

declare @DeviceID bigint
declare @MasterID bigint

set @DeviceID = 0;
set @MasterID = 0;


declare @existPair int

SELECT TOP (1)  @DeviceID = [data] FROM [bNowStatus] where chatID = @chatID AND statusID = 5 AND [name] = @name
if (@DeviceID <> 0)
	begin	
		SELECT TOP (1) @MasterID = [MasterID] FROM [bMaster] Where isActive = 1 AND chatID = @chatID AND TypeDeviceID = 2
			if (@MasterID <> 0)
				begin					
				UPDATE [bDeviceMasterPair] SET [isActive] = 0 WHERE [DeviceID] = @DeviceID AND [isActive] = 1 AND [MasterID] = @MasterID				
				SELECT @existPair = count(*)  FROM [bDeviceMasterPair] where [DeviceID] = @DeviceID AND [MasterID] = @MasterID AND [isActive] = 1
					if (@existPair = 0)
						begin
							UPDATE [bNowStatus]
							   SET 
								   [statusID] = 0
								  ,[data] = '-'
								  ,[name] = '-'
								  ,[dateCreate] = GETDATE()
							   WHERE [chatID] = @chatID;
							   select 1 as 'd', @name + ' has been successfully deleted' as 'm' -----------OK
						end
						else 
							begin 
								select 0 as 'd', @name + ' not paired' as 'm'
							 end 
				end
				else begin select 2 as 'd', 'Master is not found' as 'm' end

	end
	else begin select 3 as 'd', 'Wrong device NAME' as 'm' end
END

GO


