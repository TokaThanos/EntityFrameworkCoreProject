# EntityFrameworkCore Football League Solution

![ERD Diagram](images/erd.png)

This solution demonstrates a football league management system using Entity Framework Core with a SQL Server backend. It includes projects for the domain models, data access, and a console application to interact with the database.

## Project Structure

- **EntityFrameworkCore.Domain**: Contains domain models such as `Team`, `Coach`, `League`, and `Match`.
- **EntityFrameworkCore.Data**: Contains the `FootballLeagueDbContext`, EF Core configurations, and migration files.
- **EntityFrameworkCore.Application**: Contains the business logic.
- **EntityFrameworkCore.Console**: Console app for interacting with the database using various EF Core queries.
- **EntityFrameworkCore.Api**: Web API with CRUD functionalities.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)
- [SQL Server Docker Image](https://hub.docker.com/_/microsoft-mssql-server)

## Setting Up the Environment

### 1. Set the SQL Server SA Password and JWT Key with a `.env` File

The project uses a `.env` file to store environment variables, including the SQL Server `sa` password and a secure JWT signing key.

1. Create a file named `.env` in the root of your project.
2. Add the following lines, replacing `YourStrong!Passw0rd` with your password and `Base64Encoded64ByteKeyHere` with your generated signing key:
   ```
   DB_PASSWORD=YourStrong!Passw0rd
   JWT_KEY=Base64Encoded64ByteKeyHere
   SQL_CONNECTION_STRING=Server=localhost,1433;Database=FootballLeague_EfCore;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
   ```
   The `Database=FootballLeague_EfCore;` part is the actual name EF Core will use when creating the database.
   
4. Generate a secure 64-byte JWT key (Base64 encoded) — recommended for HS512:

- **PowerShell (Windows)**:
  ```powershell
  $bytes = New-Object byte[] 64
  [System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)
  [Convert]::ToBase64String($bytes)
  ```

- **Terminal (Linux/macOS)**:
  ```bash
  openssl rand -base64 64
  ```

4. Copy the generated Base64 string into `JWT_KEY` in `.env`.

> **Note:** Do not commit your actual `.env` file with sensitive information to version control.

### 2. Start SQL Server with Docker Compose

From the root of the solution (where `docker-compose.yaml` is located), run:

```sh
docker compose up -d
```

This will start a SQL Server instance on port 1433.

### 3. Apply and List Entity Framework Core Migrations

Navigate to the solution root (where the `.sln` file is located), and use the following commands:

- **Apply Migrations:**  
  This command will create the database and apply all migrations from the `Migrations` folder:
  ```sh
  dotnet ef database update --project EntityFrameworkCore.Data --startup-project EntityFrameworkCore.Api
  ```

- **List Available Migrations:**  
  This command will show all migrations that have been added to your project:
  ```sh
  dotnet ef migrations list --project EntityFrameworkCore.Data --startup-project EntityFrameworkCore.Api
  ```

- `--project EntityFrameworkCore.Data` specifies the project containing your `DbContext` and migrations.
- `--startup-project EntityFrameworkCore.Api` specifies the executable project that configures your services and entry point.

> **Tip:** If you need to install the EF Core CLI tools, run:
> ```sh
> dotnet tool install --global dotnet-ef
> ```

### 4. Run the Web Application

Navigate to the `EntityFrameworkCore.Api` project and run:

```sh
cd EntityFrameworkCore.Api
dotnet run
```

This will start the web application.

## Useful Commands

- **View running containers:**
  ```sh
  docker ps
  ```
- **Stop the SQL Server container:**
  ```sh
  docker compose stop
  ```

## Migrations

All migration files are located in [`EntityFrameworkCore.Data/Migrations`](EntityFrameworkCore.Data/Migrations). You can add new migrations from the root folder using:

```sh
dotnet ef migrations add MigrationName --project EntityFrameworkCore.Data --startup-project EntityFrameworkCore.Console
```

## License

This project is for educational purposes.

---

**Author:**  
Toka Thanos
