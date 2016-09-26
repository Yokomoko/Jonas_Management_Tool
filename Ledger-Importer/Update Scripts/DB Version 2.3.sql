SET NOCOUNT ON

Print 'Adding Admin Statuses Table'
USE [Purchase_SaleLedger]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF OBJECT_ID(N'dbo.AdminStatuses',N'U') IS NULL
BEGIN

CREATE TABLE [dbo].[AdminStatuses](
	[AdminStatusId] [int] IDENTITY(0,1) NOT NULL,
	[AdminStatusName] [varchar](50) NULL,
 CONSTRAINT [PK_AdminStatuses] PRIMARY KEY CLUSTERED 
(
	[AdminStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

SET ANSI_PADDING OFF
GO


Print 'Adding TerminalTypes Table'



USE [Purchase_SaleLedger]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO


IF OBJECT_ID(N'dbo.TerminalTypes',N'U') IS NULL
BEGIN
CREATE TABLE [dbo].[TerminalTypes](
	[TerminalTypeId] [int] IDENTITY(0,1) NOT NULL,
	[TerminalTypeName] [varchar](50) NULL,
 CONSTRAINT [PK_TerminalTypes] PRIMARY KEY CLUSTERED 
(
	[TerminalTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

SET ANSI_PADDING OFF
GO

Print 'Fill AdminStatuses Table'
IF NOT EXISTS(Select * from AdminStatuses where AdminStatusId = 0)
BEGIN INSERT INTO AdminStatuses Select 'Created' END
IF NOT EXISTS(Select * from AdminStatuses where AdminStatusId = 1)
BEGIN INSERT INTO AdminStatuses Select 'Pending' END
IF NOT EXISTS(Select * from AdminStatuses where AdminStatusId = 2)
BEGIN INSERT INTO AdminStatuses Select 'Approved' END
IF NOT EXISTS(Select * from AdminStatuses where AdminStatusId = 3)
BEGIN INSERT INTO AdminStatuses Select 'Pending Cancellation'END
IF NOT EXISTS(Select * from AdminStatuses where AdminStatusId = 4)
BEGIN INSERT INTO AdminStatuses Select 'Pending Invoice' END
IF NOT EXISTS(Select * from AdminStatuses where AdminStatusId = 5)
BEGIN INSERT INTO AdminStatuses Select 'Invoiced' END
IF NOT EXISTS(Select * from AdminStatuses where AdminStatusId = 6)
BEGIN INSERT INTO AdminStatuses Select 'Pending Approval' END
IF NOT EXISTS(Select * from AdminStatuses where AdminStatusId = 7)
BEGIN INSERT INTO AdminStatuses Select 'Stuck' END

 Print 'Fill TerminalTypes Table'
 IF NOT EXISTS(Select * from TerminalTypes where TerminalTypeId = 0)
 BEGIN INSERT INTO TerminalTypes Select 'Quantum' END
 IF NOT EXISTS(Select * from TerminalTypes where TerminalTypeId = 1)
 BEGIN INSERT INTO TerminalTypes Select 'Pixel' END
 IF NOT EXISTS(Select * from TerminalTypes where TerminalTypeId = 2)
 BEGIN INSERT INTO TerminalTypes Select 'Absolute' END
 IF NOT EXISTS(Select * from TerminalTypes where TerminalTypeId = 3)
 BEGIN INSERT INTO TerminalTypes Select 'Fashion Master' END





USE Purchase_SaleLedger
--This should always be at the bottom
Print ''
Declare @Version varchar(3) = (Select top 1 ConfigSetting from Configuration where Label = 'DbVersion')
Print 'Updating Database Version from ' + @Version + ' to 2.3' 
Update Configuration set ConfigSetting = '2.3' where Label = 'DbVersion'
