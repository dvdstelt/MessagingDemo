Add-Migration -Name "InitialVersion" -ProjectName Sales.Data -StartupProjectName Website -ConnectionStringName SalesDatabase

Update-Database -ProjectName Sales.Data -StartupProjectName Website -ConnectionStringName SalesDatabase