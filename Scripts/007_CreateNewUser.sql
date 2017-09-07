-- ===================================================
-- Description: SQL Script for creating a new user
-- Author:	    Balazs Epresi
-- Create date: 14.08.2016
-- ===================================================
-- Parameters: Insert needed Parameters below, right after the BEGIN - Statement.
-- Username: the choosen username
-- Mandantowner: the Mandant's Owner's ID (e.g.: 1230)
-- Globalroles:  Bit representation of a boolean, if the value is 1, then the user will have the global rights (Applikationsadministrator, Applikationssupporter)
-- Mandantroles: Bit representation of a boolean, if the value is 1, then the user will have the rights for the given Mandant
-- Password: by default the user will have the '123456' password, please change it after the first log in
-- ===================================================

DECLARE
    @USERID uniqueidentifier,
	@USERNAME VARCHAR(255),
	@MANDANTOWNER VARCHAR(4),
	@GLOBALROLES BIT,
	@MANDANTROLES BIT;

BEGIN
    SET @USERID = NEWID();
	SET @USERNAME = 'UserName';	
    SET @MANDANTOWNER = '1001';
    SET @GLOBALROLES = 1; --true
    SET @MANDANTROLES = 1;
	
	INSERT INTO AspNetUsers(Id, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName) 
	VALUES(@USERID, 0, 'APiPUcotBWoktSAFRzHyVmuYQEXxft6GRSVTOV2qRZxof3A5wHVNsd/KZMX/gFjw8Q==', NEWID(), 0, 0, 0, 0, @USERNAME)

	IF (@GLOBALROLES = 1) 
		BEGIN	
			INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES(@USERID, '8de45505-ec94-4835-aa90-73345f588ce4') --Applikationsadministrator
			INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES(@USERID, '8033a13f-b98c-4bff-9464-96822639bf55') --Applikationssupporter
		END
	IF (@MANDANTROLES = 1) 
		BEGIN
			INSERT INTO AspMandantRole(MandantName, RoleName, UserId) VALUES(@MANDANTOWNER, 'Benchmarkteilnehmer', @USERID)
			INSERT INTO AspMandantRole(MandantName, RoleName, UserId) VALUES(@MANDANTOWNER, 'Benutzeradministrator', @USERID)
			INSERT INTO AspMandantRole(MandantName, RoleName, UserId) VALUES(@MANDANTOWNER, 'DataManager', @USERID)
			INSERT INTO AspMandantRole(MandantName, RoleName, UserId) VALUES(@MANDANTOWNER, 'DataReader', @USERID)
		END
END
