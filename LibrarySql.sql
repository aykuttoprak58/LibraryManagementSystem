USE [master]
GO
/****** Object:  Database [LIBRARY]    Script Date: 02/07/2024 16:07:08 ******/
CREATE DATABASE [LIBRARY]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LIBRARY', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LIBRARY.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LIBRARY_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LIBRARY_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LIBRARY] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LIBRARY].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LIBRARY] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LIBRARY] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LIBRARY] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LIBRARY] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LIBRARY] SET ARITHABORT OFF 
GO
ALTER DATABASE [LIBRARY] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LIBRARY] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LIBRARY] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LIBRARY] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LIBRARY] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LIBRARY] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LIBRARY] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LIBRARY] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LIBRARY] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LIBRARY] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LIBRARY] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LIBRARY] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LIBRARY] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LIBRARY] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LIBRARY] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LIBRARY] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LIBRARY] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LIBRARY] SET RECOVERY FULL 
GO
ALTER DATABASE [LIBRARY] SET  MULTI_USER 
GO
ALTER DATABASE [LIBRARY] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LIBRARY] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LIBRARY] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LIBRARY] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LIBRARY] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LIBRARY] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LIBRARY', N'ON'
GO
ALTER DATABASE [LIBRARY] SET QUERY_STORE = ON
GO
ALTER DATABASE [LIBRARY] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LIBRARY]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[AuthorId] [int] IDENTITY(1,1) NOT NULL,
	[AuthorName] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK__Authors__70DAFC349A99E74C] PRIMARY KEY CLUSTERED 
(
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[BookId] [uniqueidentifier] NOT NULL,
	[BookName] [nvarchar](40) NOT NULL,
	[AuthorId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ReleaseDate] [int] NOT NULL,
	[BookStatus] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK__Books__3DE0C207F418721E] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BorrowedBooks]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BorrowedBooks](
	[BarrowedBookId] [uniqueidentifier] NOT NULL,
	[BarrowedBookName] [nvarchar](max) NOT NULL,
	[MemberFullName] [nvarchar](20) NOT NULL,
	[IdentityNumber] [nvarchar](11) NOT NULL,
	[BorrowedDate] [date] NOT NULL,
	[ReturnDate] [date] NOT NULL,
 CONSTRAINT [PK_BorrowedBooks] PRIMARY KEY CLUSTERED 
(
	[BarrowedBookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK__Categori__19093A0B3EE897F2] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[MemberId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](15) NOT NULL,
	[Surname] [nvarchar](15) NOT NULL,
	[BirthDate] [date] NOT NULL,
	[IdentityNumber] [nvarchar](11) NOT NULL,
	[Gender] [nvarchar](15) NOT NULL,
	[MemberShipType] [nvarchar](15) NOT NULL,
	[MemberShipStatusId] [int] NOT NULL,
	[UserName] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[PasswordHash] [varbinary](max) NOT NULL,
	[PasswordSalt] [varbinary](max) NOT NULL,
	[PasswordResetToken] [nvarchar](max) NOT NULL,
	[ResetTokenExpires] [datetime] NOT NULL,
 CONSTRAINT [PK__Members__0CF04B185AFC318E] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberShipStatus]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberShipStatus](
	[MemberShipStatusId] [int] IDENTITY(1,1) NOT NULL,
	[MemberShipStatusName] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK__MemberSh__F49AE2B1610A796A] PRIMARY KEY CLUSTERED 
(
	[MemberShipStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberStatus]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberStatus](
	[MemberId] [uniqueidentifier] NOT NULL,
	[MemberFullName] [nvarchar](20) NOT NULL,
	[IdentityNumber] [nvarchar](max) NOT NULL,
	[BarrowedBookNumber] [int] NOT NULL,
	[ReturnedBookNumber] [int] NOT NULL,
	[PunishmentNumber] [int] NOT NULL,
	[PunishmentStatus] [nvarchar](max) NOT NULL,
	[PunishmentMailStatus] [bit] NOT NULL,
 CONSTRAINT [PK_MemberStatus] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnedBooks]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnedBooks](
	[ReturnedBookId] [uniqueidentifier] NOT NULL,
	[ReturnedBookName] [nvarchar](max) NOT NULL,
	[MemberFullName] [nvarchar](20) NOT NULL,
	[IdentityNumber] [nvarchar](11) NOT NULL,
	[ReturnDate] [date] NOT NULL,
	[Deadline] [date] NOT NULL,
	[PunishmentStatus] [nvarchar](40) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 02/07/2024 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[SettingId] [int] NOT NULL,
	[SettingName] [varchar](50) NOT NULL,
	[SettingStatus] [bit] NOT NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Authors] ON 

INSERT [dbo].[Authors] ([AuthorId], [AuthorName]) VALUES (1, N'Orhan Pamuk')
INSERT [dbo].[Authors] ([AuthorId], [AuthorName]) VALUES (2, N'Yaşar Kemal ')
INSERT [dbo].[Authors] ([AuthorId], [AuthorName]) VALUES (3, N'Elif Şafak')
INSERT [dbo].[Authors] ([AuthorId], [AuthorName]) VALUES (4, N'Aziz Nesin')
INSERT [dbo].[Authors] ([AuthorId], [AuthorName]) VALUES (5, N'Cemal Süreya')
SET IDENTITY_INSERT [dbo].[Authors] OFF
GO
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'369ae430-42c5-49c0-9905-0297c90b2f9d', N'Kara Kitap', 1, 1, 1990, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'3dfb5893-9953-4385-b276-06c0a5ed5f75', N'İnce Memed', 2, 1, 1955, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'72723f3b-6a25-45e0-82f7-0d8a1027f87b', N'Aşk', 3, 1, 2009, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'2ee9b139-37ed-47a7-9da7-1eb6f0a8b36a', N'Sürü', 4, 1, 1970, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'a57a1341-7ef4-48d6-b58c-3ba88b14a434', N'Gelincik Tarlası', 5, 1, 1967, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'28ed54d0-9982-4f9c-a3e1-3c38c7e5f42c', N'Öteki Renkler', 1, 2, 1999, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'88c7d067-f296-4f5d-9373-472b6346b5a0', N'Teneke', 2, 2, 1955, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'7d701c4a-1174-4297-bd1b-4e51c77f5358', N'Mucizevi Mandarin', 3, 2, 2002, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'd1c0a72b-8fd6-45cb-b7af-4f74207a5032', N'Şimdiki Çocuklar Harika', 4, 2, 1952, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'f3934cfc-693f-4a4c-a41b-584f8e9e4f51', N'Güz Bitiği', 5, 2, 1972, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'd1aa39a2-4f37-45ff-9d85-6030ec8f25a4', N'Öteki Renkler', 1, 3, 1999, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'729932f6-7b6a-4f73-b556-62de1212d5f1', N'Kırlangıçlar Sonra Uçar', 2, 3, 1962, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'82b120bc-b6bc-4729-9a9d-643f92fcbe9d', N'Bir Şeyi Fark Etmiş Olmak', 3, 3, 2012, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'4e1aa60f-0020-497c-b2a7-6b12b7a14e62', N'Yaşar''la Söyleşi', 4, 3, 1970, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'5f7367ff-5970-4a9d-97f7-6c2a5b2cb47b', N'Güz Bitiği', 5, 3, 1972, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'0367e80c-4c09-45c0-9c8e-6f3993f37d59', N'Kar', 1, 4, 2007, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'ceb5d3e4-03a1-4c45-a85c-874fd6340b0e', N'Ölmez Otu', 2, 4, 1971, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'bce68c3e-20be-4a0c-bb4b-89a617759727', N'Aşk', 3, 4, 2005, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'9901f1e3-6232-4d2a-a4c3-8bc74c312491', N'Sevda Sözleri', 4, 4, 1987, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'd7aa9e52-9121-40d0-a14d-9446fc4ab77c', N'Beni Öp Sonra Doğur Beni', 5, 4, 1973, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'8e0265b5-4f30-479b-ba20-98e50c31ee5c', N'Benim Adım Kırmızı', 1, 5, 1998, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'3de4b0e7-48c2-4d2f-89ee-c8565f017c76', N'Demirciler Çarşısı Cinayeti', 2, 5, 1951, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'e2f4b32f-3c4d-4de7-8378-cf85a15253d8', N'Şemspare', 3, 5, 2009, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'8dd32795-42e1-482f-939b-ee4b584954cc', N'Karahan Tatilde', 4, 5, 2021, N'Rafta')
INSERT [dbo].[Books] ([BookId], [BookName], [AuthorId], [CategoryId], [ReleaseDate], [BookStatus]) VALUES (N'b200744f-1b2e-4a1f-bcfc-f78c2c418f1b', N'Gelincik Tarlası', 5, 5, 1967, N'Rafta')
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES (1, N'Roman')
INSERT [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES (2, N'Polisiye')
INSERT [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES (3, N'Korku')
INSERT [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES (4, N'Drama')
INSERT [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES (5, N'Macera')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
INSERT [dbo].[Members] ([MemberId], [Name], [Surname], [BirthDate], [IdentityNumber], [Gender], [MemberShipType], [MemberShipStatusId], [UserName], [Email], [PasswordHash], [PasswordSalt], [PasswordResetToken], [ResetTokenExpires]) VALUES (N'41384558-3371-4a97-b62a-059ffb7e07cc', N'Karahan', N'Toprak', CAST(N'1993-10-12' AS Date), N'40483998852', N'Male', N'Admin', 1, N'karahan58', N'aykut58toprak@gmail.com', 0xC9A0DDC04F4648D14FFC2898AC2CFCAC42ED1793199116CB25AE7368B0FBF51038FE8EFFB6F7774E6E6A16D0A92D85A73EF899F4B52DCAD9EBE353966C5A570D, 0x29DC0D27C9273445D87AFEE3638892ABF3AAEBA027BB6C98705540D834FE5DC8C23822625D0AD17A781EB98BC375ADDF73A6770443D86DEC7C1FB9660FA3861A6D2B47B99ECB825FD6AA31B945533348007AFD167CC820E8BE3C3DBE404246384092248ABDD54BAC79E338692F05C847A0EE14FF11972199EB9750C6BA793F95, N'', CAST(N'2000-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Members] ([MemberId], [Name], [Surname], [BirthDate], [IdentityNumber], [Gender], [MemberShipType], [MemberShipStatusId], [UserName], [Email], [PasswordHash], [PasswordSalt], [PasswordResetToken], [ResetTokenExpires]) VALUES (N'2f8b889c-e245-4a36-bda9-2f1d09e0d3b0', N'Berk', N'Toprak', CAST(N'2009-10-12' AS Date), N'40483998859', N'Male', N'Student', 1, N'berk58', N'berk58toprak@gmail.com', 0x2C17A8C3C2AD110AE811B71CB9E45C763A2D72E3C213A0C9AEF8EDCB5CBAD4FB122E2F04C48BC809352745B9E4B26126CD423DA5A9339F65A8B01E8FB124D305, 0xD4BEE502EEEDD857D3CA8A5B21BD4403EA0F2282C8C789DDB8E39122E322CC982BEC0042B21548B39F751BFB0DF3C60676B1C714FD87CE6723EE1649703A111CCE5CA3D80CE0B0CEBE295479066994377DA5CDE7BA7506160A62122E3E450EC9E4E64E03B5A848138E2BC7880CFA7D407D257C737C042F28690F842DECDF3F0D, N'', CAST(N'2000-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Members] ([MemberId], [Name], [Surname], [BirthDate], [IdentityNumber], [Gender], [MemberShipType], [MemberShipStatusId], [UserName], [Email], [PasswordHash], [PasswordSalt], [PasswordResetToken], [ResetTokenExpires]) VALUES (N'96e76a2b-dec7-421f-9ee7-b70985c13b0f', N'Ahmet', N'Toprak', CAST(N'1969-10-12' AS Date), N'40483998854', N'Male', N'Citizen', 1, N'ahmet58', N'ahmet58toprak@gmail.com', 0x286FD9A82AE6E376CEFA17028BE39CB0684BA87C1722A5A22078BB70A421267B97ED33C1B97A74BAB8349BF48DBB8802E0BBA15C9DCB866D0990347EF4806A28, 0x4429638783EF34B62A9A6CE249ADCFD3ADE3386D50D316BC5659195ACC10BAEC4F175A0CD71554A2DF68055663E55F57C768EDAD5E81C6B02EB84B0AED5167CC69272BB78D46DCD7C3986AB670EE7879A27796AC730A2D8E9617E707A327E3CE1D36C3CB1C1A43E000BA74CCAE1FBC67594AF021C503910BF64F66076AC77C08, N'', CAST(N'2000-01-01T00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[MemberShipStatus] ON 

INSERT [dbo].[MemberShipStatus] ([MemberShipStatusId], [MemberShipStatusName]) VALUES (1, N'Aktif')
INSERT [dbo].[MemberShipStatus] ([MemberShipStatusId], [MemberShipStatusName]) VALUES (2, N'Pasif')
INSERT [dbo].[MemberShipStatus] ([MemberShipStatusId], [MemberShipStatusName]) VALUES (3, N'İptal Edildi')
SET IDENTITY_INSERT [dbo].[MemberShipStatus] OFF
GO
INSERT [dbo].[MemberStatus] ([MemberId], [MemberFullName], [IdentityNumber], [BarrowedBookNumber], [ReturnedBookNumber], [PunishmentNumber], [PunishmentStatus], [PunishmentMailStatus]) VALUES (N'41384558-3371-4a97-b62a-059ffb7e07cc', N'Aykut Toprak', N'40483998852', 0, 0, 0, N'Ceza Yok', 1)
INSERT [dbo].[MemberStatus] ([MemberId], [MemberFullName], [IdentityNumber], [BarrowedBookNumber], [ReturnedBookNumber], [PunishmentNumber], [PunishmentStatus], [PunishmentMailStatus]) VALUES (N'2f8b889c-e245-4a36-bda9-2f1d09e0d3b0', N'Berk Toprak', N'40483998859', 0, 0, 0, N'Ceza Yok', 1)
INSERT [dbo].[MemberStatus] ([MemberId], [MemberFullName], [IdentityNumber], [BarrowedBookNumber], [ReturnedBookNumber], [PunishmentNumber], [PunishmentStatus], [PunishmentMailStatus]) VALUES (N'96e76a2b-dec7-421f-9ee7-b70985c13b0f', N'Ahmet Toprak', N'40483998854', 0, 0, 0, N'Ceza Yok', 1)
GO
INSERT [dbo].[ReturnedBooks] ([ReturnedBookId], [ReturnedBookName], [MemberFullName], [IdentityNumber], [ReturnDate], [Deadline], [PunishmentStatus]) VALUES (N'fb5c4396-af5e-4fc2-81b3-d0d6f9b3422c', N'İnce Memed', N'Berk Toprak', N'40483998859', CAST(N'2024-07-02' AS Date), CAST(N'2024-08-01' AS Date), N'Zamanında Teslim Edildi')
INSERT [dbo].[ReturnedBooks] ([ReturnedBookId], [ReturnedBookName], [MemberFullName], [IdentityNumber], [ReturnDate], [Deadline], [PunishmentStatus]) VALUES (N'5ab1e47f-c377-462b-9d8e-9f30766deefa', N'Sürü', N'Berk Toprak', N'40483998859', CAST(N'2024-07-02' AS Date), CAST(N'2024-08-01' AS Date), N'Zamanında Teslim Edildi')
INSERT [dbo].[ReturnedBooks] ([ReturnedBookId], [ReturnedBookName], [MemberFullName], [IdentityNumber], [ReturnDate], [Deadline], [PunishmentStatus]) VALUES (N'79df57fa-ea41-4528-bb97-b3c980b8bb23', N'Aşk', N'Berk Toprak', N'40483998859', CAST(N'2024-07-02' AS Date), CAST(N'2024-08-01' AS Date), N'Zamanında Teslim Edildi')
INSERT [dbo].[ReturnedBooks] ([ReturnedBookId], [ReturnedBookName], [MemberFullName], [IdentityNumber], [ReturnDate], [Deadline], [PunishmentStatus]) VALUES (N'f487bfe4-6ea5-4e6b-8e51-c72ba114c9c7', N'Öteki Renkler', N'Berk Toprak', N'40483998859', CAST(N'2024-07-02' AS Date), CAST(N'2024-08-01' AS Date), N'Zamanında Teslim Edildi')
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingStatus]) VALUES (1, N'7 GÜN ÖNCE İADE HATIRLATMA MESAJI', 1)
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingStatus]) VALUES (2, N'SON GÜN İADE HATIRLATMA MESAJI', 1)
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingStatus]) VALUES (3, N'GEÇ İADE CEZA MESAJI', 1)
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingStatus]) VALUES (4, N'2 KEZ ÜST ÜSTE GEÇ İADE CEZA MESAJI', 1)
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingStatus]) VALUES (5, N'4 KEZ ÜST ÜSTE GEÇ İADE CEZA MESAJI', 1)
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingStatus]) VALUES (6, N'ŞİFREMİ UNUTTUM', 1)
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingStatus]) VALUES (7, N'ANA AYAR', 1)
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK__Books__AuthorId__4316F928] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([AuthorId])
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK__Books__AuthorId__4316F928]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK__Books__CategoryI__440B1D61] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK__Books__CategoryI__440B1D61]
GO
ALTER TABLE [dbo].[Members]  WITH CHECK ADD  CONSTRAINT [FK__Members__MemberS__3C69FB99] FOREIGN KEY([MemberShipStatusId])
REFERENCES [dbo].[MemberShipStatus] ([MemberShipStatusId])
GO
ALTER TABLE [dbo].[Members] CHECK CONSTRAINT [FK__Members__MemberS__3C69FB99]
GO
USE [master]
GO
ALTER DATABASE [LIBRARY] SET  READ_WRITE 
GO
