# EntityFrameworkCore Football League Solution

This solution demonstrates a football league management system using Entity Framework Core with a SQL Server backend. It includes projects for the domain models, data access, and a console application for running queries and commands.

## Project Structure

- **EntityFrameworkCore.Domain**: Contains domain models such as `Team`, `Coach`, `League`, and `Match`.
- **EntityFrameworkCore.Data**: Contains the `FootballLeagueDbContext`, EF Core configurations, and migration files.
- **EntityFrameworkCore.Console**: Console app for interacting with the database using various EF Core queries.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)
- [SQL Server Docker Image](https://hub.docker.com/_/microsoft-mssql-server)

## Setting Up the Environment

### 1. Set the SQL Server SA Password

The Docker Compose file expects an environment variable `DB_PASSWORD` for the SQL Server `sa` user.

**On Windows (Command Prompt):**
```sh
set DB_PASSWORD=YourStrong!Passw0rd
```

**On PowerShell:**
```sh
$env:DB_PASSWORD="YourStrong!Passw0rd"
```

**On Linux/macOS:**
```sh
export DB_PASSWORD=YourStrong!Passw0rd
```

> **Note:** Replace `YourStrong!Passw0rd` with a secure password of your choice.

### 2. Start SQL Server with Docker Compose

From the root of the solution (where `docker-compose.yaml` is located), run:

```sh
docker compose up -d
```

This will start a SQL Server instance on port 1433.

### 3. Apply Entity Framework Core Migrations

Navigate to the solution root (where the `.sln` file is located), and run the following command to apply migrations:

```sh
dotnet ef database update --project EntityFrameworkCore.Data --startup-project EntityFrameworkCore.Console
```

- `--project EntityFrameworkCore.Data` specifies the project containing your `DbContext` and migrations.
- `--startup-project EntityFrameworkCore.Console` specifies the executable project that configures your services and entry point.

This will create the database and apply all migrations from the `Migrations` folder.

> **Tip:** If you need to install the EF Core CLI tools, run:
> ```sh
> dotnet tool install --global dotnet-ef
> ```

### 4. Run the Console Application

Navigate to the `EntityFrameworkCore.Console` project and run:

```sh
cd EntityFrameworkCore.Console
dotnet run
```

This will execute the main program, which includes various EF Core queries and commands.

## Useful Commands

- **View running containers:**
  ```sh
  docker ps
  ```
- **Stop the SQL Server container:**
  ```sh
  docker compose down
  ```

## Migrations

All migration files are located in [`EntityFrameworkCore.Data/Migrations`](EntityFrameworkCore.Data/Migrations). You can add new migrations from root folder using:

```sh
dotnet ef migrations add MigrationName --project EntityFrameworkCore.Data --startup-project EntityFrameworkCore.Console
```

## License

This project is for educational purposes.

---

**Author:**  
Toka Thanos
