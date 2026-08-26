USE [DigitalBankingDb];
GO

IF OBJECT_ID(N'dbo.Accounts', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Accounts]
    (
        [AccountId] INT IDENTITY(1, 1) NOT NULL,
        [AccountNumber] NVARCHAR(34) NOT NULL,
        [CustomerId] INT NOT NULL,
        [AccountType] NVARCHAR(20) NOT NULL,
        [Balance] DECIMAL(18, 2) NOT NULL
            CONSTRAINT [DF_Accounts_Balance] DEFAULT (0),
        [Status] NVARCHAR(20) NOT NULL
            CONSTRAINT [DF_Accounts_Status] DEFAULT (N'Open'),

        CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([AccountId]),
        CONSTRAINT [UQ_Accounts_AccountNumber] UNIQUE ([AccountNumber]),
        CONSTRAINT [CK_Accounts_AccountType] CHECK ([AccountType] IN (N'Savings', N'Current')),
        CONSTRAINT [CK_Accounts_Balance] CHECK ([Balance] >= 0),
        CONSTRAINT [CK_Accounts_Status] CHECK ([Status] IN (N'Open', N'Closed', N'Blocked')),
        CONSTRAINT [FK_Accounts_Customers] FOREIGN KEY ([CustomerId])
            REFERENCES [dbo].[Customers] ([Id])
    );
END;
GO