USE [bus]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 06/19/2014 18:10:38 ******/
DROP TABLE [dbo].[Address]
GO
/****** Object:  Table [dbo].[Bus]    Script Date: 06/19/2014 18:10:39 ******/
ALTER TABLE [dbo].[Bus] DROP CONSTRAINT [DF_Bus_DelFlag]
GO
DROP TABLE [dbo].[Bus]
GO
/****** Object:  Table [dbo].[BusLine]    Script Date: 06/19/2014 18:10:39 ******/
ALTER TABLE [dbo].[BusLine] DROP CONSTRAINT [DF_BusLine_DelFlag]
GO
DROP TABLE [dbo].[BusLine]
GO
/****** Object:  Table [dbo].[Driver]    Script Date: 06/19/2014 18:10:39 ******/
ALTER TABLE [dbo].[Driver] DROP CONSTRAINT [DF_Driver_DelFlag]
GO
DROP TABLE [dbo].[Driver]
GO
/****** Object:  Table [dbo].[LineUser]    Script Date: 06/19/2014 18:10:39 ******/
ALTER TABLE [dbo].[LineUser] DROP CONSTRAINT [DF_LineUser_DelFlag]
GO
DROP TABLE [dbo].[LineUser]
GO
/****** Object:  Table [dbo].[Manager]    Script Date: 06/19/2014 18:10:39 ******/
ALTER TABLE [dbo].[Manager] DROP CONSTRAINT [DF_Manager_DelFlag]
GO
DROP TABLE [dbo].[Manager]
GO
/****** Object:  Table [dbo].[MyContent]    Script Date: 06/19/2014 18:10:39 ******/
DROP TABLE [dbo].[MyContent]
GO
/****** Object:  Table [dbo].[News]    Script Date: 06/19/2014 18:10:39 ******/
DROP TABLE [dbo].[News]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 06/19/2014 18:10:39 ******/
DROP TABLE [dbo].[Order]
GO
/****** Object:  Table [dbo].[Pay]    Script Date: 06/19/2014 18:10:39 ******/
ALTER TABLE [dbo].[Pay] DROP CONSTRAINT [DF_Pay_DelFlag]
GO
DROP TABLE [dbo].[Pay]
GO
/****** Object:  Table [dbo].[PayMent]    Script Date: 06/19/2014 18:10:39 ******/
DROP TABLE [dbo].[PayMent]
GO
/****** Object:  Table [dbo].[Question]    Script Date: 06/19/2014 18:10:39 ******/
DROP TABLE [dbo].[Question]
GO
/****** Object:  Table [dbo].[QuestionItem]    Script Date: 06/19/2014 18:10:39 ******/
DROP TABLE [dbo].[QuestionItem]
GO
/****** Object:  Table [dbo].[QuestionItem2]    Script Date: 06/19/2014 18:10:40 ******/
DROP TABLE [dbo].[QuestionItem2]
GO
/****** Object:  Table [dbo].[SMSCode]    Script Date: 06/19/2014 18:10:40 ******/
DROP TABLE [dbo].[SMSCode]
GO
/****** Object:  Table [dbo].[Station]    Script Date: 06/19/2014 18:10:40 ******/
DROP TABLE [dbo].[Station]
GO
/****** Object:  Table [dbo].[SysCode]    Script Date: 06/19/2014 18:10:40 ******/
DROP TABLE [dbo].[SysCode]
GO
/****** Object:  Table [dbo].[UserQuestion]    Script Date: 06/19/2014 18:10:40 ******/
DROP TABLE [dbo].[UserQuestion]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 06/19/2014 18:10:40 ******/
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_DelFlag]
GO
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[UserState]    Script Date: 06/19/2014 18:10:40 ******/
DROP TABLE [dbo].[UserState]
GO
/****** Object:  Table [dbo].[WXUsers]    Script Date: 06/19/2014 18:10:40 ******/
DROP TABLE [dbo].[WXUsers]
GO
/****** Object:  Table [dbo].[WXUsers]    Script Date: 06/19/2014 18:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WXUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OpenID] [nvarchar](100) NOT NULL,
	[NickName] [nvarchar](100) NULL,
	[HeadImg] [nvarchar](300) NULL,
	[Sex] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_WXUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserState]    Script Date: 06/19/2014 18:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserState](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StateName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserState] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[UserState] ON
INSERT [dbo].[UserState] ([ID], [StateName]) VALUES (1, N'已预定用户')
SET IDENTITY_INSERT [dbo].[UserState] OFF
/****** Object:  Table [dbo].[Users]    Script Date: 06/19/2014 18:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WXUserID] [int] NOT NULL,
	[Names] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[AddressSel] [nvarchar](300) NULL,
	[Address] [nvarchar](500) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[StartLong] [float] NOT NULL,
	[StartLat] [float] NOT NULL,
	[EndLong] [float] NOT NULL,
	[EndLat] [float] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[isFinal] [bit] NOT NULL,
	[Sex] [int] NOT NULL,
	[EndAddressSel] [nvarchar](300) NULL,
	[EndAddress] [nvarchar](500) NULL,
	[ParentUserID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[EMail] [nvarchar](100) NULL,
	[QQ] [nvarchar](50) NULL,
	[CompanyName] [nvarchar](50) NULL,
	[Number] [nvarchar](50) NOT NULL,
	[UpdateTime] [datetime] NULL,
	[UpdateMngID] [int] NULL,
	[CreateMngID] [int] NULL,
	[DelFlag] [nchar](1) NOT NULL CONSTRAINT [DF_Users_DelFlag]  DEFAULT ('N'),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([ID], [WXUserID], [Names], [Password], [Phone], [AddressSel], [Address], [StartTime], [EndTime], [StartLong], [StartLat], [EndLong], [EndLat], [CreateTime], [isFinal], [Sex], [EndAddressSel], [EndAddress], [ParentUserID], [StateID], [EMail], [QQ], [CompanyName], [Number], [UpdateTime], [UpdateMngID], [CreateMngID], [DelFlag]) VALUES (1, 0, N'老朱', N'8EAE2FB9A7DC8A97', N'1111', NULL, N'理工大学', CAST(0x0000A34A008C1360 AS DateTime), CAST(0x0000A34A011826C0 AS DateTime), 0, 0, 0, 0, CAST(0x0000A34A0171F084 AS DateTime), 1, 2, NULL, N'11', 0, 0, N'222@qq', N'123456', N'你猜', N'140614001', NULL, NULL, NULL, N'N')
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Table [dbo].[UserQuestion]    Script Date: 06/19/2014 18:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserQuestion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[QuestionItem2ID] [int] NOT NULL,
	[QuestionItemID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserQuestion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysCode]    Script Date: 06/19/2014 18:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysCode](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Types] [nvarchar](5) NOT NULL,
	[Keys] [nvarchar](20) NOT NULL,
	[Names] [nvarchar](100) NOT NULL,
	[SortNo] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_SysCode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysCode] ON
INSERT [dbo].[SysCode] ([ID], [Types], [Keys], [Names], [SortNo], [CreateTime]) VALUES (2, N'MNG', N'MNG', N'管理员', 1, CAST(0x0000A34D00000000 AS DateTime))
INSERT [dbo].[SysCode] ([ID], [Types], [Keys], [Names], [SortNo], [CreateTime]) VALUES (5, N'MNG', N'OPT', N'操作员', 2, CAST(0x0000A34D00000000 AS DateTime))
INSERT [dbo].[SysCode] ([ID], [Types], [Keys], [Names], [SortNo], [CreateTime]) VALUES (6, N'MNG', N'USM', N'车长', 3, CAST(0x0000A34D00000000 AS DateTime))
INSERT [dbo].[SysCode] ([ID], [Types], [Keys], [Names], [SortNo], [CreateTime]) VALUES (9, N'MNG', N'MOT', N'监察员', 4, CAST(0x0000A34D00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[SysCode] OFF
/****** Object:  Table [dbo].[Station]    Script Date: 06/19/2014 18:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Station](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Names] [nvarchar](200) NOT NULL,
	[LineID] [int] NOT NULL,
	[Images] [nvarchar](100) NOT NULL,
	[SortID] [int] NOT NULL,
 CONSTRAINT [PK_Station] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Station] ON
INSERT [dbo].[Station] ([ID], [Names], [LineID], [Images], [SortID]) VALUES (1, N'站2', 3, N'/Upload/201401/201401062204411778.jpg', 2)
INSERT [dbo].[Station] ([ID], [Names], [LineID], [Images], [SortID]) VALUES (3, N'站11', 3, N'/Upload/201401/201401062204411778.jpg', 1)
SET IDENTITY_INSERT [dbo].[Station] OFF
/****** Object:  Table [dbo].[SMSCode]    Script Date: 06/19/2014 18:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SMSCode](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](12) NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[IsUse] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SMSCode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SMSCode] ON
INSERT [dbo].[SMSCode] ([ID], [Phone], [Code], [EndTime], [IsUse], [CreateTime]) VALUES (1, N'13170393542', N'559378', CAST(0x0000A2B70023484A AS DateTime), 0, CAST(0x0000A2B70020892A AS DateTime))
INSERT [dbo].[SMSCode] ([ID], [Phone], [Code], [EndTime], [IsUse], [CreateTime]) VALUES (2, N'13170393542', N'405283', CAST(0x0000A2B70025BAE7 AS DateTime), 0, CAST(0x0000A2B70022FBC7 AS DateTime))
INSERT [dbo].[SMSCode] ([ID], [Phone], [Code], [EndTime], [IsUse], [CreateTime]) VALUES (3, N'13170393542', N'7432', CAST(0x0000A2E9010B968D AS DateTime), 0, CAST(0x0000A2E90108D76D AS DateTime))
INSERT [dbo].[SMSCode] ([ID], [Phone], [Code], [EndTime], [IsUse], [CreateTime]) VALUES (4, N'13170393542', N'4170', CAST(0x0000A2E9010DE573 AS DateTime), 0, CAST(0x0000A2E9010B2653 AS DateTime))
INSERT [dbo].[SMSCode] ([ID], [Phone], [Code], [EndTime], [IsUse], [CreateTime]) VALUES (5, N'13170393542', N'6982', CAST(0x0000A2E9010E6EED AS DateTime), 0, CAST(0x0000A2E9010BAFCD AS DateTime))
INSERT [dbo].[SMSCode] ([ID], [Phone], [Code], [EndTime], [IsUse], [CreateTime]) VALUES (6, N'13170393542', N'2940', CAST(0x0000A2E9012B1A2D AS DateTime), 0, CAST(0x0000A2E901285B0D AS DateTime))
INSERT [dbo].[SMSCode] ([ID], [Phone], [Code], [EndTime], [IsUse], [CreateTime]) VALUES (7, N'18668312765', N'5356', CAST(0x0000A3300008DFD4 AS DateTime), 0, CAST(0x0000A330000620B4 AS DateTime))
INSERT [dbo].[SMSCode] ([ID], [Phone], [Code], [EndTime], [IsUse], [CreateTime]) VALUES (8, N'18668312765', N'6065', CAST(0x0000A330013FF95E AS DateTime), 0, CAST(0x0000A330013D3A3E AS DateTime))
SET IDENTITY_INSERT [dbo].[SMSCode] OFF
/****** Object:  Table [dbo].[QuestionItem2]    Script Date: 06/19/2014 18:10:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionItem2](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionItemID] [int] NOT NULL,
	[Value] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_QuestionItem2] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[QuestionItem2] ON
INSERT [dbo].[QuestionItem2] ([ID], [QuestionItemID], [Value]) VALUES (1, 2, N'好')
INSERT [dbo].[QuestionItem2] ([ID], [QuestionItemID], [Value]) VALUES (2, 2, N'不好')
INSERT [dbo].[QuestionItem2] ([ID], [QuestionItemID], [Value]) VALUES (3, 1, N'设计很好')
INSERT [dbo].[QuestionItem2] ([ID], [QuestionItemID], [Value]) VALUES (4, 1, N'设计一般')
INSERT [dbo].[QuestionItem2] ([ID], [QuestionItemID], [Value]) VALUES (5, 1, N'懒死啦')
INSERT [dbo].[QuestionItem2] ([ID], [QuestionItemID], [Value]) VALUES (6, 7, N'NO')
INSERT [dbo].[QuestionItem2] ([ID], [QuestionItemID], [Value]) VALUES (7, 7, N'OK')
SET IDENTITY_INSERT [dbo].[QuestionItem2] OFF
/****** Object:  Table [dbo].[QuestionItem]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionItem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](500) NOT NULL,
	[QuestionID] [int] NOT NULL,
 CONSTRAINT [PK_QuestionItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[QuestionItem] ON
INSERT [dbo].[QuestionItem] ([ID], [ItemName], [QuestionID]) VALUES (1, N'网页设计怎么样？', 1)
INSERT [dbo].[QuestionItem] ([ID], [ItemName], [QuestionID]) VALUES (2, N'用户体验怎么样？', 1)
INSERT [dbo].[QuestionItem] ([ID], [ItemName], [QuestionID]) VALUES (7, N'测试项目怎么样？', 1)
SET IDENTITY_INSERT [dbo].[QuestionItem] OFF
/****** Object:  Table [dbo].[Question]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Question] ON
INSERT [dbo].[Question] ([ID], [Title]) VALUES (1, N'关于网站的一些建议')
SET IDENTITY_INSERT [dbo].[Question] OFF
/****** Object:  Table [dbo].[PayMent]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayMent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LineID] [int] NOT NULL,
	[UsersID] [int] NOT NULL,
	[PayNames] [nvarchar](500) NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Money] [money] NOT NULL,
	[isUse] [bit] NOT NULL,
	[PayType] [nvarchar](50) NULL,
	[HeXiao] [nvarchar](50) NULL,
	[HXTime] [datetime] NULL,
	[Memo] [nvarchar](1000) NULL,
	[addUsers] [nvarchar](50) NULL,
	[CreateTime] [datetime] NOT NULL,
	[SZ] [int] NULL,
	[ZWQ] [int] NULL,
 CONSTRAINT [PK_PayMent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[PayMent] ON
INSERT [dbo].[PayMent] ([ID], [LineID], [UsersID], [PayNames], [StartTime], [EndTime], [Money], [isUse], [PayType], [HeXiao], [HXTime], [Memo], [addUsers], [CreateTime], [SZ], [ZWQ]) VALUES (1, 3, 9, N'7', CAST(0x0000A2AE00000000 AS DateTime), CAST(0x0000A2BD00000000 AS DateTime), 344.0000, 0, N'现金缴费', N'sdfsdf', CAST(0x0000A2B20001CF5D AS DateTime), N'', NULL, CAST(0x0000A2AE0016674D AS DateTime), NULL, NULL)
INSERT [dbo].[PayMent] ([ID], [LineID], [UsersID], [PayNames], [StartTime], [EndTime], [Money], [isUse], [PayType], [HeXiao], [HXTime], [Memo], [addUsers], [CreateTime], [SZ], [ZWQ]) VALUES (2, 0, 12, N'', CAST(0x0000A2B1018849C4 AS DateTime), CAST(0x0000A2B1018849C5 AS DateTime), 0.0000, 0, N'现金缴费', N'', CAST(0x0000A2B1018849C4 AS DateTime), N'', NULL, CAST(0x0000A2B1018849C4 AS DateTime), NULL, NULL)
INSERT [dbo].[PayMent] ([ID], [LineID], [UsersID], [PayNames], [StartTime], [EndTime], [Money], [isUse], [PayType], [HeXiao], [HXTime], [Memo], [addUsers], [CreateTime], [SZ], [ZWQ]) VALUES (3, 3, 12, N'sfsfsdfsd', CAST(0x0000A2B200000000 AS DateTime), CAST(0x0000A2C400000000 AS DateTime), 342.0000, 0, N'现金缴费', N'', CAST(0x0000A2B2000A25AB AS DateTime), N'', NULL, CAST(0x0000A2B200000000 AS DateTime), 1, 2)
INSERT [dbo].[PayMent] ([ID], [LineID], [UsersID], [PayNames], [StartTime], [EndTime], [Money], [isUse], [PayType], [HeXiao], [HXTime], [Memo], [addUsers], [CreateTime], [SZ], [ZWQ]) VALUES (4, 3, 12, N'dfgdfgd', CAST(0x0000A2A500000000 AS DateTime), CAST(0x0000A2BC00000000 AS DateTime), 0.0000, 0, N'现金缴费', N'', CAST(0x0000A2B201268685 AS DateTime), N'', NULL, CAST(0x0000A2BB00000000 AS DateTime), 1, 1)
SET IDENTITY_INSERT [dbo].[PayMent] OFF
/****** Object:  Table [dbo].[Pay]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pay](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LineUserID] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[PayTime] [datetime] NULL,
	[PayType] [nvarchar](10) NULL,
	[MangerID] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[LockFlag] [char](2) NULL,
	[Etc] [nvarchar](500) NULL,
	[CreateTime] [datetime] NOT NULL,
	[DelFlag] [nchar](1) NOT NULL CONSTRAINT [DF_Pay_DelFlag]  DEFAULT ('N'),
 CONSTRAINT [PK_Pay] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LineID] [int] NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[Names] [nvarchar](50) NOT NULL,
	[Months] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[States] [int] NOT NULL,
	[Memo] [nvarchar](500) NULL,
	[UserID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[PayTypeID] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Order] ON
INSERT [dbo].[Order] ([ID], [LineID], [Phone], [Names], [Months], [Price], [States], [Memo], [UserID], [CreateTime], [PayTypeID]) VALUES (4, 3, N'13170393542', N'马大爷', 1, 123.0000, 1, N'', 6, CAST(0x0000A28101732532 AS DateTime), 1)
INSERT [dbo].[Order] ([ID], [LineID], [Phone], [Names], [Months], [Price], [States], [Memo], [UserID], [CreateTime], [PayTypeID]) VALUES (5, 3, N'', N'马哥', 1, 123.0000, 1, N'', 13, CAST(0x0000A2EB016F39D2 AS DateTime), 3)
INSERT [dbo].[Order] ([ID], [LineID], [Phone], [Names], [Months], [Price], [States], [Memo], [UserID], [CreateTime], [PayTypeID]) VALUES (6, 3, N'18668312765', N'朱辉', 1, 123.0000, 1, N'', 14, CAST(0x0000A330013E52D7 AS DateTime), 2)
SET IDENTITY_INSERT [dbo].[Order] OFF
/****** Object:  Table [dbo].[News]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Thumb] [nvarchar](500) NULL,
	[SubContent] [nvarchar](1000) NULL,
	[Contents] [text] NULL,
	[CreateTime] [datetime] NOT NULL,
	[ClassID] [int] NOT NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[News] ON
INSERT [dbo].[News] ([ID], [Title], [Thumb], [SubContent], [Contents], [CreateTime], [ClassID]) VALUES (1, N'weasasd', N'ad', N'ad', N'asd', CAST(0x0000A27B002CF811 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[News] OFF
/****** Object:  Table [dbo].[MyContent]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyContent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[action] [nvarchar](50) NULL,
	[Title] [nvarchar](200) NULL,
	[Thumb] [nvarchar](200) NULL,
	[SubContent] [nvarchar](1000) NULL,
	[LinkUrl] [nvarchar](500) NULL,
	[Contents] [text] NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MyContent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[MyContent] ON
INSERT [dbo].[MyContent] ([ID], [action], [Title], [Thumb], [SubContent], [LinkUrl], [Contents], [CreateTime]) VALUES (1, N'reg_myprofile', N'感谢您的关注，马上注册成为会员。', N'/Upload/201311/201311122224571670.jpg', N'感谢您的关注，马上注册成为会员。', N'http://www.seobim.com/home/register', N'', CAST(0x0000A274017202BF AS DateTime))
SET IDENTITY_INSERT [dbo].[MyContent] OFF
/****** Object:  Table [dbo].[Manager]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manager](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](80) NOT NULL,
	[RealName] [nvarchar](60) NULL,
	[ManagerType] [nvarchar](20) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[DelFlag] [nchar](1) NOT NULL CONSTRAINT [DF_Manager_DelFlag]  DEFAULT ('N'),
 CONSTRAINT [PK_Manager1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Manager] ON
INSERT [dbo].[Manager] ([ID], [UserName], [Password], [RealName], [ManagerType], [CreateTime], [DelFlag]) VALUES (1, N'admin', N'277552F17787A0C4', N'老吴', N'MNG', CAST(0x0000A281011AD965 AS DateTime), N'N')
SET IDENTITY_INSERT [dbo].[Manager] OFF
/****** Object:  Table [dbo].[LineUser]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LineUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LineID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[RideType] [nvarchar](10) NOT NULL,
	[BusCard] [nvarchar](100) NULL,
	[BusCardNo] [nvarchar](100) NULL,
	[Names] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[StartAddress] [nvarchar](100) NULL,
	[EndAddress] [nvarchar](100) NULL,
	[EndTime] [datetime] NULL,
	[CreateTime] [datetime] NOT NULL,
	[Memo] [nvarchar](500) NULL,
	[QQ] [nvarchar](50) NULL,
	[EMail] [nvarchar](50) NULL,
	[StateID] [int] NOT NULL,
	[DelFlag] [nchar](1) NOT NULL CONSTRAINT [DF_LineUser_DelFlag]  DEFAULT ('N'),
 CONSTRAINT [PK_LineUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Driver]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Driver](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DriverName] [nvarchar](50) NOT NULL,
	[Sex] [int] NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[IdCard] [nvarchar](50) NOT NULL,
	[BirthDay] [datetime] NOT NULL,
	[DriveStartTime] [datetime] NOT NULL,
	[BreakRules] [nvarchar](500) NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[Etc] [nvarchar](500) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[DelFlag] [nchar](1) NOT NULL CONSTRAINT [DF_Driver_DelFlag]  DEFAULT ('N'),
 CONSTRAINT [PK_Driver] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusLine]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusLine](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LineName] [nvarchar](500) NOT NULL,
	[StartBusID] [int] NOT NULL,
	[StartAddressSel] [nvarchar](300) NULL,
	[StartAddress] [nvarchar](500) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[StartLong] [float] NOT NULL,
	[StartLat] [float] NOT NULL,
	[EndBusID] [int] NOT NULL,
	[EndAddressSel] [nvarchar](300) NULL,
	[EndAddress] [nvarchar](500) NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[EndLong] [float] NOT NULL,
	[EndLat] [float] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Price] [money] NOT NULL,
	[PriceMon] [money] NOT NULL,
	[PriceNgt] [money] NOT NULL,
	[MinNum] [int] NOT NULL,
	[TypeID] [int] NOT NULL,
	[Number] [nvarchar](50) NULL,
	[Price2] [money] NULL,
	[CheX] [nvarchar](50) NULL,
	[CheZ] [nvarchar](50) NULL,
	[LineType] [int] NULL,
	[DelFlag] [nchar](1) NOT NULL CONSTRAINT [DF_BusLine_DelFlag]  DEFAULT ('N'),
 CONSTRAINT [PK_BusLine1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'纬度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusLine', @level2type=N'COLUMN',@level2name=N'EndLat'
GO
/****** Object:  Table [dbo].[Bus]    Script Date: 06/19/2014 18:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DriverID] [int] NOT NULL,
	[BusNo] [nvarchar](50) NOT NULL,
	[MotoType] [nvarchar](50) NOT NULL,
	[SeatCnt] [int] NOT NULL,
	[Corp] [nvarchar](200) NOT NULL,
	[InsuEndDate] [datetime] NOT NULL,
	[BuyDate] [datetime] NOT NULL,
	[OwnerName] [nvarchar](50) NOT NULL,
	[OwnerPhone] [nvarchar](50) NOT NULL,
	[Etc1] [nvarchar](500) NOT NULL,
	[Etc2] [nvarchar](500) NOT NULL,
	[Etc3] [nvarchar](500) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[DelFlag] [nchar](1) NULL CONSTRAINT [DF_Bus_DelFlag]  DEFAULT ('N'),
 CONSTRAINT [PK_Bus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 06/19/2014 18:10:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[AddName] [nvarchar](100) NOT NULL,
	[SortID] [int] NULL,
	[AddLevel] [tinyint] NOT NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
