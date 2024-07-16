# **ForceGetCase**

## RUN

> Ensure MySql database is running on port 3306 with user 'root' and password 'root' (or change the ConnectionString in the appsettings.json file)

Install EF Core CLI Tools

```bash
dotnet tool install --global dotnet-ef 
```

Go to project root folder and start a command prompt/terminal.

Run database migrations

```bash
dotnet ef database update --project src/ForceGetCase.DataAccess --startup-project src/ForceGetCase.API
```

Run the application

```bash
dotnet run --project src/ForceGetCase.API
```

On another terminal tab/window, go to web application project folder root and run it.

```bash
cd ForceGetCaseFe
ng serve
```

