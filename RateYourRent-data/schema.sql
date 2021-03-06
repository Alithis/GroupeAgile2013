USE [notetaloc]
GO
/****** Object:  Table [dbo].[AdresseTable]    Script Date: 2013-05-29 11:55:41 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LocataireTable]    Script Date: 2013-05-29 11:55:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LocataireTable](
	[LocataireId] [int] NOT NULL,
	[UserId] [int] NULL,
	[AdresseId] [int] NULL,
	[PreuveDeDomicile] [bit] NULL,
	[PreuveType] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[LocataireId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NoteTable]    Script Date: 2013-05-29 11:55:42 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProprietaireTable]    Script Date: 2013-05-29 11:55:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProprietaireTable](
	[ProprietaireId] [int] NOT NULL,
	[UserId] [int] NULL,
	[AdresseId] [int] NULL,
	[PreuveDeProp] [bit] NULL,
	[PreuveType] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProprietaireId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SampleDroppableTable]    Script Date: 2013-05-29 11:55:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SampleDroppableTable](
	[Adresse] [varchar](255) NOT NULL,
	[Note] [int] NULL,
 CONSTRAINT [PK_SampleDroppableTable] PRIMARY KEY CLUSTERED 
(
	[Adresse] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserTable]    Script Date: 2013-05-29 11:55:42 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[LocataireTable]  WITH CHECK ADD FOREIGN KEY([AdresseId])
REFERENCES [dbo].[AdresseTable] ([AdresseId])
GO
ALTER TABLE [dbo].[LocataireTable]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserTable] ([UserId])
GO
ALTER TABLE [dbo].[NoteTable]  WITH CHECK ADD FOREIGN KEY([AdresseId])
REFERENCES [dbo].[AdresseTable] ([AdresseId])
GO
ALTER TABLE [dbo].[NoteTable]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserTable] ([UserId])
GO
ALTER TABLE [dbo].[ProprietaireTable]  WITH CHECK ADD FOREIGN KEY([AdresseId])
REFERENCES [dbo].[AdresseTable] ([AdresseId])
GO
ALTER TABLE [dbo].[ProprietaireTable]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserTable] ([UserId])
GO
