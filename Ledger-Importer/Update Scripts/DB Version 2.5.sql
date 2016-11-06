USE Purchase_SaleLedger
IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'CogsLedgerId'
      AND Object_ID = Object_ID(N'CostOfGoodsSold'))
BEGIN
    ALTER TABLE CostOfGoodsSold ADD CogsLedgerId BIGINT NULL
END
GO

USE [Purchase_SaleLedger]
GO

/****** Object:  View [dbo].[vw_GenerateCogsReport]    Script Date: 06/11/2016 16:10:25 ******/
DROP VIEW [dbo].[vw_GenerateCogsReport]
GO

/****** Object:  View [dbo].[vw_GenerateCogsReport]    Script Date: 06/11/2016 16:10:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_GenerateCogsReport] AS
    Select 
	Id as CogsId,
	CogsLedgerId,
	CogsCompanyName, 
	CogsSiteName, 
	CogsStatus, 
	Statuses.StatusName as CogsStatusName,
	CogsGPCode, 
	Case when CogsDueDate < '2015/01/01' then null else CogsDueDate end as CogsDueDate,
	MaintenanceTypes.MaintenanceType as CogsGPCategoryType,
	CogsGPCategory, 
	CogsDescription, 
	CogsSalesOrderId,
	CogsItemQuantity,
	CogsItemListPrice,
	CogsItemBuyPrice,
	CogsItemQuantity * CogsItemBuyPrice as CogsCogsValue,
	CogsItemQuantity * CogsItemListPrice as CogsGrossSalesAtList,
	(CogsItemQuantity * CogsItemListPrice) - (CogsItemQuantity * CogsItemBuyPrice) as CogsProfitValue
From
	CostOfGoodsSold
LEFT JOIN Statuses on CostOfGoodsSold.CogsStatus = Statuses.StatusId
LEFT JOIN MaintenanceTypes on CostOfGoodsSold.CogsGPCategory = MaintenanceTypes.ReportingDescription

GO


USE Purchase_SaleLedger
IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'SaleLedgerLedgerId'
      AND Object_ID = Object_ID(N'SaleLedger'))
BEGIN
    ALTER TABLE SaleLedger ADD SaleLedgerLedgerId BIGINT NULL
END
GO


USE [Purchase_SaleLedger]
GO

/****** Object:  View [dbo].[SaleLedgerExtended]    Script Date: 05/11/2016 17:47:07 ******/
DROP VIEW [dbo].[SaleLedgerExtended]
GO

/****** Object:  View [dbo].[SaleLedgerExtended]    Script Date: 05/11/2016 17:47:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


 CREATE View [dbo].[SaleLedgerExtended] as 
 

Select 
	UniqueID,
	CustRef,
	CustName,
	SiteName,
	GL,
	GLDescription,
	Date,
	DueDate,
	DATEPART(yyyy,Date) as Year,
	DATEPART(mm,Date) as Month,
	DATEPART(dd,Date) as Day,
	InvoiceNo,
	ItemDescription,
	MaintenanceTypes.JonasGroup,
	JonasGroups.GroupName as JonasGroupName,
	MaintenanceGLBridge.MaintenanceType,
	MaintenanceTypes.MaintTypeDescription,
	MaintenanceTypes.ReportingDescription,
	Type as EntryType,
	Qty as QtyValue,
	Net as NetValue,
	Tax as TaxValue,
	Gross as GrossValue,
	Profit as ProfitValue,
	CustOrderNo,
	ImportType,
	Category,
	RAND(CAST(NEWID() AS varbinary)) as UniqueID2,
	MiniPack,
	SiteSurveyDate,
	BacklogComments,
	Deposit,
	AssignedTo,
	MegJobNo,
	DirectDebit,
	Spare1,
	isNull(TerminalTypes.TerminalTypeName,'Unknown') as TerminalTypeName,
	Spare2,
	isNull(AdminStatuses.AdminStatusName,'Unknown') as AdminStatusName,
	SaleLedgerLedgerId as LedgerId
from 
	SaleLedger
Left Join GLTypes on SaleLedger.GL = GLTypes.GLNo
Left Join MaintenanceGLBridge on SaleLedger.GL = MaintenanceGLBridge.GLNumber
Left Join MaintenanceTypes on MaintenanceGLBridge.MaintenanceType = MaintenanceTypes.MaintenanceType or MaintenanceTypes.MaintTypeDescription = SaleLedger.Category
Left Join JonasGroups on MaintenanceTypes.JonasGroup = JonasGroups.GroupNo
Left Join TerminalTypes on SaleLedger.Spare1 = Convert(varchar(2),TerminalTypes.TerminalTypeId)
Left Join AdminStatuses on SaleLedger.Spare2 = Convert(varchar(2),AdminStatuses.AdminStatusId)
GO

USE Purchase_SaleLedger
--This should always be at the bottom
Print ''
Declare @Version varchar(3) = (Select top 1 ConfigSetting from Configuration where Label = 'DbVersion')
Print 'Updating Database Version from ' + @Version + ' to 2.5' 
Update Configuration set ConfigSetting = '2.5' where Label = 'DbVersion'
