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

USE [Purchase_SaleLedger]
GO


print 'Update GenerateCogsReport View'
/****** Object:  View [dbo].[vw_GenerateCogsReport]    Script Date: 06/11/2016 16:37:14 ******/
DROP VIEW [dbo].[vw_GenerateCogsReport]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

exec sp_ExecuteSql N'CREATE VIEW [dbo].[vw_GenerateCogsReport] AS
    Select 
	Id as CogsId,
	CogsLedgerId,
	CogsCompanyName, 
	CogsSiteName, 
	CogsStatus, 
	AdminStatuses.AdminStatusName as CogsStatusName,
	CogsGPCode, 
	Case when CogsDueDate < ''2015/01/01'' then null else CogsDueDate end as CogsDueDate,
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
LEFT JOIN AdminStatuses on CostOfGoodsSold.CogsStatus = AdminStatuses.AdminStatusId
LEFT JOIN MaintenanceTypes on CostOfGoodsSold.CogsGPCategory = MaintenanceTypes.ReportingDescription'


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

print 'drop getCogs function'
IF object_id(N'GetCogs', N'FN') IS NOT NULL
DROP FUNCTION GetCogs
GO


print 'drop getGrossCogs function'
/****** Object:  UserDefinedFunction [dbo].[GetNetandGrossCogs]    Script Date: 07/11/2016 11:57:07 ******/
IF object_id(N'GetGrossCogs', N'FN') IS NOT NULL
DROP FUNCTION [dbo].[GetGrossCogs]
GO


print 'Generate Cogs Stored Procedure which gets net and gross values'
USE [Purchase_SaleLedger]
GO

/****** Object:  StoredProcedure [dbo].[proc_GetCogsValue]    Script Date: 06/11/2016 17:17:18 ******/

/****** Object:  StoredProcedure [dbo].[proc_GetCogsValue]    Script Date: 06/11/2016 17:17:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		A Chapman
-- Create date: 06/11/2016
-- =============================================

USE [Purchase_SaleLedger]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'GetNetandGrossCogs') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetNetandGrossCogs]
GO

/****** Object:  UserDefinedFunction [dbo].[GetNetandGrossCogs]    Script Date: 07/11/2016 11:57:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

sp_executesql N'
CREATE FUNCTION [dbo].[GetNetandGrossCogs]
(
	@CogsFilter int
)
RETURNS TABLE
RETURN
Select 
	Sum(CogsCogsValue) as NetValue, 
	Sum(CogsGrossSalesAtList) as GrossValue
	 from 
	vw_GenerateCogsReport
INNER JOIN SaleLedgerExtended on 
	vw_GenerateCogsReport.CogsLedgerId = SaleLedgerExtended.LedgerId
where 
	JonasGroup != 4 AND
	--CogsStatus in () AND 
	--CogsGPCategoryType in (@GPCategory) AND
 	(
		(@CogsFilter = 0) OR 
	--installed this this month
		(@CogsFilter = 1 and CogsDueDate between DATEADD(month, DATEDIFF(month, 0, getdate()), 0) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0)) AND CogsStatus in (4,5)) OR
	--installed this month excluding this week
		(@CogsFilter = 2 and CogsDueDate between dateadd(wk, datediff(wk, 0, getdate()) + 1, -1) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0)) AND CogsStatus in (4,5)) OR
	--installed this week
		(@CogsFilter = 3 and CogsDueDate between dateadd(wk, datediff(wk, 0, getdate()), -1) AND dateadd(wk, datediff(wk, 0, getdate()) + 1, -1) AND CogsStatus in (4,5)) OR
	--Forecast for month
	    (@CogsFilter = 4 and CogsDueDate between DATEADD(month, DATEDIFF(month, 0, getdate()), 0) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0))) OR
	--Future Sales
		(@CogsFilter = 5 and CogsDueDate >= DATEADD(mm,DATEDIFF(m,0,GETDATE())+1,0)) OR
	--No Forecast
		(@CogsFilter = 6 and (CogsDueDate is null or CogsDueDate < ''2015/01/01'')) OR
	--Stuck Orders
		(@CogsFilter = 7 and SaleLedgerExtended.Spare2 = ''7'') OR
	--Cancelled Orders
		(@CogsFilter = 8 and CogsStatus = 5) OR
	--Hardware Sales
		(@CogsFilter = 9 and JonasGroup = 2) OR
	--Software Sales
		(@CogsFilter = 10 and JonasGroup = 1) OR
	--Professional Services
		(@CogsFilter = 11 and JonasGroup = 3)
		)
	'
GO
print 'Generate Cogs Stored Procedure which gets net and gross values'

GO

USE Purchase_SaleLedger
--This should always be at the bottom
Print ''
Declare @Version varchar(3) = (Select top 1 ConfigSetting from Configuration where Label = 'DbVersion')
Print 'Updating Database Version from ' + @Version + ' to 2.5' 
Update Configuration set ConfigSetting = '2.5' where Label = 'DbVersion'
