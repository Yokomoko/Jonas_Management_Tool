Print 'Fix Existing Import'
USE [Purchase_SaleLedger]
GO
/****** Object:  StoredProcedure [dbo].[CRM_Grid_ImportOrders_Adv]    Script Date: 23/09/2016 09:42:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CRM_Grid_ImportOrders_Adv]
	@Date DateTime NULL,
	@CustName varchar(100) NULL,
	@SiteName nvarchar(255) NULL,
	@CustRef varchar(30) NULL,
	@DueDate date NULL,
	@Category nvarchar(255) NULL,
	@ItemDescription nvarchar(max) NULL,
	@Qty numeric(16, 6) NULL,
	@Net money NULL,
	@Tax money NULL,
	@Gross money NULL,
	@Profit money NULL,
	@Currency nvarchar(3) NULL,
	@CustOrderNo nvarchar(20) NULL,
	@MiniPack smallint NULL,
	@SiteSurveyDate varchar(255) NULL,
	@BacklogComments varchar(max) NULL,
	@Deposit varchar(255) NULL,
	@AssignedTo varchar(255) NULL,
	@MegJobNo varchar(255) NULL,
	@DirectDebit smallint NULL

AS BEGIN
SET NOCOUNT ON;

Declare @TempResult int
Declare @FinalResult int

Insert into Temp_OrderLedger (Date, CustName, SiteName, CustRef, DueDate, Category, ItemDescription, Qty, Net, Tax, Gross, Profit, Currency, CustOrderNo, MiniPack, SiteSurveyDate, BacklogComments, Deposit, AssignedTo, MegJobNo, DirectDebit)
Values (@Date, @CustName, @SiteName, @CustRef, @DueDate, @Category, @ItemDescription, @Qty, @Net, @Tax, @Gross, @Profit, @Currency, @CustOrderNo, @MiniPack, @SiteSurveyDate, @BacklogComments, @Deposit, @AssignedTo, @MegJobNo, @DirectDebit)
End

GO

USE Purchase_SaleLedger

Print 'Fix Advanced Import Procedure 2 for OpenCRM Import'
IF EXISTS ( SELECT * FROM   sysobjects WHERE  id = object_id(N'[dbo].[CRM_Grid_ImportOrders_Adv2]')  and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].[CRM_Grid_ImportOrders_Adv2]
END
GO
CREATE PROCEDURE [dbo].[CRM_Grid_ImportOrders_Adv2]
	@Date DateTime NULL,
	@CustName varchar(100) NULL,
	@SiteName nvarchar(255) NULL,
	@CustRef varchar(30) NULL,
	@DueDate date NULL,
	@Category nvarchar(255) NULL,
	@ItemDescription nvarchar(max) NULL,
	@Qty numeric(16, 6) NULL,
	@Net money NULL,
	@Tax money NULL,
	@Gross money NULL,
	@Profit money NULL,
	@Currency nvarchar(3) NULL,
	@CustOrderNo nvarchar(20) NULL,
	@MiniPack smallint NULL,
	@SiteSurveyDate varchar(255) NULL,
	@BacklogComments varchar(max) NULL,
	@Deposit varchar(255) NULL,
	@AssignedTo varchar(255) NULL,
	@MegJobNo varchar(255) NULL,
	@DirectDebit smallint NULL,
	@Spare1 varchar(255) NULL
AS BEGIN

SET NOCOUNT ON;

Declare @TempResult int
Declare @FinalResult int



Insert into Temp_OrderLedger (Date, CustName, SiteName, CustRef, DueDate, Category, ItemDescription, Qty, Net, Tax, Gross, Profit, Currency, CustOrderNo, MiniPack, SiteSurveyDate, BacklogComments, Deposit, AssignedTo, MegJobNo, DirectDebit, Spare1)
VALUES (@Date, @CustName, @SiteName, @CustRef, @DueDate, @Category, @ItemDescription, @Qty, @Net, @Tax, @Gross, @Profit, @Currency, @CustOrderNo, @MiniPack, @SiteSurveyDate, @BacklogComments, @Deposit, @AssignedTo, @MegJobNo, @DirectDebit, @Spare1)


End

GO
USE Purchase_SaleLedger

Print 'Fix Advanced Import Procedure 3 for OpenCRM Import'
IF EXISTS ( SELECT * FROM   sysobjects WHERE  id = object_id(N'[dbo].[CRM_Grid_ImportOrders_Adv3]')  and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].[CRM_Grid_ImportOrders_Adv3]
END
GO
CREATE PROCEDURE [dbo].[CRM_Grid_ImportOrders_Adv3]
	@Date DateTime NULL,
	@CustName varchar(100) NULL,
	@SiteName nvarchar(255) NULL,
	@CustRef varchar(30) NULL,
	@DueDate date NULL,
	@Category nvarchar(255) NULL,
	@ItemDescription nvarchar(max) NULL,
	@Qty numeric(16, 6) NULL,
	@Net money NULL,
	@Tax money NULL,
	@Gross money NULL,
	@Profit money NULL,
	@Currency nvarchar(3) NULL,
	@CustOrderNo nvarchar(20) NULL,
	@MiniPack smallint NULL,
	@SiteSurveyDate varchar(255) NULL,
	@BacklogComments varchar(max) NULL,
	@Deposit varchar(255) NULL,
	@AssignedTo varchar(255) NULL,
	@MegJobNo varchar(255) NULL,
	@DirectDebit smallint NULL,
	@Spare1 varchar(255) NULL,
	@Spare2 varchar(255) NULL

AS BEGIN

SET NOCOUNT ON;

Declare @TempResult int
Declare @FinalResult int

Insert into Temp_OrderLedger (Date, CustName, SiteName, CustRef, DueDate, Category, ItemDescription, Qty, Net, Tax, Gross, Profit, Currency, CustOrderNo, MiniPack, SiteSurveyDate, BacklogComments, Deposit, AssignedTo, MegJobNo, DirectDebit, Spare1, Spare2)
VALUES (@Date, @CustName, @SiteName, @CustRef, @DueDate, @Category, @ItemDescription, @Qty, @Net, @Tax, @Gross, @Profit, @Currency, @CustOrderNo, @MiniPack, @SiteSurveyDate, @BacklogComments, @Deposit, @AssignedTo, @MegJobNo, @DirectDebit, @Spare1, @Spare2)


End

GO


USE Purchase_SaleLedger
--This should always be at the bottom
Print ''
Declare @Version varchar(3) = (Select top 1 ConfigSetting from Configuration where Label = 'DbVersion')
Print 'Updating Database Version from ' + @Version + ' to 2.2' 
Update Configuration set ConfigSetting = '2.2' where Label = 'DbVersion'
