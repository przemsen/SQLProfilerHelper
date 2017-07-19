# SQL Profiler Helper

1. Get from a profiler something like

```sql
exec sp_executesql N'SELECT SUM([re.TableA].[Amount])
FROM [dbo].[TableB] AS [re6]
INNER JOIN [dbo].[TableBTransaction] AS [re.TableA] ON [re6].[Id] = [re.TableA].[TableBId]
WHERE ([re6].[TableBTypeId] = @__rePaidTypeId_0) AND (@_outer_Id1 = [re6].[RegistrationId])',N'@__rePaidTypeId_0 int,@_outer_Id1 bigint',
@__rePaidTypeId_0=7,@_outer_Id1=941
```
2. Copy this crap to the clipboard

3. ![alt text](https://raw.githubusercontent.com/przemsen/SQLProfilerHelper/master/sqlprofhlpscreen.png)

4. Paste 

```sql
SELECT SUM
(
  [re.TableA].[Amount]
)
FROM [dbo].[TableB] AS [re6]
INNER JOIN [dbo].[TableBTransaction] AS [re.TableA] ON [re6].[Id] = [re.TableA].[TableBId]
WHERE 
(
  [re6].[TableBTypeId] = 7
) AND 
(
  941 = [re6].[RegistrationId]
)
```
Current executable file can be downloaded from Releases tab.

