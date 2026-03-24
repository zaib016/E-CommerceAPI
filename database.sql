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
CREATE TABLE [Categories] (
    [CategoryId] int NOT NULL IDENTITY,
    [CategoryName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
);

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);

CREATE TABLE [Products] (
    [ProductId] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [ProductName] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [Stock] nvarchar(max) NOT NULL,
    [Price] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE CASCADE
);

CREATE TABLE [Orders] (
    [OrderId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ProductId] int NOT NULL,
    [TotalAmount] nvarchar(max) NOT NULL,
    [OrderTime] datetime2 NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK_Orders_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);

CREATE INDEX [IX_Orders_ProductId] ON [Orders] ([ProductId]);

CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260304165730_initailMigration', N'9.0.13');

COMMIT;
GO

