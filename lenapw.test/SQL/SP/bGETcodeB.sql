USE [alexandr_gorbunov_]
GO

/****** Object:  StoredProcedure [dbo].[bGETcodeB]    Script Date: 30-Jan-17 1:09:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[bGETcodeB]
	-- Add the parameters for the stored procedure here
	@random INT,
	@codeA INT,
	@chatID BIGINT,
	@TypeDevice int,
	@slaveName nvarchar(50)
AS
BEGIN	
	SET NOCOUNT ON;    

declare @DeviceID bigint
declare @MasterID bigint
declare @timeOut bigint
set @DeviceID = 0;
set @MasterID = 0;
declare @existPair int
select top(1) @DeviceID = DeviceID, @timeOut = DATEDIFF (SECOND, [timeCodeA], GETDATE()) FROM bDevice where [codeA] = @codeA
  if (@DeviceID <> 0 AND @timeOut < 120)  
	begin 
		--'EXIST'
		UPDATE top(1) [bDevice] SET [codeB] = @random, [name] = @slaveName  WHERE DeviceID = @DeviceID
		SELECT TOP (1) @MasterID = MasterID FROM [bMaster] where chatID = @chatID AND TypeDeviceID = @TypeDevice
		if (@MasterID <> 0)
			begin
				UPDATE bMaster SET [isActive] = 1, [codeB] = @random WHERE MasterID = @MasterID
			end
		else -- Create new Master
			begin    
				INSERT INTO bMaster  
				([chatID] 
				,[isActive] 
				,[TypeDeviceID] 
				,[codeB]
				,[name])
					   VALUES 
					   (@chatID 
					   ,1 
					   ,@TypeDevice 
					   ,@random
					   ,@slaveName);
				SELECT @MasterID = SCOPE_IDENTITY();
			end
	--create pair 
	SELECT @existPair = count(*) FROM [bDeviceMasterPair] where [DeviceID] = @DeviceID AND [MasterID] = @MasterID AND [isActive] = 1
		if (@existPair <> 0)
			begin 
				SELECT -22 AS 'b' -- for code A	pair exist before!!!
			end
		else
			begin 
				INSERT INTO [bDeviceMasterPair]
				   ([DeviceID]
				   ,[MasterID]
				   ,[isActive])
					VALUES
				   (@DeviceID
				   ,@MasterID
				   ,1)
			end
 SELECT @existPair = count(*) FROM [bDeviceMasterPair] where [DeviceID] = @DeviceID AND [MasterID] = @MasterID AND [isActive] = 1
		if (@existPair = 1)
			begin 
			    --SUCCESS PAIR	
				SELECT @random AS 'b'	
				--clear code B
				UPDATE [dbo].[bDevice] SET [codeA] = NULL ,[codeB] = NULL  WHERE [DeviceID] = @DeviceID
			end
		else
			begin
				SELECT -4 AS 'b'	--unexpected error		
			end		
	end	
else -------------------------------'NOT EXIST code A'
begin 	
	if (@DeviceID = 0)  
		begin
			SELECT -11 AS 'b' -- not correct code A	
		end
	if (@DeviceID <> 0 AND @timeOut > 120)  
		begin
			SELECT -12 AS 'b' -- code A	is Expired 2min
		end
	
end

END

GO

