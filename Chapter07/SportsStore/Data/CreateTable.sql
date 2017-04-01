/*
SportsStore 的部署脚本

此代码由工具生成。
如果重新生成此代码，则对此文件的更改可能导致
不正确的行为并将丢失。
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "SportsStore"
:setvar DefaultFilePrefix "SportsStore"
:setvar DefaultDataPath "C:\Users\Administrator\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb\"
:setvar DefaultLogPath "C:\Users\Administrator\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb\"

GO
:on error exit
GO
/*
请检测 SQLCMD 模式，如果不支持 SQLCMD 模式，请禁用脚本执行。
要在启用 SQLCMD 模式后重新启用脚本，请执行:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'要成功执行此脚本，必须启用 SQLCMD 模式。';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO

IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
GO
BEGIN TRANSACTION
GO
PRINT N'正在创建 [dbo].[Products]...';


GO
CREATE TABLE [dbo].[Products] (
    [ProductID]   INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100)  NOT NULL,
    [Description] NVARCHAR (500)  NOT NULL,
    [Category]    NVARCHAR (50)   NOT NULL,
    [Price]       DECIMAL (16, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC)
);


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO

IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT N'数据库更新的事务处理部分成功。'
COMMIT TRANSACTION
END
ELSE PRINT N'数据库更新的事务处理部分失败。'
GO
DROP TABLE #tmpErrors
GO
PRINT N'更新完成。';


GO
