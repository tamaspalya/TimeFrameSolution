USE [dmai0920_1086318]
GO

/****** Object:  Table [dbo].[UserTeamTable]    Script Date: 04/11/2021 12.28.44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserTeamTable](
	[UserID] [int] NOT NULL,
	[TeamID] [int] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserTeamTable]  WITH CHECK ADD  CONSTRAINT [FK_UserTeamTable_Team] FOREIGN KEY([TeamID])
REFERENCES [dbo].[Team] ([Id])
ON DELETE CASCADE 
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[UserTeamTable] CHECK CONSTRAINT [FK_UserTeamTable_Team]
GO

ALTER TABLE [dbo].[UserTeamTable]  WITH CHECK ADD  CONSTRAINT [FK_UserTeamTable_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE 
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[UserTeamTable] CHECK CONSTRAINT [FK_UserTeamTable_User]
GO


