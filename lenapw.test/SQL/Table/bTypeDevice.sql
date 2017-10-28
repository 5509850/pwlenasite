USE [alexandr_gorbunov_]
GO

/****** Object:  Table [dbo].[bTypeDevice]    Script Date: 30-Jan-17 1:14:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[bTypeDevice](
	[TypeDeviceID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_bTypeDevice] PRIMARY KEY CLUSTERED 
(
	[TypeDeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

