/****** Object:  Table [dbo].[tbTransaction]    Script Date: 5/29/2022 1:18:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTransaction](
	[ID] [uniqueidentifier] NOT NULL,
	[TransactionIdentificator] [varchar](50) NULL,
	[Amount] [decimal](18, 2) NULL,
	[CurrencyCode] [varchar](50) NULL,
	[TransactionDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[AccessTime] [datetime] NULL,
	[FileExtension] [varchar](50) NULL,
 CONSTRAINT [PK_tbTransaction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tbTransaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [Status], [AccessTime], [FileExtension]) VALUES (N'd63bf18a-ef5f-4601-8ba5-1732a28563e8', N'Inv00003', CAST(20000.00 AS Decimal(18, 2)), N'EUR', CAST(N'2022-05-24T04:09:15.000' AS DateTime), N'R', CAST(N'2022-05-29T13:16:16.087' AS DateTime), N'.xml')
INSERT [dbo].[tbTransaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [Status], [AccessTime], [FileExtension]) VALUES (N'06c352f4-c259-4193-a42d-3130f7039742', N'Invoice00002011', CAST(1000.00 AS Decimal(18, 2)), N'USD', CAST(N'2022-05-29T00:33:16.000' AS DateTime), N'R', CAST(N'2022-05-29T13:12:50.913' AS DateTime), N'.csv')
INSERT [dbo].[tbTransaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [Status], [AccessTime], [FileExtension]) VALUES (N'980493ad-aa07-4d22-8761-7c6542a82ee3', N'Inv00001', CAST(50000.00 AS Decimal(18, 2)), N'EUR', CAST(N'2022-05-24T04:09:15.000' AS DateTime), N'R', CAST(N'2022-05-29T13:16:16.083' AS DateTime), N'.xml')
INSERT [dbo].[tbTransaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [Status], [AccessTime], [FileExtension]) VALUES (N'30dbb4bf-b9a6-498e-b416-80a4709c85a3', N'Invoice00001015', CAST(1000.00 AS Decimal(18, 2)), N'USD', CAST(N'2022-05-30T00:33:16.000' AS DateTime), N'A', CAST(N'2022-05-29T13:12:50.800' AS DateTime), N'.csv')
INSERT [dbo].[tbTransaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [Status], [AccessTime], [FileExtension]) VALUES (N'8d2b739f-ca87-44b6-8131-db1537093a13', N'Inv00002', CAST(10000.00 AS Decimal(18, 2)), N'EUR', CAST(N'2022-05-28T04:09:15.000' AS DateTime), N'R', CAST(N'2022-05-29T13:16:16.090' AS DateTime), N'.xml')
GO
