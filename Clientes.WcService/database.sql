-- Criação do banco (se não existir)
IF DB_ID('ClientesDb') IS NULL
BEGIN
    CREATE DATABASE ClientesDb;
END
GO

USE ClientesDb;
GO

-- ======================================================
-- 1. Tabelas
-- ======================================================

-- Exclui se já existir
IF OBJECT_ID('dbo.Clientes') IS NOT NULL DROP TABLE dbo.Clientes;
IF OBJECT_ID('dbo.SituacoesCliente') IS NOT NULL DROP TABLE dbo.SituacoesCliente;
GO

-- Situações do cliente
CREATE TABLE dbo.SituacoesCliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL
);
GO

-- Clientes
CREATE TABLE dbo.Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(200) NOT NULL,
    Cpf CHAR(11) NOT NULL UNIQUE,
    DataNascimento DATE NOT NULL,
    Sexo CHAR(1) NULL, -- 'M' ou 'F'
    IdSituacao INT NOT NULL,
    CONSTRAINT FK_Clientes_Situacao FOREIGN KEY (IdSituacao) REFERENCES dbo.SituacoesCliente(Id)
);
GO

-- ======================================================
-- 2. Procedures
-- ======================================================

-- Lista todos os clientes
IF OBJECT_ID('dbo.sp_Cliente_List') IS NOT NULL DROP PROCEDURE dbo.sp_Cliente_List;
GO
CREATE PROCEDURE dbo.sp_Cliente_List
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.Id,
        c.Nome,
        c.Cpf,
        c.DataNascimento,
        c.Sexo,
        c.IdSituacao
    FROM dbo.Clientes c
    ORDER BY c.Nome;
END;
GO

-- Obtém cliente por Id
IF OBJECT_ID('dbo.sp_Cliente_GetById') IS NOT NULL DROP PROCEDURE dbo.sp_Cliente_GetById;
GO
CREATE PROCEDURE dbo.sp_Cliente_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.Id,
        c.Nome,
        c.Cpf,
        c.DataNascimento,
        c.Sexo,
        c.IdSituacao
    FROM dbo.Clientes c
    WHERE c.Id = @Id;
END;
GO

-- Inserir cliente
IF OBJECT_ID('dbo.sp_Cliente_Insert') IS NOT NULL DROP PROCEDURE dbo.sp_Cliente_Insert;
GO
CREATE PROCEDURE dbo.sp_Cliente_Insert
    @Nome NVARCHAR(200),
    @Cpf CHAR(11),
    @DataNascimento DATE,
    @Sexo CHAR(1) = NULL,
    @IdSituacao INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Clientes (Nome, Cpf, DataNascimento, Sexo, IdSituacao)
    VALUES (@Nome, @Cpf, @DataNascimento, @Sexo, @IdSituacao);
END;
GO

-- Atualizar cliente
IF OBJECT_ID('dbo.sp_Cliente_Update') IS NOT NULL DROP PROCEDURE dbo.sp_Cliente_Update;
GO
CREATE PROCEDURE dbo.sp_Cliente_Update
    @Id INT,
    @Nome NVARCHAR(200),
    @Cpf CHAR(11),
    @DataNascimento DATE,
    @Sexo CHAR(1) = NULL,
    @IdSituacao INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Clientes
    SET Nome = @Nome,
        Cpf = @Cpf,
        DataNascimento = @DataNascimento,
        Sexo = @Sexo,
        IdSituacao = @IdSituacao
    WHERE Id = @Id;
END;
GO

-- Deletar cliente
IF OBJECT_ID('dbo.sp_Cliente_Delete') IS NOT NULL DROP PROCEDURE dbo.sp_Cliente_Delete;
GO
CREATE PROCEDURE dbo.sp_Cliente_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Clientes WHERE Id = @Id;
END;
GO

-- Lista todas as situações
IF OBJECT_ID('dbo.sp_Situacao_List') IS NOT NULL DROP PROCEDURE dbo.sp_Situacao_List;
GO
CREATE PROCEDURE dbo.sp_Situacao_List
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome FROM dbo.SituacoesCliente ORDER BY Nome;
END;
GO

-- ======================================================
-- 3. Dados iniciais (opcional)
-- ======================================================

INSERT INTO dbo.SituacoesCliente (Nome) VALUES ('Ativo');
INSERT INTO dbo.SituacoesCliente (Nome) VALUES ('Inativo');
INSERT INTO dbo.SituacoesCliente (Nome) VALUES ('Bloqueado');
GO
