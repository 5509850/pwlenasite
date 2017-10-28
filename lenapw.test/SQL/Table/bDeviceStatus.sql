USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bDeviceStatus]    Script Date: 30-Jan-17 1:13:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[bDeviceStatus](
	[deviceStatusID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT [PK_bDeviceStatus] PRIMARY KEY CLUSTERED 
(
	[deviceStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

