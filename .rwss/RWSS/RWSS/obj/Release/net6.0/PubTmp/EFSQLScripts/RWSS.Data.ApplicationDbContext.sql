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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611191722_InitialCreate')
BEGIN
    CREATE TABLE [AppUser] (
        [Id] int NOT NULL IDENTITY,
        [Year] int NOT NULL,
        [IndexNumber] int NOT NULL,
        CONSTRAINT [PK_AppUser] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611191722_InitialCreate')
BEGIN
    CREATE TABLE [DeaneryWorker] (
        [Id] int NOT NULL IDENTITY,
        [Position] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_DeaneryWorker] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611191722_InitialCreate')
BEGIN
    CREATE TABLE [Events] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [EventCategory] int NOT NULL,
        [AppUserId] int NOT NULL,
        [DeaneryWorkerId] int NOT NULL,
        CONSTRAINT [PK_Events] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Events_AppUser_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [AppUser] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Events_DeaneryWorker_DeaneryWorkerId] FOREIGN KEY ([DeaneryWorkerId]) REFERENCES [DeaneryWorker] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611191722_InitialCreate')
BEGIN
    CREATE INDEX [IX_Events_AppUserId] ON [Events] ([AppUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611191722_InitialCreate')
BEGIN
    CREATE INDEX [IX_Events_DeaneryWorkerId] ON [Events] ([DeaneryWorkerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611191722_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230611191722_InitialCreate', N'7.0.5');
END;
GO

COMMIT;
GO

