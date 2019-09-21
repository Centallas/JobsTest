USE [CCG]
GO
/****** Object:  UserDefinedFunction [dbo].[fnGetNitData]    Script Date: 21/09/2019 2:45:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--https://stackoverflow.com/questions/44054512/how-to-use-xml-type-valued-functions-in-sql-server-for-xml

ALTER FUNCTION [dbo].[fnGetNitData](@nitID int) RETURNS XML 
AS 
BEGIN 
    RETURN (SELECT  [RAZONSOCIAL],[NOMBRECOMERCIO],[NIT], [CODIGOUNICO] FROM [dbo].[viewnit] WHERE [NIT] = @nitID 
    FOR XML PATH('NIT'),TYPE)
END