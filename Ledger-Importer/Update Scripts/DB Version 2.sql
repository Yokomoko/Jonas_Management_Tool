Print 'Upgrading from DBv1 to DBv2'

Use Purchase_SaleLedger
Print 'Setting SaleLedger.GL to nullable'
Alter Table SaleLedger Alter Column GL int null

Print 'Setting Currency to 3 characters'
Alter Table SaleLedger Alter Column Currency nvarchar(3)
Alter Table Temp_OrderLedger Alter Column Currency nvarchar(3)

Print 'Rename Sage_Temp_OrderLedger to Temp_OrderLedger'

IF EXISTS (Select * from sys.objects where object_id = object_id(N'Sage_Temp_OrderLedger'))
Begin
Exec sp_rename 'Sage_Temp_OrderLedger', 'Temp_OrderLedger';
End

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Sage_Grid_ImportOrders') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].[Sage_Grid_ImportOrders]
GO
CREATE PROCEDURE [dbo].[Sage_Grid_ImportOrders]
		@tblLedger CustomTypeSaleOrderImport READONLY

AS BEGIN

SET NOCOUNT ON;

Declare @TempResult int
Declare @FinalResult int


Delete from Temp_OrderLedger --SaleLedgerOrdersTemp

Insert into Temp_OrderLedger (Date,CustName, CustRef, DueDate, GL, UniqueID, ItemDescription, Qty, Net, Tax, Gross, Profit, DeliveryAddress, Currency, CustOrderNo) 
Select Date,CustName, CustRef, Convert(Date,DueDate), GL, UniqueID, ItemDescription, Qty, Net, Tax, Gross, Profit, DeliveryAddress, Currency, CustOrderNo from @tblLedger where GL != 1 or GL !=0001
End
GO

IF EXISTS (Select * from sys.objects where object_id = OBJECT_ID(N'CRM_Temp_ImportOrders') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].CRM_Temp_ImportOrders
GO
CREATE PROCEDURE [dbo].[CRM_Temp_ImportOrders]
AS BEGIN
Delete From Purchase_SaleLedger.dbo.SaleLedger where [Type] = 'CRM Sales Order'
--Import valid data from the temporary table into the main table
Insert into Purchase_SaleLedger.dbo.SaleLedger (Date, CustRef, CustName, DueDate, Category, ItemDescription, Qty, Net, Tax, Gross, Profit, Type, SiteName, Currency, CustOrderNo,ImportType)
	Select Date,CustRef, CustName, Nullif(DueDate,''), Category, ItemDescription, Qty, Net, Tax, Gross, Profit, 'CRM Sales Order', SiteName, Currency, CustOrderNo,'CRM' from Temp_OrderLedger
	WHERE NOT EXISTS (Select UniqueID from SaleLedger) and Date is not null
	and Date > '2015/01/01'

Delete from Temp_OrderLedger
END

GO

Print 'Adding SaleLedger.Category and SaleLedger.SiteName for new CRM Report'
Use Purchase_SaleLedger
IF NOT EXISTS (Select * from Sys.columns where Name = 'Category' and object_id = object_id(N'SaleLedger'))
Begin
Alter Table SaleLedger Add Category nvarchar(255)
End
Use Purchase_SaleLedger
IF NOT EXISTS (Select * from Sys.columns where Name = 'Category' and object_id = object_id(N'Temp_OrderLedger'))
Begin
Alter Table Temp_OrderLedger Add Category nvarchar(255) null
End

IF NOT EXISTS (Select * from Sys.columns where Name = 'SiteName' and object_id = object_id(N'SaleLedger'))
Begin
Alter Table SaleLedger Add SiteName nvarchar(255)
End
IF NOT EXISTS (Select * from Sys.columns where Name = 'SiteName' and object_id = object_id(N'Temp_OrderLedger'))
Begin
Alter Table Temp_OrderLedger Add SiteName nvarchar(255)
End
GO

Print 'Create New Table Type for CRM'
IF NOT EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'CustomTypeSaleOrderImport_CRM')
CREATE TYPE [dbo].[CustomTypeSaleOrderImport_CRM] AS TABLE(
	[Date] [date] NULL,
	[CustName] [varchar](100) NULL,
	[CustRef] [varchar](30) NULL,
	[DueDate] [date] NULL,
	[Category] [nvarchar](255) NULL,
	[ItemDescription] [nvarchar](max) NULL,
	[Qty] [numeric](16, 6) NULL,
	[Net] [money] NULL,
	[Tax] [money] NULL,
	[Gross] [money] NULL,
	[Profit] [money] NULL,
	[SiteName] [nvarchar](255) NULL,
	[Currency] [nvarchar](3) NULL,
	[CustOrderNo] [nvarchar](20) NULL
)
GO



Print 'Creating CRM Sales Order Sproc to Import from Grid into Temporary Table'

IF EXISTS (Select * from sys.objects where object_id = OBJECT_ID(N'CRM_Grid_ImportOrders') AND type in (N'P', N'PC'))
DROP PROCEDURE CRM_Grid_ImportOrders
GO

CREATE PROCEDURE [dbo].[CRM_Grid_ImportOrders]
		@tblLedger CustomTypeSaleOrderImport_CRM READONLY

AS BEGIN

SET NOCOUNT ON;

Declare @TempResult int
Declare @FinalResult int


Delete from Temp_OrderLedger --SaleLedgerOrdersTemp


Insert into Temp_OrderLedger (Date, CustName, CustRef, DueDate, Category, ItemDescription, Qty, Net, Tax, Gross, Profit, SiteName, Currency, CustOrderNo)
Select Convert([Date],[Date]), CustName, CustRef, Convert(Date,DueDate), Category, ItemDescription, Qty, Net, Tax, Gross, Profit, SiteName, Currency, CustOrderNo from @tblLedger


End
GO


Print 'Creating CRM Sales Order Sproc to Import from Temporary Table to SaleLedger Table'
IF EXISTS (Select * from sys.objects where object_id = object_id(N'CRM_Temp_ImportOrders') AND type in (N'P', N'PC'))
DROP PROCEDURE CRM_Temp_ImportOrders
GO

CREATE PROCEDURE [dbo].[CRM_Temp_ImportOrders]
AS BEGIN

Delete from Purchase_SaleLedger.dbo.SaleLedger where [Type] = 'CRM Sales Order'

--Import valid data from the temporary table into the main table
Insert into Purchase_SaleLedger.dbo.SaleLedger (Date, CustRef, CustName, DueDate, Category,  ItemDescription, Qty, Net, Tax, Gross, Profit, [Type], SiteName, Currency,  CustOrderNo,ImportType)
	Select Date,CustRef, CustName, Nullif(DueDate,''), Category,  ItemDescription, Qty, Net, Tax, Gross, Profit, 'CRM Sales Order', SiteName, Currency,  CustOrderNo,'CRM' from Temp_OrderLedger
	where Date is not null
	and Date > '2015/01/01'

--Delete from Temp_OrderLedger
END
GO


Print 'Update SaleLedgerExtended View to include Category and SiteName'
IF EXISTS(SELECT * FROM sys.views WHERE name = 'SaleLedgerExtended' AND schema_id = SCHEMA_ID('dbo'))
DROP VIEW dbo.SaleLedgerExtended
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
	RAND(CAST(NEWID() AS varbinary)) as UniqueID2
from 
	SaleLedger
Left Join GLTypes on SaleLedger.GL = GLTypes.GLNo
Left Join MaintenanceGLBridge on SaleLedger.GL = MaintenanceGLBridge.GLNumber
Left Join MaintenanceTypes on MaintenanceGLBridge.MaintenanceType = MaintenanceTypes.MaintenanceType or MaintenanceTypes.MaintTypeDescription = SaleLedger.Category

Left Join JonasGroups on MaintenanceTypes.JonasGroup = JonasGroups.GroupNo
GO





--Always Last
Print ''
Print 'Updating Database version in Configuration'
Use Purchase_SaleLedger
Update Configuration set ConfigSetting = 1.5 where Label = 'DbVersion'