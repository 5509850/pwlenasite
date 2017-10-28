USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bNowStatus]    Script Date: 30-Jan-17 1:13:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[bNowStatus](
	[chatID] [bigint] NULL,
	[statusID] [int] NULL,
	[data] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[dateCreate] [datetime] NULL
) ON [PRIMARY]

GO

