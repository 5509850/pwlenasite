USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bDeviceMasterPair]    Script Date: 30-Jan-17 1:12:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[bDeviceMasterPair](
	[DeviceID] [bigint] NULL,
	[MasterID] [bigint] NULL,
	[isActive] [bit] NULL
) ON [PRIMARY]

GO

