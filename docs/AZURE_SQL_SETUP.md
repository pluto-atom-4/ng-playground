# Azure SQL Database Configuration Guide

This guide walks through configuring the ng-playground backend to connect to an Azure SQL Database.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Creating an Azure SQL Database](#creating-an-azure-sql-database)
3. [Connection String Configuration](#connection-string-configuration)
4. [Database Migrations](#database-migrations)
5. [Testing the Connection](#testing-the-connection)
6. [Troubleshooting](#troubleshooting)
7. [Security Best Practices](#security-best-practices)

---

## Prerequisites

Before you begin, ensure you have:

- **Azure Account**: Create a free account at [azure.microsoft.com](https://azure.microsoft.com)
- **.NET 9 SDK**: Installed locally (check with `dotnet --version`)
- **SQL Server Management Studio (SSMS)** (optional): For GUI database management
- **Entity Framework Core CLI Tools** (optional): `dotnet tool install --global dotnet-ef`

### Verify .NET Version
```bash
cd apps/backend
dotnet --version  # Should be 9.0 or higher
```

---

## Creating an Azure SQL Database

### Step 1: Create a Resource Group (if needed)
```bash
az group create --name ng-playground-rg --location eastus
```

### Step 2: Create an Azure SQL Server
```bash
az sql server create \
  --resource-group ng-playground-rg \
  --name ng-playground-sql-<random> \
  --admin-user sqladmin \
  --admin-password YourSecurePassword123!
```

**Note**: Replace `<random>` with a unique identifier (e.g., timestamp or initials). Server names must be globally unique.

### Step 3: Create an Azure SQL Database
```bash
az sql db create \
  --resource-group ng-playground-rg \
  --server ng-playground-sql-<random> \
  --name ng_playground_db \
  --edition Standard \
  --compute-model Serverless \
  --capacity 5
```

### Step 4: Configure Firewall Rules
Allow your local IP to connect:

```bash
az sql server firewall-rule create \
  --resource-group ng-playground-rg \
  --server ng-playground-sql-<random> \
  --name AllowLocalIP \
  --start-ip-address YOUR_IP_ADDRESS \
  --end-ip-address YOUR_IP_ADDRESS
```

To allow all Azure services (including App Service):
```bash
az sql server firewall-rule create \
  --resource-group ng-playground-rg \
  --server ng-playground-sql-<random> \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

**Find your IP**: Visit [whatismyipaddress.com](https://whatismyipaddress.com) or run:
```bash
curl -s https://api.ipify.org
```

---

## Connection String Configuration

### Format: SQL Authentication (Username/Password)

```
Server=tcp:<server-name>.database.windows.net,1433;Initial Catalog=<database-name>;User Id=<username>;Password=<password>;Encrypt=true;Connection Timeout=30;
```

**Example**:
```
Server=tcp:ng-playground-sql-abc123.database.windows.net,1433;Initial Catalog=ng_playground_db;User Id=sqladmin;Password=YourSecurePassword123!;Encrypt=true;Connection Timeout=30;
```

### Format: Azure Active Directory (Recommended for production)

If using Azure AD authentication:
```
Server=tcp:<server-name>.database.windows.net,1433;Initial Catalog=<database-name>;Authentication="Active Directory Default";Encrypt=true;Connection Timeout=30;
```

---

## Configuring appsettings Files

### Step 1: Update `apps/backend/appsettings.json` (Template)

Update the `DefaultConnection` with your Azure SQL server details:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:ng-playground-sql-abc123.database.windows.net,1433;Initial Catalog=ng_playground_db;User Id=sqladmin;Password={your_password};Encrypt=true;Connection Timeout=30;"
  }
}
```

**Important**: Use placeholder values for sensitive credentials in version-controlled files.

### Step 2: Create `apps/backend/appsettings.Development.json` (Local, Git-Ignored)

This file is **automatically git-ignored** and holds your real credentials:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:ng-playground-sql-abc123.database.windows.net,1433;Initial Catalog=ng_playground_db;User Id=sqladmin;Password=YourActualPassword123!;Encrypt=true;Connection Timeout=30;"
  }
}
```

**Verify it's git-ignored**:
```bash
cd apps/backend
cat .gitignore | grep "appsettings.Development.json"
# Should output: appsettings.Development.json
```

### Step 3: Create `appsettings.Production.json` (Optional, for deployment)

For Azure App Service deployments, configuration can be managed via Azure Key Vault or App Service Configuration settings. Create `appsettings.Production.json` if needed:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Error"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:{deployed-server}.database.windows.net,1433;Initial Catalog={deployed-db};User Id={deployed-user};Password={deployed-password};Encrypt=true;Connection Timeout=30;"
  }
}
```

---

## Database Migrations

### Step 1: Install EF Core CLI (if not already installed)
```bash
dotnet tool install --global dotnet-ef
```

### Step 2: Verify Connection
Test your connection string by running:
```bash
cd apps/backend
dotnet build
```

If the build succeeds, the connection string is valid for the runtime.

### Step 3: Create Initial Migration

```bash
cd apps/backend
dotnet ef migrations add InitialCreate
```

This creates a migration file in `Migrations/` folder documenting the schema changes.

### Step 4: Apply Migration to Database

```bash
dotnet ef database update
```

This creates tables in your Azure SQL Database.

**Expected output**:
```
Build started...
Build succeeded.
Applying migration '20260306220000_InitialCreate'.
Done.
```

### Step 5: Verify Tables Were Created

Connect with SSMS or Azure Data Studio and verify:
- `Aircraft` table
- `ComplianceLogs` table
- `__EFMigrationsHistory` table (internal EF Core tracking)

---

## Testing the Connection

### Method 1: Run Backend Locally

```bash
cd apps/backend
dotnet run
```

Navigate to `http://localhost:5000/api/compliance/overdue`

**Expected responses**:
- ✅ **200 OK with empty array**: `[]` — Connection successful, no data yet
- ❌ **500 Internal Server Error**: Check connection string in logs
- ❌ **Connection timeout**: Check firewall rules

### Method 2: Use Azure Data Studio

1. Download [Azure Data Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio)
2. Connect with:
   - **Server**: `ng-playground-sql-abc123.database.windows.net,1433`
   - **Username**: `sqladmin`
   - **Password**: Your admin password
3. Run test query:
   ```sql
   SELECT * FROM Aircraft;
   ```

### Method 3: Curl Test
```bash
curl -s http://localhost:5000/api/compliance/overdue | jq .
```

---

## Troubleshooting

### Connection Timeout
**Symptom**: `Connection timeout expired`

**Solutions**:
1. Verify firewall rule allows your IP:
   ```bash
   az sql server firewall-rule list --resource-group ng-playground-rg --server <server-name>
   ```
2. Check connection string format (no extra spaces)
3. Verify credentials are correct
4. Test from Azure portal: Home → SQL Database → Connection Strings

### Login Failed
**Symptom**: `Login failed for user 'sqladmin'`

**Solutions**:
1. Verify username in connection string (case-sensitive in some configurations)
2. Reset admin password:
   ```bash
   az sql server update --resource-group ng-playground-rg --server <server-name> --admin-password NewPassword123!
   ```
3. Check password contains valid characters (no special chars that break connection strings)

### Database Not Found
**Symptom**: `Cannot open database "ng_playground_db" requested by the login`

**Solutions**:
1. Verify database name in `Initial Catalog=<name>`
2. List existing databases:
   ```bash
   az sql db list --resource-group ng-playground-rg --server <server-name> --query "[].name"
   ```
3. Create database if missing (see [Step 3](#step-3-create-an-azure-sql-database))

### Entity Framework Errors
**Symptom**: `The entity type 'Aircraft' requires a primary key to be defined`

**Solution**: Already fixed in the implementation. Verify models have `[Key]` attributes:
```csharp
public class Aircraft
{
    [Key]
    public int AircraftId { get; set; }
    // ...
}
```

### Migration Issues
**Symptom**: `An error occurred while accessing the Microsoft.EntityFrameworkCore.Metadata.IConventionEntityTypeBuilder.HasNoKey...`

**Solutions**:
1. Delete `Migrations/` folder and start over:
   ```bash
   rm -rf Migrations/
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
2. Ensure all DbSet properties in `AppDbContext` are defined:
   ```csharp
   public DbSet<Aircraft> Aircraft { get; set; }
   public DbSet<ComplianceLog> ComplianceLogs { get; set; }
   ```

---

## Security Best Practices

### 1. **Never Commit Credentials**
- ✅ Do: Use `appsettings.Development.json` (git-ignored)
- ❌ Don't: Hardcode passwords in `appsettings.json`

### 2. **Use Strong Passwords**
Azure SQL requires:
- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one number
- At least one special character (e.g., !, @, #, $)

**Example**: `MySecurePass123!@#`

### 3. **Principle of Least Privilege**
Create a separate application user with limited permissions:

```sql
-- In SSMS/Azure Data Studio
CREATE LOGIN app_user WITH PASSWORD = 'AppPassword123!@#';
CREATE USER app_user FOR LOGIN app_user;
ALTER ROLE db_datareader ADD MEMBER app_user;
ALTER ROLE db_datawriter ADD MEMBER app_user;
```

Use `app_user` credentials for the application (not admin).

### 4. **Firewall Rules**
- ✅ Whitelist only necessary IPs
- ✅ Use smallest IP range possible
- ❌ Avoid `0.0.0.0/0` in production

### 5. **Use Azure Key Vault for Secrets (Production)**

Store connection strings in Azure Key Vault instead of files:

```csharp
// In Program.cs (production)
var keyVaultUrl = new Uri("https://your-keyvault.vault.azure.net/");
var credential = new DefaultAzureCredential();
var client = new SecretClient(keyVaultUrl, credential);

var secret = await client.GetSecretAsync("ConnectionString");
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    { "ConnectionStrings:DefaultConnection", secret.Value.Value }
});
```

### 6. **Encrypt Connections**
- ✅ Always use `Encrypt=true` in connection strings
- ✅ Use TLS 1.2 minimum (default for Azure SQL)

### 7. **Monitoring & Auditing**
Enable Azure SQL auditing:
```bash
az sql server audit-policy update \
  --resource-group ng-playground-rg \
  --server <server-name> \
  --state Enabled
```

---

## Next Steps

1. ✅ Create Azure SQL Database (see [Creating an Azure SQL Database](#creating-an-azure-sql-database))
2. ✅ Configure `appsettings.Development.json`
3. ✅ Run migrations: `dotnet ef database update`
4. ✅ Seed sample data (optional)
5. ✅ Test endpoint: `http://localhost:5000/api/compliance/overdue`
6. ✅ Deploy to Azure App Service (future step)

---

## Useful Azure CLI Commands

```bash
# List all SQL servers
az sql server list --query "[].name"

# List all databases for a server
az sql db list --server <server-name> --resource-group <group-name> --query "[].name"

# Get connection string
az sql db show-connection-string --client sqlcmd --server <server-name> --name <db-name>

# Delete database (cleanup)
az sql db delete --server <server-name> --name <db-name> --resource-group <group-name> --yes

# Delete server (cleanup)
az sql server delete --resource-group <group-name> --name <server-name> --yes
```

---

## Useful Links

- [Azure SQL Documentation](https://learn.microsoft.com/en-us/azure/azure-sql/database/sql-database-paas-overview)
- [Entity Framework Core with SQL Server](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/)
- [Connection Strings Reference](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring)
- [Azure Key Vault Integration](https://learn.microsoft.com/en-us/azure/key-vault/general/overview)
- [SQL Server Firewall Rules](https://learn.microsoft.com/en-us/azure/azure-sql/database/firewall-configure)

---

## FAQ

**Q: Can I use a free tier?**
A: Azure SQL Database no longer offers a free tier, but you can use Azure's free trial credits ($200 for 30 days). The Serverless tier with 5 compute units is very affordable (~$6-8/month for light usage).

**Q: How do I add more users to the database?**
A: See the [Security Best Practices](#security-best-practices) section for creating application users.

**Q: Can I use local SQL Server instead?**
A: Yes! Modify the connection string:
```
Server=localhost;Initial Catalog=ng_playground_db;User Id=sa;Password=YourPassword123!;Encrypt=false;TrustServerCertificate=true;
```

**Q: How do I back up my database?**
A: Azure automatically backs up Azure SQL databases. See [Automated backups](https://learn.microsoft.com/en-us/azure/azure-sql/database/automated-backups-overview).

---

## Document Version
- **Last Updated**: March 6, 2026
- **Backend Version**: .NET 9
- **EF Core Version**: 9.0.0
- **Database**: Azure SQL Database (Serverless tier)
