 if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_EA2874C]') and parent_object_id = OBJECT_ID(N'Follows'))
alter table Follows  drop constraint FK_EA2874C


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_D52173BA]') and parent_object_id = OBJECT_ID(N'Follows'))
alter table Follows  drop constraint FK_D52173BA


    if exists (select * from dbo.sysobjects where id = object_id(N'[User]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [User]

    if exists (select * from dbo.sysobjects where id = object_id(N'Follows') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Follows

    create table [User] (
        UserId UNIQUEIDENTIFIER not null,
       FirstName NVARCHAR(255) null,
       LastName NVARCHAR(255) null,
       Username NVARCHAR(255) null,
       PasswordHash VARBINARY(MAX) null,
       PasswordSalt VARBINARY(MAX) null,
       Role NVARCHAR(255) null,
       Token NVARCHAR(255) null,
       primary key (UserId)
    )

    create table Follows (
        FollowersId UNIQUEIDENTIFIER not null,
       UserId UNIQUEIDENTIFIER not null
    )

    alter table Follows
        add constraint FK_EA2874C
        foreign key (UserId)
        references [User]

    alter table Follows
        add constraint FK_D52173BA
        foreign key (FollowersId)
        references [User]
