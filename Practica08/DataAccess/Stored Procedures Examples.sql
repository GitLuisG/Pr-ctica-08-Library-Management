CREATE PROCEDURE dbo.InsertUsuario
    @username nvarchar(64) = NULL,
    @password nvarchar(128) = NULL,
    @nombre nvarchar(256) = NULL,
    @apellidos nvarchar(256) = NULL,
    @email nvarchar(256) = NULL
AS
BEGIN

INSERT INTO dbo.Usuarios 
        (Username, [Password], Nombre, Apellidos, Email)
    VALUES
        (@username, @password, @nombre, @apellidos, @email);

SELECT CONVERT(int, SCOPE_IDENTITY()) Id;

END
GO;
----------------


CREATE PROCEDURE dbo.UpdateUsuario
    @id int = 0,
    @username nvarchar(64) = NULL,
    @password nvarchar(128) = NULL,
    @nombre nvarchar(256) = NULL,
    @apellidos nvarchar(256) = NULL,
    @email nvarchar(256) = NULL
AS
BEGIN

UPDATE dbo.Usuarios 
    SET 
         Nombre = @nombre
        ,Apellidos = @apellidos
        ,Username = @username
        ,[Password] = @password
        ,Email = @email
    WHERE Id = @id;

END


