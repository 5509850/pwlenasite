USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bDeviceMenu]    Script Date: 30-Jan-17 1:12:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[bDeviceMenu](
	[DeviceMenuID] [int] IDENTITY(1,1) NOT NULL,
	[TypeDeviceID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[OrderPlace] [int] NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_bDeviceMenu] PRIMARY KEY CLUSTERED 
(
	[DeviceMenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

