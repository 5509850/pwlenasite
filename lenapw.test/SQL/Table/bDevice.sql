USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bDevice]    Script Date: 30-Jan-17 1:12:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
 CONSTRAINT [PK_bDevice] PRIMARY KEY CLUSTERED 
(
	[DeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

