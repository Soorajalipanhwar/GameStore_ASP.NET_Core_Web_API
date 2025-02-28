# Game Store API

## Starting SQL Server

```powershel
$sa_password = "Sooraj@3574"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -p 1433:1433 -v sqlvolume:/var/opt/smsql -d --rm --name mssql mcr.microsoft.com/mssql/server:2022-latest
```

## Setting the Connection string to secret manager.

```powershel
$sa_password = "Sooraj@3574"
dotnet user-secrets set "ConnectionStrings:GameStoreContext" "Server=localhost;Database=GameStore; User Id=sa; Password=$sa_password; TrustServerCertificate=True"
```
