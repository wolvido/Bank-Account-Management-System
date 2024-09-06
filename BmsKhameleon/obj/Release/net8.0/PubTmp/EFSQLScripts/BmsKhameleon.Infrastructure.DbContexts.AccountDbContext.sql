IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813093218_Initial'
)
BEGIN
    CREATE TABLE [Accounts] (
        [AccountId] uniqueidentifier NOT NULL,
        [AccountName] nvarchar(50) NOT NULL,
        [BankName] nvarchar(25) NOT NULL,
        [AccountNumber] nvarchar(60) NOT NULL,
        [AccountType] nvarchar(20) NOT NULL,
        [BankBranch] nvarchar(30) NULL,
        [InitialBalance] decimal(18,2) NOT NULL,
        [WorkingBalance] decimal(18,2) NOT NULL,
        [DateEnrolled] datetime2 NOT NULL,
        [Visibility] bit NOT NULL,
        CONSTRAINT [PK_Accounts] PRIMARY KEY ([AccountId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813093218_Initial'
)
BEGIN
    CREATE TABLE [Transactions] (
        [TransactionId] uniqueidentifier NOT NULL,
        [AccountId] uniqueidentifier NOT NULL,
        [TransactionDate] datetime2 NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [TransactionType] nvarchar(10) NOT NULL,
        [TransactionMedium] nvarchar(10) NOT NULL,
        [Note] nvarchar(70) NULL,
        [CashTransactionType] nvarchar(15) NULL,
        [Payee] nvarchar(50) NULL,
        [ChequeBankName] nvarchar(25) NULL,
        [ChequeNumber] nvarchar(15) NULL,
        CONSTRAINT [PK_Transactions] PRIMARY KEY ([TransactionId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813093218_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240813093218_Initial', N'8.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813110722_FixedForeignKeys'
)
BEGIN
    CREATE INDEX [IX_Transactions_AccountId] ON [Transactions] ([AccountId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813110722_FixedForeignKeys'
)
BEGIN
    ALTER TABLE [Transactions] ADD CONSTRAINT [FK_Transactions_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([AccountId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813110722_FixedForeignKeys'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240813110722_FixedForeignKeys', N'8.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823164644_MonthlyBalanceTable'
)
BEGIN
    CREATE TABLE [MonthlyWorkingBalances] (
        [MonthlyWorkingBalanceId] uniqueidentifier NOT NULL,
        [AccountId] uniqueidentifier NOT NULL,
        [Date] datetime2 NOT NULL,
        [WorkingBalance] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_MonthlyWorkingBalances] PRIMARY KEY ([MonthlyWorkingBalanceId]),
        CONSTRAINT [FK_MonthlyWorkingBalances_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([AccountId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823164644_MonthlyBalanceTable'
)
BEGIN
    CREATE UNIQUE INDEX [IX_MonthlyWorkingBalances_AccountId_Date] ON [MonthlyWorkingBalances] ([AccountId], [Date]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823164644_MonthlyBalanceTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240823164644_MonthlyBalanceTable', N'8.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240829065403_FixedCashTransaction'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Transactions]') AND [c].[name] = N'CashTransactionType');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Transactions] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Transactions] ALTER COLUMN [CashTransactionType] nvarchar(20) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240829065403_FixedCashTransaction'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240829065403_FixedCashTransaction', N'8.0.7');
END;
GO

COMMIT;
GO

