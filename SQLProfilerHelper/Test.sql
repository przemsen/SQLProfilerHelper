exec sp_executesql N'SELECT [t0].[InvoiceId], [t0].[CustomerId], [t0].[InvoiceDate], [t0].[BillingAddress], [t0].[BillingCity], [t0].[BillingState], [t0].[BillingCountry], [t0].[BillingPostalCode], [t0].[Total]
FROM [Invoice] AS [t0]
WHERE [t0].[InvoiceDate] < @p0',N'@p0 datetime',@p0='2017-10-22 12:32:27.070'