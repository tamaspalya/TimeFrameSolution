USE [dmai0920_1086318]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Project] (
[Id] [int] IDENTITY (1,1) NOT NULL, 
[TeamID] [int] NOT NULL,
[ProjectName] [nvarchar](100) NOT NULL,
[ProjectDescription] [nvarchar] (MAX) 
CONSTRAINT [FK_Project_Team] FOREIGN KEY([TeamID])
REFERENCES [Team] ([Id])
CONSTRAINT [PK_Project1] PRIMARY KEY CLUSTERED 
([Id] ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


GO