Use Purchase_SaleLedger


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'Configuration' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Configuring primary key for Configuration table'
ALTER TABLE Configuration
ADD CONSTRAINT pk_Configuration_Label PRIMARY KEY (Label)
END
GO

Print 'Setting EntryTypeNo to NOT NULL'
ALTER TABLE [EntryTypes] ALTER COLUMN EntryTypeNo smallint NOT NULL

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'EntryTypes' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Configuring primary key for EntryTypes table'
ALTER TABLE EntryTypes
ADD CONSTRAINT pk_EntryTypes_EntryTypeNo PRIMARY KEY (EntryTypeNo)
END
GO

Print 'Setting GLTypes.GLNo to NOT NULL'
ALTER TABLE [GLTypes] ALTER COLUMN GLNo int NOT NULL
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'GLTypes' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Configuring primary key for GLTypes table'
ALTER TABLE GLTypes
ADD CONSTRAINT PK_GLTypes_GLNo PRIMARY KEY (GLNo)
END
GO


Print 'Create Identity PK for GP_Temp_InvoiceLedger'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'GP_Temp_InvoiceLedger'))
BEGIN
    ALTER TABLE dbo.GP_Temp_InvoiceLedger ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'GP_Temp_InvoiceLedger' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set GP_Temp_InvoiceLedger.Id to primary key'
ALTER TABLE GP_Temp_InvoiceLedger
ADD CONSTRAINT PK_GP_Temp_InvoiceLedger_Id PRIMARY KEY ([Id])
END
GO



Print 'Create Identity PK for GP_Temp_OutstandingInvoices'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'GP_Temp_OutstandingInvoices'))
BEGIN
    ALTER TABLE dbo.GP_Temp_OutstandingInvoices ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'GP_Temp_OutstandingInvoices' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set GP_Temp_OutstandingInvoices.Id to primary key'
ALTER TABLE GP_Temp_OutstandingInvoices
ADD CONSTRAINT PK_GP_Temp_OutstandingInvoices_Id PRIMARY KEY ([Id])
END
GO



Print 'Create Identity PK for GP_Temp_PostedInvoices'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'GP_Temp_PostedInvoices'))
BEGIN
    ALTER TABLE dbo.GP_Temp_PostedInvoices ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'GP_Temp_PostedInvoices' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set GP_Temp_PostedInvoices.Id to primary key'
ALTER TABLE GP_Temp_PostedInvoices
ADD CONSTRAINT PK_GP_Temp_PostedInvoices_Id PRIMARY KEY ([Id])
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'JonasGroups' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set JonasGroups.GroupNo to primary key'
ALTER TABLE JonasGroups
ADD CONSTRAINT PK_JonasGroups_GroupNo PRIMARY KEY ([GroupNo])
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'JonasTypes' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set JonasTypes.JonasType to primary key'
ALTER TABLE JonasTypes
ADD CONSTRAINT PK_JonasTypes_GroupNo PRIMARY KEY ([JonasType])
END
GO


Print 'Create Identity PK for Log'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'Log'))
BEGIN
    ALTER TABLE dbo.Log ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'Log' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set Log.Id to primary key'
ALTER TABLE [Log]
ADD CONSTRAINT Log_Id PRIMARY KEY ([Id])
END
GO

Print 'Create Identity PK for MaintenanceGLBridge'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'MaintenanceGLBridge'))
BEGIN
    ALTER TABLE dbo.MaintenanceGLBridge ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'MaintenanceGLBridge' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set MaintenanceGLBridge.Id to primary key'
ALTER TABLE MaintenanceGLBridge
ADD CONSTRAINT MaintenanceGLBridge_Id PRIMARY KEY ([Id])
END
GO




Print 'Create Identity PK for OutstandingInvoices'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'OutstandingInvoices'))
BEGIN
    ALTER TABLE dbo.OutstandingInvoices ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'OutstandingInvoices' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set OutstandingInvoices.Id to primary key'
ALTER TABLE OutstandingInvoices
ADD CONSTRAINT PK_OutstandingInvoices_Id PRIMARY KEY ([Id])
END
GO



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'PostedInvoices' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set PostedInvoices.Id to primary key'
ALTER TABLE PostedInvoices
ADD CONSTRAINT PK_PostedInvoices_Id PRIMARY KEY ([ID])
END
GO



Print 'Create Identity PK for Sage_Temp_InvoiceLedger'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'Sage_Temp_InvoiceLedger'))
BEGIN
    ALTER TABLE dbo.Sage_Temp_InvoiceLedger ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'Sage_Temp_InvoiceLedger' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set Sage_Temp_InvoiceLedger.Id to primary key'
ALTER TABLE Sage_Temp_InvoiceLedger
ADD CONSTRAINT PK_Sage_Temp_InvoiceLedger_Id PRIMARY KEY ([Id])
END
GO



Print 'Create Identity PK for SaleLedger'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'SaleLedger'))
BEGIN
    ALTER TABLE dbo.SaleLedger ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'SaleLedger' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set SaleLedger.Id to primary key'
ALTER TABLE SaleLedger
ADD CONSTRAINT PK_SaleLedger_Id PRIMARY KEY ([Id])
END
GO



Print 'Create Identity PK for Temp_OrderLedger'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'Temp_OrderLedger'))
BEGIN
    ALTER TABLE dbo.Temp_OrderLedger ADD Id bigint IDENTITY(1,1)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'Temp_OrderLedger' AND TABLE_SCHEMA = 'dbo')
BEGIN
Print 'Set Temp_OrderLedger.Id to primary key'
ALTER TABLE Temp_OrderLedger
ADD CONSTRAINT PK_Temp_OrderLedger_Id PRIMARY KEY ([Id])
END
GO


