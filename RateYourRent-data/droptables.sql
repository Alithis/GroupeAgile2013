USE [NoteTaLoc]
GO

ALTER TABLE [dbo].[NoteTable] DROP CONSTRAINT [FK__NoteTable__UserI__145C0A3F]
GO

ALTER TABLE [dbo].[NoteTable] DROP CONSTRAINT [FK__NoteTable__Adres__15502E78]
GO

/****** Object:  Table [dbo].[NoteTable]    Script Date: 2013-05-29 12:21:37 ******/
DROP TABLE [dbo].[NoteTable]
GO

/****** Object:  Table [dbo].[NoteTable]    Script Date: 2013-05-29 12:21:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[NoteTable](
	[NoteId] [int] NOT NULL,
	[UserId] [int] NULL,
	[AdresseId] [int] NULL,
	[Note] [int] NULL,
	[StatutNote] [int] NULL,
	[Commentaire] [varchar](500) NULL,
	[StatutComment] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AdresseTable]    Script Date: 2013-05-29 12:28:57 ******/
DROP TABLE [dbo].[AdresseTable]
GO

/****** Object:  Table [dbo].[AdresseTable]    Script Date: 2013-05-29 12:28:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AdresseTable](
	[AdresseId] [int] NOT NULL,
	[AptNo] [varchar](50) NULL,
	[RueNo] [int] NULL,
	[Rue] [varchar](100) NULL,
	[Ville] [varchar](100) NULL,
	[Province] [varchar](100) NULL,
	[CodePostal] [varchar](10) NULL,
	[Pays] [varchar](100) NULL,
	[GeoCodeResponse] [varchar](255) NULL,
	[Longitude] [decimal](9, 6) NULL,
	[Lattitude] [decimal](9, 6) NULL,
PRIMARY KEY CLUSTERED 
(
	[AdresseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UserTable]    Script Date: 2013-05-29 12:33:48 ******/
DROP TABLE [dbo].[UserTable]
GO

/****** Object:  Table [dbo].[UserTable]    Script Date: 2013-05-29 12:33:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserTable](
	[UserId] [int] NOT NULL,
	[Nom] [varchar](50) NOT NULL,
	[Prenom] [varchar](50) NOT NULL,
	[Pseudo] [varchar](50) NOT NULL,
	[MotDePasse] [varchar](255) NOT NULL,
	[Courriel] [varchar](255) NULL,
	[DateCreer] [datetime] NULL,
	[DateMod] [datetime] NULL,
	[Statut] [int] NULL,
	[Commentaire] [varchar](500) NULL,
	[UserType] [int] NULL,
	[InscriptionConfirm] [bit] NULL,
	[SiteCondAccept] [bit] NULL,
	[AllowPubContact] [bit] NULL,
	[ValidationToken] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO







SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[NoteTable]  WITH CHECK ADD FOREIGN KEY([AdresseId])
REFERENCES [dbo].[AdresseTable] ([AdresseId])
GO

ALTER TABLE [dbo].[NoteTable]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserTable] ([UserId])
GO

