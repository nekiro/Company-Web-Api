IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Companies] (
    [CompanyID] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [EstablishmentYear] int NOT NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([CompanyID])
);

GO

CREATE TABLE [Employees] (
    [EmployeeID] bigint NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [JobTitle] int NOT NULL,
    [CompanyID] bigint NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeID]),
    CONSTRAINT [FK_Employees_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([CompanyID]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Employees_CompanyID] ON [Employees] ([CompanyID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201011145721_initial', N'3.1.8');

GO

