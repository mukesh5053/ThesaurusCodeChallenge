USE [master]
GO
 

IF  DB_ID('Thesaurus') IS NOT NULL
begin
DROP DATABASE [Thesaurus]
end

/****** Object:  Database [Thesaurus]    Script Date: 2021-08-15 20:17:48 ******/

GO

/****** Object:  Database [Thesaurus]    Script Date: 2021-08-15 20:17:48 ******/
CREATE DATABASE [Thesaurus]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Thesaurus', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Thesaurus.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Thesaurus_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Thesaurus_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

c

USE [Thesaurus]
GO

/****** Object:  Table [dbo].[TSynonyms]    Script Date: 2021-08-15 20:18:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSynonyms]') AND type in (N'U'))
DROP TABLE [dbo].[TSynonyms]
GO

/****** Object:  Table [dbo].[TSynonyms]    Script Date: 2021-08-15 20:18:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TSynonyms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Synonyms] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_TSynonyms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO


