USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bDeviceSetting]    Script Date: 30-Jan-17 1:13:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

GO

