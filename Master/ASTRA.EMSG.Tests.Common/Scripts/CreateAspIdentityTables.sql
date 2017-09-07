CREATE TABLE AspMandantRole (
	Id int IDENTITY(1,1) NOT NULL,
	MandantName nvarchar(200) NOT NULL,
	RoleName nvarchar(200) NOT NULL,
	UserId nvarchar(128) NOT NULL,
 CONSTRAINT PK_AspMandantRole PRIMARY KEY 
( Id )
);

CREATE TABLE AspNetRoles (
	Id nvarchar(128) NOT NULL,
	Name nvarchar(256) NOT NULL,
 CONSTRAINT PK_AspNetRoles PRIMARY KEY  
( Id )
);

CREATE TABLE AspNetUserClaims (
	Id int IDENTITY(1,1) NOT NULL,
	UserId nvarchar(128) NOT NULL,
	ClaimType nvarchar(200) NULL,
	ClaimValue nvarchar(200) NULL,
 CONSTRAINT PK_AspNetUserClaims PRIMARY KEY 
( Id )
);

CREATE TABLE AspNetUserLogins (
	LoginProvider nvarchar(128) NOT NULL,
	ProviderKey nvarchar(128) NOT NULL,
	UserId nvarchar(128) NOT NULL,
 CONSTRAINT PK_AspNetUserLogins PRIMARY KEY 
(	LoginProvider , ProviderKey , UserId )
);

CREATE TABLE AspNetUserRoles (
	UserId nvarchar(128) NOT NULL,
	RoleId nvarchar(128) NOT NULL,
 CONSTRAINT PK_AspNetUserRoles PRIMARY KEY 
( UserId , RoleId )
);

CREATE TABLE AspNetUsers (
	Id nvarchar(128) NOT NULL,
	Email nvarchar(256) NULL,
	EmailConfirmed bit NOT NULL,
	PasswordHash nvarchar(400) NULL,
	SecurityStamp nvarchar(400) NULL,
	PhoneNumber nvarchar(200) NULL,
	PhoneNumberConfirmed bit NOT NULL,
	TwoFactorEnabled bit NOT NULL,
	LockoutEndDateUtc datetime NULL,
	LockoutEnabled bit NOT NULL,
	AccessFailedCount int NOT NULL,
	UserName nvarchar(256) NOT NULL,
 CONSTRAINT PK_AspNetUsers PRIMARY KEY 
( Id )
);

ALTER TABLE AspMandantRole  ADD CONSTRAINT FK_AspMandantRole_AspNetUsers_UserId FOREIGN KEY(UserId)
REFERENCES AspNetUsers ( Id )
ON DELETE CASCADE
;

ALTER TABLE AspNetUserClaims  ADD  CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY(UserId)
REFERENCES AspNetUsers (Id)
ON DELETE CASCADE
;

ALTER TABLE AspNetUserLogins  ADD  CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY(UserId)
REFERENCES AspNetUsers (Id)
ON DELETE CASCADE
;

ALTER TABLE AspNetUserRoles   ADD  CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY(RoleId)
REFERENCES AspNetRoles (Id)
ON DELETE CASCADE
;

ALTER TABLE AspNetUserRoles  ADD  CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY(UserId)
REFERENCES AspNetUsers (Id)
ON DELETE CASCADE
