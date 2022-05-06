
delete from AspNetRoles;
delete from AspNetUserRoles;
go

insert into AspNetRoles (Id, Name, NormalizedName) values ('1', 'User', 'User');
insert into AspNetRoles (Id, Name, NormalizedName) values ('2', 'Admin', 'Admin');

insert into AspNetUserRoles values ('227bef81-22f2-4120-a02f-1a69e65fbb05', '1');
insert into AspNetUserRoles values ('227bef81-22f2-4120-a02f-1a69e65fbb05', '2');
go