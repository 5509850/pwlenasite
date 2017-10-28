USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bQueueCommand]    Script Date: 30-Jan-17 1:13:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[bQueueCommand](
	[QueueCommandID] [bigint] IDENTITY(1,1) NOT NULL,
	[chatID] [bigint] NULL,
	[DeviceID] [bigint] NULL,
	[commandID] [int] NULL,
	[dataCreate] [smalldatetime] NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_bQueueCommand] PRIMARY KEY CLUSTERED 
(
	[QueueCommandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

