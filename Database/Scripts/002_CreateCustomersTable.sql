USE [DigitalBankingDb];
GO

IF OBJECT_ID(N'dbo.Customers', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Customers]
    (
        [Id] INT IDENTITY(1, 1) NOT NULL,
        [CustomerId] NVARCHAR(100) NOT NULL,
        [Name] NVARCHAR(200) NOT NULL,
        [DateOfBirth] DATE NOT NULL,
        [PanId] NVARCHAR(50) NOT NULL,
        [MobileNumber] NVARCHAR(30) NOT NULL,
        [EmailAddress] NVARCHAR(320) NOT NULL,
        [Address] NVARCHAR(500) NOT NULL,
        [EmploymentDetails] NVARCHAR(500) NOT NULL,
        [IncomeDetails] DECIMAL(18, 2) NOT NULL,
        [Status] NVARCHAR(50) NOT NULL
            CONSTRAINT [DF_Customers_Status] DEFAULT (N'Submitted'),
        [CreatedDate] DATETIME2(7) NOT NULL
            CONSTRAINT [DF_Customers_CreatedDate] DEFAULT (SYSUTCDATETIME()),
        [ModifiedDate] DATETIME2(7) NULL,

        CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [UQ_Customers_CustomerId] UNIQUE ([CustomerId]),
        CONSTRAINT [CK_Customers_IncomeDetails] CHECK ([IncomeDetails] >= 0),
        CONSTRAINT [CK_Customers_Status] CHECK (LEN(LTRIM(RTRIM([Status]))) > 0)
    );
END;
GO
