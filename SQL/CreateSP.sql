-------------------===============================================================================================Create Stored Procedure
-------------------------------------------------------------SP [bDelete]   1
CREATE PROCEDURE [dbo].[bDelete]	
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

----------------------------------------------------------SP [sGETcodeA] 2

CREATE PROCEDURE  [dbo].[sGETcodeA]
	@random INT,
	@hash nvarchar(200),
	@TypeDevice INT	
AS
BEGIN	
	SET NOCOUNT ON;    
declare @exist int
declare @notuniq int
select @notuniq = count(*) FROM bDevice where [codeA] = @random AND (DATEDIFF (SECOND, [timeCodeA], GETDATE()) < 120)
if (@notuniq <> 0)
 begin
	SELECT -2 AS 'a'
 end
else
	BEGIN
select @exist = count(*) FROM bDevice where AndroidIDmacHash = @hash

  if (@exist <> 0)  
	begin 
		--'EXIST'
		UPDATE top(1) [bDevice] SET [codeA] = @random, [timeCodeA] = GETDATE()  WHERE [AndroidIDmacHash] = @hash AND [codeB] IS NULL	
	end
   else
   begin 
		--'NOT EXIST'
		INSERT INTO [bDevice]
           ([Name]
           ,[TypeDeviceID]
           ,[dataCreate]
           ,[AndroidIDmacHash]
           ,[isActive]
           ,[codeA]
		   ,[timeCodeA])
     VALUES
           ('Android 1'
           ,@TypeDevice           
           ,GETDATE()
           ,@hash
           ,0
           ,@random
		   ,GETDATE())
	end

SELECT top(1) codeA AS 'a' from [bDevice] WHERE [AndroidIDmacHash] = @hash
	END
END

-------------------------------------------------------SP [bGETcodeB] 3

CREATE PROCEDURE [dbo].[bGETcodeB]
	@random INT,
	@codeA INT,
	@chatID BIGINT,
	@TypeDevice int,
	@slaveName nvarchar(50),
	@masterName nvarchar(50)
AS
BEGIN	
	SET NOCOUNT ON;    

declare @DeviceID bigint
declare @MasterID bigint
declare @timeOut bigint
set @DeviceID = 0;
set @MasterID = 0;
declare @existPair int
declare @existBefore int
set @existBefore = 0;

---Clear all timeout code without code B
UPDATE [dbo].[bDevice] SET [codeA] = NULL where (codeA IS NOT NULL) AND  (DATEDIFF (SECOND, [timeCodeA], GETDATE()) > 130) AND ([codeB] IS NULL)

select top(1) @DeviceID = DeviceID, @timeOut = DATEDIFF (SECOND, [timeCodeA], GETDATE()) FROM bDevice where [codeA] = @codeA
  if (@DeviceID <> 0 AND @timeOut < 120)  
	begin 
		--'EXIST'
		UPDATE top(1) [bDevice] SET [codeB] = @random, [name] = @slaveName  WHERE DeviceID = @DeviceID
		SELECT TOP (1) @MasterID = MasterID FROM [bMaster] where chatID = @chatID AND TypeDeviceID = @TypeDevice
		if (@MasterID <> 0)
			begin
				UPDATE bMaster SET [isActive] = 1, [codeB] = @random, [name] = @masterName WHERE MasterID = @MasterID
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
					   ,@masterName);
				SELECT @MasterID = SCOPE_IDENTITY();
			end
	--create pair 
	SELECT @existPair = count(*) FROM [bDeviceMasterPair] where [DeviceID] = @DeviceID AND [MasterID] = @MasterID AND [isActive] = 1
		if (@existPair <> 0)
			begin 
				SELECT -22 AS 'b' -- for code A	pair exist before!!!
				set @existBefore = 1;
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
		if (@existPair = 1 AND @existBefore = 0)
			begin 
			    --SUCCESS PAIR	
				SELECT @random AS 'b'	
				--clear code B and active Device!
				UPDATE [dbo].[bDevice] SET [codeA] = NULL ,[codeB] = NULL, isActive = 1  WHERE [DeviceID] = @DeviceID				
			end
		if (@existPair = 0)
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
---clear codeB
UPDATE [dbo].[bDevice] SET [codeB] = NULL  WHERE [DeviceID] = @DeviceID	

END


-------------------------------------------------------------SP [bGETDevices] 4
CREATE PROCEDURE [dbo].[bGETDevices]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT
AS
BEGIN	
	SET NOCOUNT ON;
	DECLARE @MasterID BIGINT
	SET @MasterID = 0
    
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

-------------------------------------------------SP [bGETMenuDevice]             5 
CREATE PROCEDURE [dbo].[bGETMenuDevice]	
	@DeviceID BIGINT
AS
BEGIN	
	SET NOCOUNT ON;
	SELECT        TOP (20) [bDeviceMenu].[commandID] AS 'id', [bCommands].[name] AS 'name'
	FROM            bDeviceMenu INNER JOIN bDevice ON bDeviceMenu.TypeDeviceID = bDevice.TypeDeviceID
					INNER JOIN [bCommands] ON bDeviceMenu.commandID = [bCommands].commandID
	WHERE        (bDeviceMenu.isActive = 1) AND (bDevice.DeviceID = @DeviceID)
	ORDER BY bDeviceMenu.OrderPlace
END

-----------------------------------------------------SP [bGetSlaveStatus]                 6
CREATE PROCEDURE [dbo].[bGetSlaveStatus]	
	@chatID BIGINT
AS
BEGIN	
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
--------------------------------------------------------------SP [bRename]                  7
Create PROCEDURE [dbo].[bRename]	
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

------------------------------------------------------------------SP [bSendCommand]          8
CREATE PROCEDURE [dbo].[bSendCommand]	
	@chatID BIGINT,
	@commandID INT	
AS
BEGIN	
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
END

---------------------------------------------------------------SP [bSetStatusNow]               9
CREATE PROCEDURE [dbo].[bSetStatusNow]	
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


------------------------------------------------------------------ SP [bSetFullStatusNow] 10
CREATE PROCEDURE [dbo].[bSetFullStatusNow]
	-- Add the parameters for the stored procedure here
	@chatID BIGINT,
	@statusID INT,	
	@data nvarchar(50),
	@slaveName nvarchar(50),
	@Username nvarchar(50),
	@FirstName nvarchar(50),
	@LastName nvarchar(50)
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
			,[username] = @Username
			,[firstName] = @FirstName
			,[lastName] = @LastName
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
		   ,[username]
		   ,[firstName] 
			,[lastName] 
           ,[dateCreate])
     VALUES
           (@chatID
           ,@statusID
           ,@data
           ,@slaveName
		   ,@Username
		   ,@FirstName
		   ,@LastName
           ,GETDATE())
		end		
END


--------------------------------------------------------------SP [bGetStatus] 11

CREATE PROCEDURE [dbo].[bGetStatus]	
	@chatID BIGINT
AS
BEGIN
	
	SET NOCOUNT ON;    
	SELECT TOP (1) [statusID] AS 's' ,[data] AS 'd' ,[name] AS 'n', ISNULL([username], '-') AS 'u', ISNULL([firstName], '-') as 'f', ISNULL([lastName], '-') as 'l' FROM [bNowStatus] where chatID = @chatID
END


-------------------------======================================================for SQL LIB       SP GetProcParams        12
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

----------------------------------------------------------SP bErrorlog 13

CREATE PROCEDURE bErrorlog
	@chatID bigint,
	@Error nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
INSERT INTO [dbo].[bErrors]
           ([chatID]
           ,[Error]
           ,[datarecords])
     VALUES
           (@chatID
           ,@Error
           ,GETDATE())

END
----------------------------------------------------SP  sGetMasters             14
CREATE PROCEDURE [dbo].[sGetMasters]	
	@hash nvarchar(200)
AS
BEGIN	
	SET NOCOUNT ON;

	SELECT        TOP (20) bMaster.MasterID, bMaster.codeB, bMaster.name, bMaster.TypeDeviceID AS typedeviceID, bTypeDevice.Name AS typedevice, bMaster.isActive AS 'isActive'
FROM            bMaster INNER JOIN
                         bTypeDevice ON bMaster.TypeDeviceID = bTypeDevice.TypeDeviceID
WHERE        (bMaster.MasterID IN
                             (SELECT        TOP (200) bDeviceMasterPair.MasterID
                               FROM            bDeviceMasterPair INNER JOIN
                                                         bDevice ON bDeviceMasterPair.DeviceID = bDevice.DeviceID
                               WHERE        (bDeviceMasterPair.isActive = 1) AND (bDevice.AndroidIDmacHash = @hash)))
END

------------------------------------------------------ SP sDeletePair                 15
CREATE PROCEDURE [dbo].[sDeletePair]
	-- Add the parameters for the stored procedure here
	@hash nvarchar(200),
	@MasterID bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE  @DeviceID BIGINT
	SET @DeviceID = 0;
	SELECT TOP (1) @DeviceID = DeviceID FROM bDevice WHERE (AndroidIDmacHash = N'hash')

	IF (@DeviceID <> 0)
		begin 
			UPDATE [bDeviceMasterPair] SET  [isActive] = 0 WHERE [DeviceID] = @DeviceID AND [MasterID] = @MasterID
			select count(*) as 'ok' FROM [bDeviceMasterPair] where [DeviceID] = @DeviceID AND [MasterID] = @MasterID AND [isActive] = 1
		end
	ELSE
		begin 
			select -1 as 'ok'
		end
END

----------------------------------------- SP sRestorePair                                  16
CREATE PROCEDURE sRestorePair
	-- Add the parameters for the stored procedure here
	@hash nvarchar(200),
	@MasterID bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE  @DeviceID BIGINT
	SET @DeviceID = 0;
	SELECT TOP (1) @DeviceID = DeviceID FROM bDevice WHERE (AndroidIDmacHash = N'hash')

	IF (@DeviceID <> 0)
		begin 
			UPDATE top(1) [bDeviceMasterPair] SET  [isActive] = 1 WHERE [DeviceID] = @DeviceID AND [MasterID] = @MasterID
			select count(*) as 'ok' FROM [bDeviceMasterPair] where [DeviceID] = @DeviceID AND [MasterID] = @MasterID AND [isActive] = 0
		end
	ELSE
		begin 
			select -1 as 'ok'
		end
END
----------------------------------------SP [sSetpowerTime]                         17
CREATE PROCEDURE [dbo].[sSetpowerTime]	
	@hash nvarchar(200),
	@TypeDeviceID int,
	@GUID nvarchar(50),
	@dateTimeOnPC smalldatetime,
	@dateTimeOffPC smalldatetime
		
AS
BEGIN	
	SET NOCOUNT ON;

	DECLARE  @DeviceID BIGINT
	DECLARE  @count INT
	SET @DeviceID = 0;	
	SET @count = 0;
	DECLARE  @notactive INT
	SET @notactive = 0;
	DECLARE  @newrecords INT
	SET @newrecords = 0;


	SELECT @DeviceID = [DeviceID]  FROM [dbo].[bDevice] where [AndroidIDmacHash] = @hash AND [isActive] = 1 AND [TypeDeviceID] = @TypeDeviceID
	SELECT @notactive = count(*)   FROM [dbo].[bDevice] where [AndroidIDmacHash] = @hash AND [isActive] = 0 AND [TypeDeviceID] = @TypeDeviceID

	IF (@DeviceID = 0)
		begin 
				if (@notactive <> 0)
					begin
						select -777 as 'c', @newrecords as 'n' 
					end
					else
						begin 
							select -2 as 'c', @newrecords as 'n'
						end
				
		end
	ELSE
		begin  

		SELECT @count = count(*) FROM [dbo].[bPowerPC] WHERE [DeviceID] = @DeviceID AND [GUID] = @GUID
		
		if (@count = 0)		
			begin
				INSERT INTO [dbo].[bPowerPC]
						   ([DeviceID]
						   ,[GUID]
						   ,[dateTimeOnPC]
						   ,[dateTimeOffPC]
						   ,[synchronizeTime]
						   ,[IsSynchronized]
						   ,[IsActive])
						VALUES
						   (@DeviceID
						   ,@GUID
						   ,@dateTimeOnPC
						   ,@dateTimeOffPC
						   ,GETDATE()
						   ,1
						   ,1)
				select @newrecords = 1;
			end
		else
		    begin		
			UPDATE [dbo].[bPowerPC]
			SET 
			[dateTimeOnPC] = @dateTimeOnPC
			,[dateTimeOffPC] = @dateTimeOffPC						   
			,[synchronizeTime] = GETDATE()			
			WHERE [DeviceID] = @DeviceID AND [GUID] = @GUID
			end
		select 1 as 'c', @newrecords as 'n'
		end

END

-------------------------------------------------------------------SP [bGetStartUpTimeToday]	 18
CREATE PROCEDURE bGetStartUpTimeToday	
	@queueCommandID BIGINT
AS
BEGIN
	SET NOCOUNT ON;
    SELECT        TOP (100) bPowerPC.dateTimeOnPC AS 'on', bPowerPC.dateTimeOffPC AS 'off', bDevice.Name AS 'name', bTypeDevice.Name AS 'type', bPowerPC.synchronizeTime AS 'sync'
FROM            bPowerPC INNER JOIN
                         bQueueCommand ON bPowerPC.DeviceID = bQueueCommand.DeviceID 
						 INNER JOIN
                         bDevice ON bPowerPC.DeviceID = bDevice.DeviceID 
						 INNER JOIN
                         bTypeDevice ON bDevice.TypeDeviceID = bTypeDevice.TypeDeviceID
WHERE        (bPowerPC.IsActive = 1) 
			AND (bPowerPC.dateTimeOnPC > CONVERT(date, GETDATE())) 
			AND (bQueueCommand.QueueCommandID = @queueCommandID)
			AND (bQueueCommand.isActive = 1)
ORDER BY bPowerPC.PowerpcID 
--UPDATE       TOP (1) bQueueCommand SET isActive = 0 where  QueueCommandID = @queueCommandID
END

----------------------------------------------------------------------SP bGetPowerPCByDevice             19
CREATE PROCEDURE bGetPowerPCByDevice
		-- Add the parameters for the stored procedure here
		@date smalldatetime,
		@hash Nvarchar(50)
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;

		-- Insert statements for procedure here
		SELECT        TOP (200) bPowerPC.GUID, bPowerPC.dateTimeOnPC, bPowerPC.dateTimeOffPC, bPowerPC.synchronizeTime
	FROM            bPowerPC INNER JOIN
							 bDevice ON bPowerPC.DeviceID = bDevice.DeviceID
	WHERE        (bDevice.AndroidIDmacHash = @hash) AND (bPowerPC.IsActive = 1) AND (bPowerPC.dateTimeOnPC >= CONVERT(date, @date))
	END

----------------------------------------------SP [bGetStartUpTime]                                20
	CREATE PROCEDURE [bGetStartUpTime]
	@queueCommandID BIGINT,
	@from SMALLDATETIME,
	@to SMALLDATETIME
	
AS
BEGIN
	SET NOCOUNT ON;
    SELECT        TOP (50) bPowerPC.dateTimeOnPC AS 'on', bPowerPC.dateTimeOffPC AS 'off', bDevice.Name AS 'name', bTypeDevice.Name AS 'type', bPowerPC.synchronizeTime AS 'sync'
FROM            bPowerPC INNER JOIN
                         bQueueCommand ON bPowerPC.DeviceID = bQueueCommand.DeviceID 
						 INNER JOIN
                         bDevice ON bPowerPC.DeviceID = bDevice.DeviceID 
						 INNER JOIN
                         bTypeDevice ON bDevice.TypeDeviceID = bTypeDevice.TypeDeviceID
WHERE        (bPowerPC.IsActive = 1) 
			AND (bPowerPC.dateTimeOnPC > CONVERT(date, @from)) 
			AND (bPowerPC.dateTimeOnPC < dateadd(day, 1, CONVERT(date, @to))) 
			AND (bQueueCommand.QueueCommandID = @queueCommandID)
			AND (bQueueCommand.isActive = 1)
ORDER BY bPowerPC.PowerpcID 
UPDATE       TOP (1) bQueueCommand SET isActive = 0 where  QueueCommandID = @queueCommandID
END
------------------------------------------------------SP [bAutoSendPowerPCToMasterBot]          21
CREATE PROCEDURE bAutoSendPowerPCToMasterBot
	-- Add the parameters for the stored procedure here
	@hash nvarchar(200),
	@TypeDeviceID int,
	@GUID nvarchar(50)
AS
BEGIN	
	SET NOCOUNT ON;
SELECT        TOP (20) 
bPowerPC.dateTimeOnPC AS 'on', 
bPowerPC.dateTimeOffPC AS 'off', 
bPowerPC.synchronizeTime AS 'sync',
bDevice.Name AS 'name', 
bTypeDevice.Name AS 'type', masters.chatID as 'chatID'
FROM            bPowerPC INNER JOIN                         						 
                         bDevice ON bPowerPC.DeviceID = bDevice.DeviceID 
						 INNER JOIN
                         bTypeDevice ON bDevice.TypeDeviceID = bTypeDevice.TypeDeviceID
						 INNER JOIN 
						 -----------------------------
							 (SELECT        TOP (20) bMaster.chatID, bDeviceMasterPair.DeviceID
								FROM            bMaster INNER JOIN
								bDeviceMasterPair ON bMaster.MasterID = bDeviceMasterPair.MasterID
								WHERE        (bMaster.TypeDeviceID = @TypeDeviceID) AND (bMaster.isActive = 1) AND (bDeviceMasterPair.isActive = 1)) 
						---------------------------
				AS masters ON  masters.DeviceID = bDevice.DeviceID
WHERE        ([bPowerPC].[IsActive] = 1) 	
AND	([bPowerPC].[GUID]	= @GUID)
AND (bDevice.androidIDmacHash = @hash)
AND (bDevice.isActive = 1)
END
---------------------------------------------------------------------------- SP [sCreateMessForSending]              22

CREATE PROCEDURE sCreateMessForSending	
	@chatID bigint,
	@mess nvarchar(max)
AS
BEGIN	
	SET NOCOUNT ON;
	INSERT 
INTO              bMessForSending(chatID, [message], isSending, isError, timeCreate)
VALUES        (@chatID,@mess,0,0,GETDATE())
END
------------------------------------------------------------------------------------------SP [bSendNextMessage]        23
CREATE PROCEDURE bSendNextMessage	
	@messForSendingID bigint,
	@error int
AS
BEGIN	
	SET NOCOUNT ON;
   
declare @work int
declare @chatID bigint
declare @message nvarchar(max)
declare @ImageScreen int
declare @DateCreate smalldatetime
set @ImageScreen = 0
set @work = 0
set @DateCreate = GETDATE()

if (@messForSendingID <> 0)
	begin 
		if (@error <> 0) -- update error
			begin 
				UPDATE       TOP (1) bMessForSending
				SET                isError = 1, timeSending = GETDATE(), inProgress = NULL
				where messForSendingID = @messForSendingID
			end
		else -- udpate sending
			begin 
				UPDATE       TOP (1) bMessForSending
				SET                isSending = 1, timeSending = GETDATE(), inProgress = NULL
				where messForSendingID = @messForSendingID
			end
	end

set @messForSendingID = 0

--work only for 10 minute operate inProgress
SELECT @work = count(*)
  FROM [bMessForSending]
  where [inProgress] = 1 AND datediff(minute, [timeCreate], GETDATE()) < 10

if (@work <> 0) -- now working other process for sending
	begin
		select 1 as 'work', 0 'empty', 0 as  'messForSendingID',  0 as 'chatID' , '0' as 'message', @ImageScreen as 'imagescreen', @DateCreate as 'DateCreate' 		
	end
else
	begin 
		SELECT TOP (1) 
		@messForSendingID = [bMessForSending].[messForSendingID] ,  
		@chatID = [bMessForSending].[chatID] , 
		@message = [bMessForSending].[message], 
		@DateCreate =  ISNULL([bScreenShot].[dateCreate], GETDATE()),		
		@ImageScreen = 
		 CASE 
			WHEN [bScreenShot].[ImageScreen] IS NULL 
			THEN 0 
			ELSE 1 
		 END 
		
		FROM [bMessForSending]
		left outer join [bScreenShot]
			ON [bScreenShot].[ScreenShotID] = [bMessForSending].[ScreenShotID]
		where [isSending] = 0 AND [isError] = 0 AND [inProgress] IS NULL

		if (@messForSendingID = 0) --nothing to send
			begin 
				select 0 as 'work', 1 'empty', 0 as  'messForSendingID',  0 as 'chatID' , '0' as 'message', @ImageScreen as 'imagescreen', @DateCreate as 'DateCreate' 		
			end
		else ------work = @messForSendingID <> 0
			begin
				 UPDATE TOP (1) [bMessForSending] SET [inProgress] = 1  WHERE messForSendingID = @messForSendingID
				 select 0 as 'work', 0 'empty', @messForSendingID as  'messForSendingID',  @chatID as 'chatID' , @message as 'message', 
				 @ImageScreen as 'imagescreen', (SELECT TOP (1) 
						   [bScreenShot].[ImageScreen]      
							FROM [dbo].[bScreenShot]
							inner join [bMessForSending]
							ON [bMessForSending].[ScreenShotID] = [bScreenShot].[ScreenShotID]
							where [bMessForSending].[messForSendingID] = @messForSendingID AND [bScreenShot].[IsActive] = 1) as 'image',
							@DateCreate  as 'DateCreate' 		
			end
				
	end

END

------------------------------------------------------------------------ SP [sSetScreenShot]                      24


CREATE PROCEDURE [dbo].[sSetScreenShot]	
	@hash nvarchar(200),
	@TypeDeviceID int,
	@GUID nvarchar(50),
	@QueueCommandID bigint,
	@dateCreate	smalldatetime,
	@img image
		
AS
BEGIN	
	SET NOCOUNT ON;

	DECLARE  @DeviceID BIGINT
	DECLARE  @count INT
	SET @DeviceID = 0;	
	SET @count = 0;
	DECLARE  @notactive INT
	SET @notactive = 0;
	DECLARE  @newrecords INT
	SET @newrecords = 0;


	SELECT @DeviceID = [DeviceID]  FROM [dbo].[bDevice] where [AndroidIDmacHash] = @hash AND [isActive] = 1 AND [TypeDeviceID] = @TypeDeviceID
	SELECT @notactive = count(*)   FROM [dbo].[bDevice] where [AndroidIDmacHash] = @hash AND [isActive] = 0 AND [TypeDeviceID] = @TypeDeviceID

	IF (@DeviceID = 0)
		begin 
				if (@notactive <> 0)
					begin
						select -777 as 'c', @newrecords as 'n' 
					end
					else
						begin 
							select -2 as 'c', @newrecords as 'n'
						end
				
		end
	ELSE
		begin  

		SELECT @count = count(*) FROM [dbo].[bScreenShot] WHERE [DeviceID] = @DeviceID AND [GUID] = @GUID
		
		if (@count = 0)		
			begin
				INSERT INTO [dbo].[bScreenShot]
						   ([DeviceID]
						   ,[QueueCommandID]
						   ,[GUID]
						   ,[dateCreate]
						   ,[synchronizeTime]
						   ,[IsSynchronized]
						   ,[ImageScreen]
						   ,[IsActive])
						VALUES
						   (@DeviceID
						   ,@QueueCommandID
						   ,@GUID
						   ,@dateCreate						   
						   ,GETDATE()
						   ,1
						   ,@img
						   ,1)
				select @newrecords = 1;
				select 1 as 'c', @newrecords as 'n'
			end
			else
			begin 
				select 0 as 'c', 0 as 'n'
			end					
			
		end

END

----------------------------------------------------------------------------------------------------SP [bAutoSendScreenShotToMasterBot]             25
Create PROCEDURE [dbo].[bAutoSendScreenShotToMasterBot]
	-- Add the parameters for the stored procedure here
	@hash nvarchar(200),
	@TypeDeviceID int,
	@GUID nvarchar(50)
AS
BEGIN	
	SET NOCOUNT ON;
	
DECLARE @QueueCommandID bigint
SET @QueueCommandID = 0

select @QueueCommandID = bScreenShot.QueueCommandID from [bScreenShot] 
INNER JOIN   bDevice ON bScreenShot.DeviceID = bDevice.DeviceID 
where ([bScreenShot].[GUID] = @GUID) AND ([bScreenShot].[IsActive] = 1) AND (bDevice.androidIDmacHash = @hash)

IF (@QueueCommandID = 0)
	begin
		SELECT        TOP (20) 
		bScreenShot.ScreenShotID as 'screenshotid',
		bScreenShot.dateCreate AS 'created', 
		bScreenShot.synchronizeTime AS 'synchronizeTime', 		
		bDevice.Name AS 'name', 
		bTypeDevice.Name AS 'type', 
		masters.chatID as 'chatID'
		FROM            bScreenShot INNER JOIN                         						 
								 bDevice ON bScreenShot.DeviceID = bDevice.DeviceID 
								 INNER JOIN
								 bTypeDevice ON bDevice.TypeDeviceID = bTypeDevice.TypeDeviceID
								 INNER JOIN 
								 -----------------------------
									 (SELECT        TOP (20) bMaster.chatID, bDeviceMasterPair.DeviceID
										FROM            bMaster INNER JOIN
										bDeviceMasterPair ON bMaster.MasterID = bDeviceMasterPair.MasterID
										WHERE        (bMaster.TypeDeviceID = @TypeDeviceID) AND (bMaster.isActive = 1) AND (bDeviceMasterPair.isActive = 1)) 
										AS masters 
								---------------------------
						ON  masters.DeviceID = bDevice.DeviceID
		WHERE        ([bScreenShot].[IsActive] = 1) 	
		AND	([bScreenShot].[GUID]	= @GUID)
		AND (bDevice.androidIDmacHash = @hash)
		AND (bDevice.isActive = 1)
	end
ELSE     --(@QueueCommandID <> 0)
 begin 
	SELECT        TOP (1) 
		bScreenShot.ScreenShotID as 'screenshotid',
		bScreenShot.dateCreate AS 'created', 
		bScreenShot.synchronizeTime AS 'synchronizeTime', 		
		bDevice.Name AS 'name', 
		bTypeDevice.Name AS 'type', 
		bQueueCommand.chatID  as 'chatID'
		FROM            bScreenShot INNER JOIN                         						 
								 bDevice ON bScreenShot.DeviceID = bDevice.DeviceID 
								 INNER JOIN
								 bTypeDevice ON bDevice.TypeDeviceID = bTypeDevice.TypeDeviceID
								 INNER JOIN 
								 bQueueCommand ON bScreenShot.QueueCommandID = bQueueCommand.QueueCommandID
		WHERE        		
		(bQueueCommand.QueueCommandID = @QueueCommandID)
		AND ([bScreenShot].[IsActive] = 1) 	
		AND	([bScreenShot].[GUID]	= @GUID)
		AND (bDevice.androidIDmacHash = @hash)
		AND (bDevice.isActive = 1)
 end
END
-------------------------------------------------------------------------------------------------SP [sCreateMessPhotoForSending]          26
CREATE  PROCEDURE [dbo].[sCreateMessPhotoForSending]	
	@chatID bigint,
	@screenshotid bigint,
	@mess nvarchar(max)
AS
BEGIN	
	SET NOCOUNT ON;
	INSERT 
INTO              bMessForSending(chatID, [message], isSending, isError, timeCreate, ScreenShotID)
VALUES        (@chatID,@mess,0,0,GETDATE(), @screenshotid)
END
-----------------------------------------------------------------------------------------SP [bGetScreenShotByDevice]          27
CREATE PROCEDURE [dbo].[bGetScreenShotByDevice]	
	@GUID Nvarchar(50),
	@hash Nvarchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;    
	SELECT        TOP (1) bScreenShot.[GUID], bScreenShot.ImageScreen, bScreenShot.dateCreate,  bScreenShot.synchronizeTime
FROM            bScreenShot INNER JOIN
                         bDevice ON bScreenShot.DeviceID = bDevice.DeviceID
WHERE        (bDevice.AndroidIDmacHash = @hash) AND (bScreenShot.IsActive = 1) AND bScreenShot.[GUID] = @GUID
END

-----------------------------------------------------------------------------------SP [sGetCommand]                     28
CREATE PROCEDURE sGetCommand	
		@hash nvarchar(200)
AS
BEGIN	
	SET NOCOUNT ON;

DECLARE @DeviceID bigint
set @DeviceID = 0;

--check Device
SELECT @DeviceID = [DeviceID] FROM [bDevice] where [isActive] = 1 AND [AndroidIDmacHash] = @hash

if (@DeviceID <> 0)
	begin

--GET Command by Device ID
		SELECT        TOP (5) QueueCommandID, commandID, dataCreate
FROM            bQueueCommand 
WHERE        
DeviceID = @DeviceID
AND (isActive = 1) 
AND (IsSynchronized IS NULL) 
AND (synchronizeTime IS NULL) 
AND (DATEDIFF(minute, dataCreate, GETDATE()) < 60)
ORDER BY QueueCommandID DESC

--Update command is notActive
UPDATE [dbo].[bQueueCommand]
   SET 
       [isActive] = 0
      ,[synchronizeTime] = GETDATE()
      ,[IsSynchronized] = 1
 WHERE QueueCommandID IN (SELECT        TOP (5) QueueCommandID FROM  bQueueCommand  WHERE         
		DeviceID = @DeviceID
		AND (isActive = 1) 
		AND (IsSynchronized IS NULL) 
		AND (synchronizeTime IS NULL) 
		AND (DATEDIFF(minute, dataCreate, GETDATE()) < 60)
		ORDER BY QueueCommandID DESC)

--Update LastCheck Time
UPDATE [bDevice] SET [lastCheck] = GETDATE() WHERE DeviceID = @DeviceID
	end
END
-------------------------------------------------------- SP [bCheckDeviceOnline]                       29
CREATE PROCEDURE bCheckDeviceOnline
	@queueCommandID BIGINT	
AS
BEGIN
	SET NOCOUNT ON;
DECLARE @lastCheck smalldatetime
set @lastCheck = getdate()

SELECT @lastCheck = [bDevice].[lastCheck]  FROM [bDevice]
  inner join [bQueueCommand]
		ON [bDevice].[DeviceID] = [bQueueCommand].[DeviceID]
  where [bQueueCommand].[QueueCommandID] = @queueCommandID

  if (@lastCheck IS NULL)
	   begin
	    UPDATE [dbo].[bQueueCommand]    SET  [isActive] = 0  WHERE QueueCommandID  = @queueCommandID
		select CONVERT(bit, 0) as 'online'
	   end
   else
	begin
		if (DATEDIFF(minute, @lastCheck, GETDATE()) > 2)
			begin
				UPDATE [dbo].[bQueueCommand]    SET  [isActive] = 0  WHERE QueueCommandID  = @queueCommandID
				select CONVERT(bit, 0) as 'online'
			end
		else
		begin
				select CONVERT(bit, 1) as 'online'
			end
	end
END
-----------------------------------------------------------------------------SP [bGetSlavePCStatus]             30

CREATE PROCEDURE [bGetSlavePCStatus]	
	@queueCommandID BIGINT
AS
BEGIN

SET NOCOUNT ON;
DECLARE @DeviceID bigint
SET @DeviceID = 0
DECLARE @lastChecks smalldatetime
set @lastChecks = getdate()
DECLARE @online bit
SET @online = CONVERT(bit, 0)
DECLARE @on smalldatetime
DECLARE @off smalldatetime

SELECT TOP (1) @DeviceID = [DeviceID] FROM [bQueueCommand] WHERE [QueueCommandID] = @queueCommandID
SELECT @lastChecks = [lastCheck]  FROM [bDevice] WHERE [DeviceID] = @DeviceID	  
SELECT TOP (1) @on = [dateTimeOnPC] , @off = [dateTimeOffPC] FROM [bPowerPC] where [DeviceID] = @DeviceID order by [PowerpcID] DESC
 
if (@lastChecks IS NOT NULL) 
	begin
		if (DATEDIFF(minute, @lastChecks, GETDATE()) > 2)
			begin
				select @online = CONVERT(bit, 0)
			end
		else
		begin
				select @online = CONVERT(bit, 1)
		end
		SELECT        bDevice.Name AS 'name', bTypeDevice.Name AS 'type', @online as 'online',  ISNULL(@on, GETDATE()) AS 'on', ISNULL(@off, GETDATE()) AS 'off'
		FROM          bDevice INNER JOIN
                         bTypeDevice ON bDevice.TypeDeviceID = bTypeDevice.TypeDeviceID
		WHERE        [bDevice].[DeviceID] = @DeviceID
	end			

UPDATE       TOP (1) bQueueCommand SET isActive = 0 where  QueueCommandID = @queueCommandID
END