# File Task API

### Configuring the sample to use SQL Server

1. Ensure your connection strings in `appsettings.json` point to a local SQL Server instance.
1. Ensure the tool EF was already installed. You can find some help [here](https://docs.microsoft.com/ef/core/miscellaneous/cli/dotnet)

    ```
    dotnet tool update --global dotnet-ef
    ```

1. Open a command prompt in the Data folder and execute the following commands:

    ```
    dotnet restore
    dotnet tool restore
    dotnet ef --startup-project ../FileAppTask.API migrations add InitialMigration --context FileContext
    dotnet ef --startup-project ../FileAppTask.API database update InitialMigration --context FileContext
    ```

    These commands will create database.

1. Run the application.
