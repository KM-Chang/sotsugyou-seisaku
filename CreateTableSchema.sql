USE [openeyes]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[�樮����](
	[���u�s��] [char](10) NOT NULL,
	[�M�I����] [float] NULL,
	[���A] [nchar](2) NULL,
	[���] [datetime] NOT NULL,
 CONSTRAINT [PK_�樮����] PRIMARY KEY CLUSTERED 
(
	[���u�s��] ASC,
	[���] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[�樮����] ADD  CONSTRAINT [DF_�樮����_���]  DEFAULT (getdate()) FOR [���]
GO


