--1

IF OBJECT_ID('[bAutoSendPowerPCToMasterBot]', 'P') IS NOT NULL
begin
	DROP PROC [bAutoSendPowerPCToMasterBot]
end
--2
IF OBJECT_ID('[bAutoSendScreenShotToMasterBot]', 'P') IS NOT NULL
begin
	DROP PROC [bAutoSendScreenShotToMasterBot]
end
--3
IF OBJECT_ID('[bCheckDeviceOnline]', 'P') IS NOT NULL
begin
	DROP PROC [bCheckDeviceOnline]
end
--4
IF OBJECT_ID('[bDelete]', 'P') IS NOT NULL
begin
	DROP PROC [bDelete]
end
--5
IF OBJECT_ID('[bErrorlog]', 'P') IS NOT NULL
begin
	DROP PROC [bErrorlog]
end
--6
IF OBJECT_ID('[bGETcodeB]', 'P') IS NOT NULL
begin
	DROP PROC [bGETcodeB]
end
--7
IF OBJECT_ID('[bGETDevices]', 'P') IS NOT NULL
begin
	DROP PROC [bGETDevices]
end
--8
IF OBJECT_ID('[bGETMenuDevice]', 'P') IS NOT NULL
begin
	DROP PROC [bGETMenuDevice]
end
--9
IF OBJECT_ID('[bGetPowerPCByDevice]', 'P') IS NOT NULL
begin
	DROP PROC [bGetPowerPCByDevice]
end
--10
IF OBJECT_ID('[bGetScreenShotByDevice]', 'P') IS NOT NULL
begin
	DROP PROC [bGetScreenShotByDevice]
end
--11
IF OBJECT_ID('[bGetSlavePCStatus]', 'P') IS NOT NULL
begin
	DROP PROC [bGetSlavePCStatus]
end
--12
IF OBJECT_ID('[bGetSlaveStatus]', 'P') IS NOT NULL
begin
	DROP PROC [bGetSlaveStatus]
end
--13
IF OBJECT_ID('[bGetStartUpTime]', 'P') IS NOT NULL
begin
	DROP PROC [bGetStartUpTime]
end
--14
IF OBJECT_ID('[bGetStartUpTimeToday]', 'P') IS NOT NULL
begin
	DROP PROC [bGetStartUpTimeToday]
end
--15
IF OBJECT_ID('[bGetStatus]', 'P') IS NOT NULL
begin
	DROP PROC [bGetStatus]
end
--16
IF OBJECT_ID('[bRename]', 'P') IS NOT NULL
begin
	DROP PROC [bRename]
end
--17
IF OBJECT_ID('[bSendCommand]', 'P') IS NOT NULL
begin
	DROP PROC [bSendCommand]
end
--18
IF OBJECT_ID('[bSendNextMessage]', 'P') IS NOT NULL
begin
	DROP PROC [bSendNextMessage]
end
--19
IF OBJECT_ID('[bSetFullStatusNow]', 'P') IS NOT NULL
begin
	DROP PROC [bSetFullStatusNow]
end
--20
IF OBJECT_ID('[bSetStatusNow]', 'P') IS NOT NULL
begin
	DROP PROC [bSetStatusNow]
end
--21
IF OBJECT_ID('[GetProcParams]', 'P') IS NOT NULL
begin
	DROP PROC [GetProcParams]
end
--22
IF OBJECT_ID('[sCreateMessForSending]', 'P') IS NOT NULL
begin
	DROP PROC [sCreateMessForSending]
end
--23
IF OBJECT_ID('[sCreateMessPhotoForSending]', 'P') IS NOT NULL
begin
	DROP PROC [sCreateMessPhotoForSending]
end
--24
IF OBJECT_ID('[sDeletePair]', 'P') IS NOT NULL
begin
	DROP PROC [sDeletePair]
end
--25
IF OBJECT_ID('[sGETcodeA]', 'P') IS NOT NULL
begin
	DROP PROC [sGETcodeA]
end
--26
IF OBJECT_ID('[sGetCommand]', 'P') IS NOT NULL
begin
	DROP PROC [sGetCommand]
end
--27
IF OBJECT_ID('[sGetMasters]', 'P') IS NOT NULL
begin
	DROP PROC [sGetMasters]
end
--28
IF OBJECT_ID('[sRestorePair]', 'P') IS NOT NULL
begin
	DROP PROC [sRestorePair]
end
--29
IF OBJECT_ID('[sSetpowerTime]', 'P') IS NOT NULL
begin
	DROP PROC [sSetpowerTime]
end
--30
IF OBJECT_ID('[sSetScreenShot]', 'P') IS NOT NULL
begin
	DROP PROC [sSetScreenShot]
end



