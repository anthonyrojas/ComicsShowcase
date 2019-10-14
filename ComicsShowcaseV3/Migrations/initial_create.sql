
Welcome to .NET Core 3.0!
---------------------
SDK Version: 3.0.100

Telemetry
---------
The .NET Core tools collect usage data in order to help us improve your experience. The data is anonymous. It is collected by Microsoft and shared with the community. You can opt-out of telemetry by setting the DOTNET_CLI_TELEMETRY_OPTOUT environment variable to '1' or 'true' using your favorite shell.

Read more about .NET Core CLI Tools telemetry: https://aka.ms/dotnet-cli-telemetry

----------------
Explore documentation: https://aka.ms/dotnet-docs
Report issues and find source on GitHub: https://github.com/dotnet/core
Find out what's new: https://aka.ms/dotnet-whats-new
Learn about the installed HTTPS developer cert: https://aka.ms/aspnet-core-https
Use 'dotnet --help' to see available commands or visit: https://aka.ms/dotnet-cli-docs
Write your first app: https://aka.ms/first-net-core-app
--------------------------------------------------------------------------------------
[40m[32minfo[39m[22m[49m: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 3.0.0 initialized 'ComicsContext' using provider 'Microsoft.EntityFrameworkCore.SqlServer' with options: None
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Users] (
    [ID] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [BirthDate] nvarchar(max) NOT NULL,
    [ProfileStr] nvarchar(max) NULL,
    [Profile] varbinary(max) NULL,
    [redditUsername] nvarchar(max) NULL,
    [instagramUsername] nvarchar(max) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [Collectibles] (
    [ID] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [ImageStr] nvarchar(max) NULL,
    [ImageData] varbinary(max) NULL,
    [UPC] bigint NOT NULL,
    [Autographed] bit NOT NULL,
    [ItemCategory] nvarchar(max) NOT NULL,
    [UserID] int NULL,
    CONSTRAINT [PK_Collectibles] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Collectibles_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([ID]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Comics] (
    [ID] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [UPC] bigint NOT NULL,
    [FiveDigitId] int NOT NULL,
    [ImageStr] nvarchar(max) NULL,
    [ImageData] varbinary(max) NULL,
    [Publisher] nvarchar(max) NOT NULL,
    [Conidition] nvarchar(max) NOT NULL,
    [UserID] int NOT NULL,
    CONSTRAINT [PK_Comics] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Comics_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [GraphicNovels] (
    [ID] int NOT NULL IDENTITY,
    [ISBN] bigint NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [ImageStr] nvarchar(max) NULL,
    [ImageData] varbinary(max) NULL,
    [GraphicNovelType] int NOT NULL,
    [BookCondition] nvarchar(max) NOT NULL,
    [Publisher] nvarchar(max) NOT NULL,
    [UserID] int NOT NULL,
    CONSTRAINT [PK_GraphicNovels] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_GraphicNovels_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [Creators] (
    [ID] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [UserID] int NOT NULL,
    [ComicBookID] int NULL,
    [GraphicNovelID] int NULL,
    CONSTRAINT [PK_Creators] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Creators_Comics_ComicBookID] FOREIGN KEY ([ComicBookID]) REFERENCES [Comics] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Creators_GraphicNovels_GraphicNovelID] FOREIGN KEY ([GraphicNovelID]) REFERENCES [GraphicNovels] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Creators_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([ID]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Collectibles_UserID] ON [Collectibles] ([UserID]);

GO

CREATE INDEX [IX_Comics_UserID] ON [Comics] ([UserID]);

GO

CREATE INDEX [IX_Creators_ComicBookID] ON [Creators] ([ComicBookID]);

GO

CREATE INDEX [IX_Creators_GraphicNovelID] ON [Creators] ([GraphicNovelID]);

GO

CREATE INDEX [IX_Creators_UserID] ON [Creators] ([UserID]);

GO

CREATE INDEX [IX_GraphicNovels_UserID] ON [GraphicNovels] ([UserID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191007050414_initial_create', N'3.0.0');

GO


