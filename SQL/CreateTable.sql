--------------------=================================CREATE TABLES
-------------------------Table [bCommands]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bCommands'))
BEGIN
     DROP TABLE [dbo].[bCommands]
END

CREATE TABLE [dbo].[bCommands](
	[commandID] [int] NOT NULL,
	[name] [nvarchar](50) NULL
) ON [PRIMARY]
---------------------Table [bDevice]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bDevice'))
BEGIN
     DROP TABLE [dbo].[bDevice]
END

CREATE TABLE [dbo].[bDevice](
	[DeviceID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[TypeDeviceID] [int] NULL,
	[Token] [nvarchar](512) NULL,
	[dataCreate] [smalldatetime] NULL,
	[AndroidIDmacHash] [nvarchar](50) NULL,
	[isActive] [bit] NULL,
	[codeA] [int] NULL,
	[codeB] [int] NULL,
	[timeCodeA] [datetime] NULL,
	[lastCheck] [smalldatetime] NULL,
 CONSTRAINT [PK_bDevice] PRIMARY KEY CLUSTERED 
(
	[DeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
---------------------------TABLE [dbo].[bDeviceMasterPair]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bDeviceMasterPair'))
BEGIN
     DROP TABLE [dbo].[bDeviceMasterPair]
END

CREATE TABLE [dbo].[bDeviceMasterPair](
	[DeviceID] [bigint] NULL,
	[MasterID] [bigint] NULL,
	[isActive] [bit] NULL
) ON [PRIMARY]
---------------------------TABLE [dbo].[bDeviceMenu]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bDeviceMenu'))
BEGIN
     DROP TABLE [dbo].[bDeviceMenu]
END

CREATE TABLE [dbo].[bDeviceMenu](
	[DeviceMenuID] [int] IDENTITY(1,1) NOT NULL,
	[commandID] [int] NULL,
	[TypeDeviceID] [int] NULL,
	[OrderPlace] [int] NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_bDeviceMenu] PRIMARY KEY CLUSTERED 
(
	[DeviceMenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

---------------------------TABLE [dbo].[bDeviceSetting]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bDeviceSetting'))
BEGIN
     DROP TABLE [dbo].[bDeviceSetting]
END

CREATE TABLE [dbo].[bDeviceSetting](
	[DeviceID] [bigint] NULL,
	[deviceStatusID] [int] NULL,
	[GEOon] [bit] NULL,
	[GeoPeriodTime] [int] NULL,
	[lastUpdate] [smalldatetime] NULL,
	[battarey] [int] NULL,
	[Latitude] [nvarchar](50) NULL,
	[lastGeoTime] [smalldatetime] NULL,
	[Longitude] [nvarchar](50) NULL,
	[CallOn] [bit] NULL
) ON [PRIMARY]

---------------------------TABLE [dbo].[bDeviceStatus]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bDeviceStatus'))
BEGIN
     DROP TABLE [dbo].[bDeviceStatus]
END

CREATE TABLE [dbo].[bDeviceStatus](
	[deviceStatusID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT [PK_bDeviceStatus] PRIMARY KEY CLUSTERED 
(
	[deviceStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

---------------------------TABLE [dbo].[bMaster]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bMaster'))
BEGIN
     DROP TABLE [dbo].[bMaster]
END

CREATE TABLE [dbo].[bMaster](
	[MasterID] [bigint] IDENTITY(1,1) NOT NULL,
	[chatID] [bigint] NULL,
	[isActive] [bit] NULL,
	[TypeDeviceID] [int] NULL,
	[codeB] [int] NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT [PK_bMaster] PRIMARY KEY CLUSTERED 
(
	[MasterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

---------------------------TABLE [dbo].[bNowStatus]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bNowStatus'))
BEGIN
     DROP TABLE [dbo].[bNowStatus]
END

CREATE TABLE [dbo].[bNowStatus](
	[chatID] [bigint] NULL,
	[statusID] [int] NULL,
	[data] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[username] [nvarchar](50) NULL,
	[firstName] [nvarchar](50) NULL,
	[lastName] [nvarchar](50) NULL,
	[dateCreate] [datetime] NULL
) ON [PRIMARY]

---------------------------TABLE [dbo].[bQueueCommand]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bQueueCommand'))
BEGIN
     DROP TABLE [dbo].[bQueueCommand]
END

CREATE TABLE [dbo].[bQueueCommand](
	[QueueCommandID] [bigint] IDENTITY(1,1) NOT NULL,
	[chatID] [bigint] NULL,
	[DeviceID] [bigint] NULL,
	[commandID] [int] NULL,
	[dataCreate] [smalldatetime] NULL,
	[isActive] [bit] NULL,
	[synchronizeTime] [smalldatetime] NULL,
	[IsSynchronized] [bit] NULL,
 CONSTRAINT [PK_bQueueCommand] PRIMARY KEY CLUSTERED 
(
	[QueueCommandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

---------------------------TABLE [dbo].[bStatus]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bStatus'))
BEGIN
     DROP TABLE [dbo].[bStatus]
END

CREATE TABLE [dbo].[bStatus](
	[statusID] [int] NULL,
	[name] [nvarchar](50) NULL
) ON [PRIMARY]

---------------------------TABLE [dbo].[bTypeDevice]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bTypeDevice'))
BEGIN
     DROP TABLE [dbo].[bTypeDevice]
END

CREATE TABLE [dbo].[bTypeDevice](
	[TypeDeviceID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_bTypeDevice] PRIMARY KEY CLUSTERED 
(
	[TypeDeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

---------------------------TABLE [dbo].[bErrors]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bErrors'))
BEGIN
     DROP TABLE [dbo].[bErrors]
END

CREATE TABLE [dbo].[bErrors](
	[errorsID] [int] IDENTITY(1,1) NOT NULL,
	[chatID] [bigint] NULL,
	[Error] [nvarchar](max) NULL,
	[datarecords] [smalldatetime] NULL,
 CONSTRAINT [PK_bErrors] PRIMARY KEY CLUSTERED 
(
	[errorsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


-------------------------TABLE [dbo.bPowerPC]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bPowerPC'))
BEGIN
     DROP TABLE [dbo].[bPowerPC]
END

CREATE TABLE [dbo].[bPowerPC](
	[PowerpcID] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceID] [bigint] NULL,
	[GUID] [nvarchar](50) NULL,
	[dateTimeOnPC] [smalldatetime] NULL,
	[dateTimeOffPC] [smalldatetime] NULL,
	[synchronizeTime] [smalldatetime] NULL,
	[IsSynchronized] [bit] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_bPowerPC] PRIMARY KEY CLUSTERED 
(
	[PowerpcID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

------------------------------------Table [bMessForSending]
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bMessForSending'))
BEGIN
     DROP TABLE [dbo].[bMessForSending]
END

CREATE TABLE [dbo].[bMessForSending](
	[messForSendingID] [bigint] IDENTITY(1,1) NOT NULL,
	[chatID] [bigint] NULL,
	[message] [nvarchar](max) NULL,
	[isSending] [bit] NULL,
	[isError] [bit] NULL,
	[timeCreate] [smalldatetime] NULL,
	[timeSending] [smalldatetime] NULL,
	[inProgress] [bit] NULL,	
	[ScreenShotID] [bigint] NULL,
 CONSTRAINT [PK_bMessForSending] PRIMARY KEY CLUSTERED 
(
	[messForSendingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


---------------------------------------------Table [bScreenShot]

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'bScreenShot'))
BEGIN
     DROP TABLE [dbo].[bScreenShot]
END

CREATE TABLE [dbo].[bScreenShot](
	[ScreenShotID] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceID] [bigint] NULL,
	[QueueCommandID] [bigint] NULL,
	[GUID] [nvarchar](50) NULL,
	[dateCreate] [smalldatetime] NULL,
	[synchronizeTime] [smalldatetime] NULL,
	[IsSynchronized] [bit] NULL,
	[ImageScreen] [image] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_bScreenShot] PRIMARY KEY CLUSTERED 
(
	[ScreenShotID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]







