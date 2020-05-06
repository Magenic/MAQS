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
CREATE TABLE [dbo].[Cities](
	[CityID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[CityName] [nvarchar](max) NOT NULL,
	[CityPopulation] [decimal](18, 2) 
);
GO
CREATE TABLE [dbo].[Datatype](
	[bitintType] [bigint] NULL,
	[bitType] [bit] NULL,
	[char10Type] [char](10) NULL,
	[dateType] [date] NULL,
	[dateTimeType] [datetime] NULL,
	[floatType] [float] NULL,
	[intType] [int] NULL,
	[ncharType] [nchar](10) NULL,
	[nvarcharType] [nvarchar](50) NULL,
	[varcharType] [varchar](50) NULL,
	[decimalType] [decimal](18, 2) NULL,
	[xmlType] [xml] NULL
);
GO