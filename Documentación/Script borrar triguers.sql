use escuelabd

go
DECLARE @SQL NVARCHAR(MAX) = '';

SELECT @SQL = @SQL + 
    'DROP TRIGGER ' 
    + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) 
    + '.' + QUOTENAME(name) + ';' + CHAR(10)
FROM sys.triggers
WHERE parent_class_desc = 'OBJECT_OR_COLUMN';

PRINT @SQL;

EXEC sp_executesql @SQL;