GO

/****** Object:  Table [dbo].[TaskUser]    Script Date: 24/11/2021 11:17:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WorkItemUser](
	[WorkItemId] [int] NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WorkItemUser]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemUser_WorkItem] FOREIGN KEY([WorkItemId])
REFERENCES [dbo].[WorkItem] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WorkItemUser] CHECK CONSTRAINT [FK_WorkItemUser_WorkItem]
GO

ALTER TABLE [dbo].[WorkItemUser]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemUser_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WorkItemUser] CHECK CONSTRAINT [FK_WorkItemUser_User]
GO

