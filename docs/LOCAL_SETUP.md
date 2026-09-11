# Local Setup Notes

SmartERP is a mature .NET Framework/WPF solution with SQL Server and DevExpress dependencies. These notes are intentionally environment-neutral so no production or private infrastructure details are stored in the repository.

## Prerequisites

- Windows development environment
- Visual Studio with .NET Framework desktop development tooling
- .NET Framework 4.8 support
- SQL Server / SQL Server Developer Edition
- NuGet package restore
- Appropriate DevExpress WPF tooling/licence for UI components used by the desktop client
- Microsoft Outlook if testing Office Interop functionality

## Database Configuration

Do not commit real credentials to `App.config` or source files.

For SQL-authenticated development, set the password in your local environment:

```powershell
$env:SMARTERP_SQL_PASSWORD = "your-local-development-password"
```

Configure your local `DBContextERP` connection string with a development-only SQL Server instance and database. Keep machine names, usernames and passwords out of commits.

Windows Authentication can also be used where the relevant application path supports it.

## Restore and Build

1. Clone the repository.
2. Restore NuGet packages.
3. Configure the local SQL Server connection.
4. Ensure the required DevExpress references are available.
5. Open the solution/projects in Visual Studio.
6. Build the business layer first, then the WPF client.

Because this repository represents a long-running enterprise application, local build requirements may depend on the versions of third-party components originally used by the solution.

## Security

- Never use production credentials for portfolio/local builds.
- Never commit passwords, API keys or private connection strings.
- Prefer Windows Authentication, environment variables, local user-secrets-equivalent configuration or deployment-time secret injection where appropriate.
- If a credential has ever been committed publicly, rotate it even after removing it from the current version of the file.

## Portfolio Scope

The primary purpose of the public repository is technical review of the architecture, domain modelling, WPF application code and ERP engineering work. It is not distributed as a ready-to-deploy production package.
