-- STORED P

USE [MAINDBNAME]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddUser]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_AddUser]
    @pLogin NVARCHAR(50), 
    @pPassword NVARCHAR(50),
    @pFirstName NVARCHAR(40) = NULL, 
    @pLastName NVARCHAR(40) = NULL,
    @responseMessage NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @salt UNIQUEIDENTIFIER=NEWID()
    BEGIN TRY

        INSERT INTO dbo.[User] (LoginName, PasswordHash, Salt, FirstName, LastName)
        VALUES(@pLogin, HASHBYTES('SHA2_512', @pPassword+CAST(@salt AS NVARCHAR(36))), @salt, @pFirstName, @pLastName)

       SET @responseMessage='Success'

    END TRY
    BEGIN CATCH
        SET @responseMessage=ERROR_MESSAGE() 
    END CATCH

END


-- ======================================================

USE [MAINDBNAME]
GO
/****** Object:  StoredProcedure [dbo].[sp_UserLogin]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_UserLogin]
    @pLoginName NVARCHAR(254),
    @pPassword NVARCHAR(50),
    @responseMessage NVARCHAR(250)='' OUTPUT
AS
BEGIN

    SET NOCOUNT ON

    DECLARE @userID INT

    IF EXISTS (SELECT TOP 1 UserID FROM [dbo].[User] WHERE LoginName=@pLoginName)
    BEGIN
        SET @userID=(SELECT UserID FROM [dbo].[User] WHERE LoginName=@pLoginName AND PasswordHash=HASHBYTES('SHA2_512', @pPassword+CAST(Salt AS NVARCHAR(36))))

       IF(@userID IS NULL)
           SET @responseMessage=0
       ELSE 
           SET @responseMessage=@userID
    END
    ELSE
       SET @responseMessage=0 
END
	return @responseMessage 