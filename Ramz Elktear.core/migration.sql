BEGIN TRANSACTION;
ALTER TABLE [dbo].[CarImages] ADD [ColorsId] nvarchar(450) NULL;

CREATE INDEX [IX_CarImages_ColorsId] ON [dbo].[CarImages] ([ColorsId]);

ALTER TABLE [dbo].[CarImages] ADD CONSTRAINT [FK_CarImages_Colors_ColorsId] FOREIGN KEY ([ColorsId]) REFERENCES [dbo].[Colors] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250718181632_AddColorToImageCar', N'9.0.1');

COMMIT;
GO

