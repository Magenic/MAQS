CREATE DATABASE MagenicAutomation;
GO
USE MagenicAutomation;
GO
CREATE TABLE [dbo].[States](
	[StateID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[StateName] [nvarchar](max) NOT NULL,
	[StateAbbreviation] [nvarchar](2) NULL
);
GO