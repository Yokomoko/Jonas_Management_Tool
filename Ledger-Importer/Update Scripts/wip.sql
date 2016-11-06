Declare @Statuses int = null
Declare @GPCategory int = null
Declare @DateFilter int = 0

--Total Backlog

Select 
	1 as ID,
	'Total Backlog' as Description,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) as GrossValue,
	Sum(vw_GenerateCogsReport.CogsCogsValue) as CogsValue,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) - Sum(vw_GenerateCogsReport.CogsCogsValue) as NetValue
from 
	vw_GenerateCogsReport
INNER JOIN SaleLedgerExtended on 
	SaleLedgerExtended.QtyValue = vw_GenerateCogsReport.CogsItemQuantity AND
	SaleLedgerExtended.ItemDescription = vw_GenerateCogsReport.CogsDescription AND
	SaleLedgerExtended.CustOrderNo = convert(nvarchar(20), vw_GenerateCogsReport.CogsSalesOrderId) AND 
	SaleLedgerExtended.Spare2 = vw_GenerateCogsReport.CogsStatus AND
	SaleLedgerExtended.ImportType = 'OpenCRM Sales Order'
where 
	(@Statuses is null) or (@Statuses is not null and CogsStatus in (@Statuses)) AND 
	(@GPCategory is null) or (@GPCategory is not null and CogsGPCategoryType in (@GPCategory)) AND
	--total backlog
		(@DateFilter = 0)

Union

Select 
	2,
	'This Month' as Description,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) as GrossValue,
	Sum(vw_GenerateCogsReport.CogsCogsValue) as CogsValue,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) - Sum(vw_GenerateCogsReport.CogsCogsValue) as NetValue
from 
	vw_GenerateCogsReport
INNER JOIN SaleLedgerExtended on 
	SaleLedgerExtended.QtyValue = vw_GenerateCogsReport.CogsItemQuantity AND
	SaleLedgerExtended.ItemDescription = vw_GenerateCogsReport.CogsDescription AND
	SaleLedgerExtended.CustOrderNo = convert(nvarchar(20), vw_GenerateCogsReport.CogsSalesOrderId) AND 
	SaleLedgerExtended.Spare2 = vw_GenerateCogsReport.CogsStatus AND
	SaleLedgerExtended.ImportType = 'OpenCRM Sales Order'
where 
	(@Statuses is null) or (@Statuses is not null and CogsStatus in (@Statuses)) AND 
	(@GPCategory is null) or (@GPCategory is not null and CogsGPCategoryType in (@GPCategory)) AND
		(CogsDueDate between DATEADD(month, DATEDIFF(month, 0, getdate()), 0) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0)))

Union
Select 
	3,
	'This Month Excluding This Week' as Description,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) as GrossValue,
	Sum(vw_GenerateCogsReport.CogsCogsValue) as CogsValue,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) - Sum(vw_GenerateCogsReport.CogsCogsValue) as NetValue
from 
	vw_GenerateCogsReport
INNER JOIN SaleLedgerExtended on 
	SaleLedgerExtended.QtyValue = vw_GenerateCogsReport.CogsItemQuantity AND
	SaleLedgerExtended.ItemDescription = vw_GenerateCogsReport.CogsDescription AND
	SaleLedgerExtended.CustOrderNo = convert(nvarchar(20), vw_GenerateCogsReport.CogsSalesOrderId) AND 
	SaleLedgerExtended.Spare2 = vw_GenerateCogsReport.CogsStatus AND
	SaleLedgerExtended.ImportType = 'OpenCRM Sales Order'
where 
	(@Statuses is null) or (@Statuses is not null and CogsStatus in (@Statuses)) AND 
	(@GPCategory is null) or (@GPCategory is not null and CogsGPCategoryType in (@GPCategory)) AND
	--this month excluding this week
		(CogsDueDate between dateadd(wk, datediff(wk, 0, getdate()) + 1, -1) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0)))
Union

Select 
	4,
	'This Week' as Description,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) as GrossValue,
	Sum(vw_GenerateCogsReport.CogsCogsValue) as CogsValue,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) - Sum(vw_GenerateCogsReport.CogsCogsValue) as NetValue
from 
	vw_GenerateCogsReport
INNER JOIN SaleLedgerExtended on 
	SaleLedgerExtended.QtyValue = vw_GenerateCogsReport.CogsItemQuantity AND
	SaleLedgerExtended.ItemDescription = vw_GenerateCogsReport.CogsDescription AND
	SaleLedgerExtended.CustOrderNo = convert(nvarchar(20), vw_GenerateCogsReport.CogsSalesOrderId) AND 
	SaleLedgerExtended.Spare2 = vw_GenerateCogsReport.CogsStatus AND
	SaleLedgerExtended.ImportType = 'OpenCRM Sales Order'
where 
	(@Statuses is null) or (@Statuses is not null and CogsStatus in (@Statuses)) AND 
	(@GPCategory is null) or (@GPCategory is not null and CogsGPCategoryType in (@GPCategory)) AND
	--this week
		CogsDueDate between dateadd(wk, datediff(wk, 0, getdate()), -1) AND dateadd(wk, datediff(wk, 0, getdate()) + 1, -1)

Union

Select 
	5,
	'Forecast for Month' as Description,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) as GrossValue,
	Sum(vw_GenerateCogsReport.CogsCogsValue) as CogsValue,
	Sum(SaleLedgerExtended.NetValue * SaleLedgerExtended.QtyValue) - Sum(vw_GenerateCogsReport.CogsCogsValue) as NetValue
from 
	vw_GenerateCogsReport
INNER JOIN SaleLedgerExtended on 
	SaleLedgerExtended.QtyValue = vw_GenerateCogsReport.CogsItemQuantity AND
	SaleLedgerExtended.ItemDescription = vw_GenerateCogsReport.CogsDescription AND
	SaleLedgerExtended.CustOrderNo = convert(nvarchar(20), vw_GenerateCogsReport.CogsSalesOrderId) AND 
	SaleLedgerExtended.Spare2 = vw_GenerateCogsReport.CogsStatus AND
	SaleLedgerExtended.ImportType = 'OpenCRM Sales Order'
where 
	(@Statuses is null) or (@Statuses is not null and CogsStatus in (@Statuses)) AND 
	(@GPCategory is null) or (@GPCategory is not null and CogsGPCategoryType in (@GPCategory)) --AND
	--Forecast for month
	--i dont know