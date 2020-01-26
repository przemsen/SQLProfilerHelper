exec sp_executesql N'SELECT [t0].[InvoiceId], [t0].[CustomerId], [t0].[InvoiceDate], [t0].[BillingAddress], [t0].[BillingCity], [t0].[BillingState], [t0].[BillingCountry], [t0].[BillingPostalCode], [t0].[Total]
FROM [Invoice] AS [t0]
WHERE [t0].[InvoiceDate] < @p0',N'@p0 datetime',@p0='2017-10-22 12:32:27.070'

exec sp_executesql N'SELECT [t0].[InvoiceId], [t0].[CustomerId], [t0].[InvoiceDate], [t0].[BillingAddress], [t0].[BillingCity], [t0].[BillingState], [t0].[BillingCountry], [t0].[BillingPostalCode], [t0].[Total]
FROM [Invoice] AS [t0]
WHERE [t0].[InvoiceDate] < @p0',N'@p0 datetime',@p0='2017-10-22 12:32:27.070'
go

exec sp_executesql N'SELECT [t0].[InvoiceId], [t0].[CustomerId], [t0].[InvoiceDate], [t0].[BillingAddress], [t0].[BillingCity], [t0].[BillingState], [t0].[BillingCountry], [t0].[BillingPostalCode], [t0].[Total]
FROM [Invoice] AS [t0]
WHERE [t0].[InvoiceDate] < @p0 AND [t0].[id] = @p1',N'@p0 datetime, @p1 int',@p0='2017-10-22 12:32:27.070',@p1=1
GO

exec sp_executesql N'SELECT [t0].[InvoiceId], [t0].[CustomerId],
[t0].[InvoiceDate], [t0].[BillingAddress], [t0].[BillingCity],
[t0].[BillingState], [t0].[BillingCountry], [t0].[BillingPostalCode], [t0].[Total],
@p2 AS TestLongValue
FROM [Invoice] AS [t0]
WHERE [t0].[InvoiceDate] < @p0 AND [t0].[id] = @p1',N'@p0 datetime, @p1 int, @p2 nvarchar(2000)',@p0='2017-10-22 12:32:27.070',@p1=1,@p2=N'hahahaha, hahaha'
GO