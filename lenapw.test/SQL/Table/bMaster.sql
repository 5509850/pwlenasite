USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bMaster]    Script Date: 30-Jan-17 1:13:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

GO

