
Create the Web API project
--------------------------------
dotnet new webapi -n MyApi

cd MyApi

Add EF Core + Npgsql packages
------------------------------------------
dotnet add package Microsoft.EntityFrameworkCore.Design,
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL,
dotnet add package EFCore.NamingConventions

Error:
----------------
Error (active) CS0234 The type or namespace name 'EntityFrameworkCore', 'DbContext' , 'DbSet' does not exist in the namespace 'Microsoft' (are you missing an assembly reference?)

use net8.0
----------------------------------------------
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.8" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.8">
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.4" />
    <PackageReference Include="EFCore.NamingConventions" Version="8.0.3" />
  </ItemGroup>
</Project>

Restore packages
--------------------------
dotnet restore

Fix your DbContext using statements
--------------------------------------
using Microsoft.EntityFrameworkCore;

namespace MyApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}

Clean & rebuild (important)
----------------------------------
dotnet clean,
dotnet build


Error
--------------------
AddSwaggerGen is not part of ASP.NET Core by default — it comes from Swagger / OpenAPI packages, and they’re simply not installed.

Add Swagger package
---------------------------------------------
dotnet add package Swashbuckle.AspNetCore --version 6.6.2


dotnet tool install --global dotnet-ef