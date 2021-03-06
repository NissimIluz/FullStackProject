USE [master]
GO
/****** Object:  Database [FinalProjectDB]    Script Date: 26/07/2021 13:07:37 ******/
CREATE DATABASE [FinalProjectDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FinalProjectDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\FinalProjectDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FinalProjectDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\FinalProjectDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [FinalProjectDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FinalProjectDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FinalProjectDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FinalProjectDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FinalProjectDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FinalProjectDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FinalProjectDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FinalProjectDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FinalProjectDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FinalProjectDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FinalProjectDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FinalProjectDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FinalProjectDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FinalProjectDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FinalProjectDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FinalProjectDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FinalProjectDB] SET  MULTI_USER 
GO
ALTER DATABASE [FinalProjectDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FinalProjectDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FinalProjectDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FinalProjectDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FinalProjectDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FinalProjectDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [FinalProjectDB] SET QUERY_STORE = OFF
GO
USE [FinalProjectDB]
GO
/****** Object:  Table [dbo].[DocumentMarkers]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentMarkers](
	[MarkerID] [nvarchar](50) NOT NULL,
	[MarkerType] [nvarchar](50) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[DocumentID] [nvarchar](50) NOT NULL,
	[MarkerLocation] [nvarchar](50) NOT NULL,
	[StrokeColor] [nvarchar](50) NOT NULL,
	[FillColor] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DocumentMarkers] PRIMARY KEY CLUSTERED 
(
	[MarkerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documents]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents](
	[UserID] [nvarchar](50) NOT NULL,
	[ImageURL] [nvarchar](500) NOT NULL,
	[DocumentName] [nvarchar](50) NOT NULL,
	[DocumentID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[DocumentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SharedDocuments]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SharedDocuments](
	[DocumentID] [nvarchar](50) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
 CONSTRAINT [IX_SharedDocuments] UNIQUE NONCLUSTERED 
(
	[DocumentID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Subscribe] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Subscribe]  DEFAULT ((1)) FOR [Subscribe]
GO
ALTER TABLE [dbo].[Documents]  WITH CHECK ADD  CONSTRAINT [FK_Documents_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Documents] CHECK CONSTRAINT [FK_Documents_Users]
GO
ALTER TABLE [dbo].[SharedDocuments]  WITH CHECK ADD FOREIGN KEY([DocumentID])
REFERENCES [dbo].[Documents] ([DocumentID])
GO
ALTER TABLE [dbo].[SharedDocuments]  WITH CHECK ADD FOREIGN KEY([DocumentID])
REFERENCES [dbo].[Documents] ([DocumentID])
GO
ALTER TABLE [dbo].[SharedDocuments]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SharedDocuments]  WITH CHECK ADD  CONSTRAINT [FK_SharedDocuments_Documents] FOREIGN KEY([DocumentID])
REFERENCES [dbo].[Documents] ([DocumentID])
GO
ALTER TABLE [dbo].[SharedDocuments] CHECK CONSTRAINT [FK_SharedDocuments_Documents]
GO
ALTER TABLE [dbo].[SharedDocuments]  WITH CHECK ADD  CONSTRAINT [FK_SharedDocuments_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SharedDocuments] CHECK CONSTRAINT [FK_SharedDocuments_Users]
GO
ALTER TABLE [dbo].[SharedDocuments]  WITH CHECK ADD  CONSTRAINT [FK_UserDocuments] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SharedDocuments] CHECK CONSTRAINT [FK_UserDocuments]
GO
/****** Object:  StoredProcedure [dbo].[ChangeMarkerColor]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ChangeMarkerColor] 
	-- Add the parameters for the stored procedure here
	@MarkerID nvarchar(50),
	@StrokeColor  nvarchar(50),
	@FillColor nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE DocumentMarkers
	SET StrokeColor = @StrokeColor, FillColor = @FillColor
	WHERE MarkerID = @MarkerID;

	select @@rowcount
END
GO
/****** Object:  StoredProcedure [dbo].[CreateDocument]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateDocument]
	-- Add the parameters for the stored procedure here
	@ImageURL nvarchar(500),
	@DocumentID nvarchar(50),
	@DocumentName nvarchar(50),
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into Documents values (@UserID, @ImageURL, @DocumentName, @DocumentID) 
END
GO
/****** Object:  StoredProcedure [dbo].[CreateMarker]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateMarker]
	-- Add the parameters for the stored procedure here
	@DocumentID nvarchar(50),
	@MarkerID nvarchar(50),
	@MarkerType nvarchar(50),
	@MarkerLocation nvarchar(50),
	@StrokeColor nvarchar(50),
	@FillColor nvarchar(50),
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into DocumentMarkers values (@MarkerID, @MarkerType, @UserID, @DocumentID, @MarkerLocation,@StrokeColor,@FillColor) 
END
GO
/****** Object:  StoredProcedure [dbo].[CreateShare]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateShare]
	-- Add the parameters for the stored procedure here
	@DocumentID nvarchar(50),
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into SharedDocuments values (@DocumentID, @UserID)
END
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateUser]
	-- Add the parameters for the stored procedure here
	@UserID nvarchar(50),
	@UserName nvarchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into Users values (@UserID,@UserName,1)
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllOtherUsers]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[GetAllOtherUsers]
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT UserID from Users where NOT UserID = @UserID AND Subscribe = 1
END
GO
/****** Object:  StoredProcedure [dbo].[GetDocumentByID]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetDocumentByID] 
	-- Add the parameters for the stored procedure here
	@DocumentID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Documents where DocumentID = @DocumentID
END
GO
/****** Object:  StoredProcedure [dbo].[GetMarkers]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMarkers] 
	-- Add the parameters for the stored procedure here
		@DocumentID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from DocumentMarkers where DocumentID = @DocumentID
END
GO
/****** Object:  StoredProcedure [dbo].[GetNotSharedWithUsers]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetNotSharedWithUsers] 
	-- Add the parameters for the stored procedure here
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DocumentID, DocumentName from Documents where UserID = @UserID and DocumentID not in 
	 (select DocumentID from SharedDocuments) order by DocumentID

	--select SharedDocuments.DocumentID,DocumentName,SharedDocuments.UserID from SharedDocuments, Documents where SharedDocuments.DocumentID = Documents.DocumentID and Documents.UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[GetSharedByUser]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSharedByUser] 
	-- Add the parameters for the stored procedure here
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	-- select * from SharedDocuments where DocumentID in (SELECT DocumentID from Documents where UserID = @UserID)

	select SharedDocuments.DocumentID,DocumentName,SharedDocuments.UserID 
	from SharedDocuments, Documents 
	where SharedDocuments.DocumentID = Documents.DocumentID and Documents.UserID = @UserID
	order by DocumentID
END
GO
/****** Object:  StoredProcedure [dbo].[GetSharedDocuments]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSharedDocuments]
	-- Add the parameters for the stored procedure here
		@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

        -- Insert statements for procedure here
	SELECT SharedDocuments.DocumentID,DocumentName FROM SharedDocuments,Documents WHERE SharedDocuments.UserID=@UserID and 
	SharedDocuments.DocumentID = Documents.DocumentID
END
GO
/****** Object:  StoredProcedure [dbo].[GetUser]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUser]
	-- Add the parameters for the stored procedure here
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Users where UserId= @UserID and Subscribe =1
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserDocuments]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserDocuments]
	-- Add the parameters for the stored procedure here
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from SharedDocuments where DocumentID in(
	SELECT DocumentID from Documents where UserID = @UserID
	)
END
GO
/****** Object:  StoredProcedure [dbo].[MarkUserAsRemoved]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MarkUserAsRemoved]
	-- Add the parameters for the stored procedure here
	@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update Subscribe into 0 if you want to remove the user.
	UPDATE Users set Subscribe = 0 where UserId = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveDocument]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveDocument] 
	-- Add the parameters for the stored procedure here
	@DocumentID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE from SharedDocuments where DocumentID = @DocumentID
	DELETE from Documents where DocumentID = @DocumentID
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveMarker]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveMarker]
	-- Add the parameters for the stored procedure here
	@MarkerID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE from DocumentMarkers where MarkerID = @MarkerID
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveShare]    Script Date: 26/07/2021 13:07:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveShare]
	-- Add the parameters for the stored procedure here
		@DocumentID nvarchar(50),
		@UserID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE from SharedDocuments where DocumentID = @DocumentID and UserID = @UserID
END
GO
USE [master]
GO
ALTER DATABASE [FinalProjectDB] SET  READ_WRITE 
GO
