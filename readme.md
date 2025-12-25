
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

error:
-------------
MyApi>dotnet tool install --global dotnet-ef
Package Source Mapping is enabled, but no source found under the specified package ID: dotnet-ef. See the documentation for Package Source Mapping at https://aka.ms/nuget-package-source-mapping for more details.

MyApi>dotnet new tool-manifest --force
MyApi>dotnet tool install dotnet-ef --version 8.0.8 --add-source https://api.nuget.org/v3/index.json

You can invoke the tool from this directory using the following commands: 'dotnet tool run dotnet-ef' or 'dotnet dotnet-ef'.
Tool 'dotnet-ef' (version '8.0.8') was successfully installed. Entry is added to the manifest file D:\Develop\Dotnet\MyApi\.config\dotnet-tools.json.

MyApi>dotnet tool run dotnet-ef --version
Entity Framework Core .NET Command-line Tools
8.0.8

AppDbContext.cs
-----------------------
using Microsoft.EntityFrameworkCore;

namespace MyApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}

Program.cs
-----------------
using Microsoft.EntityFrameworkCore;
using MyApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();
app.MapControllers();
app.Run();

appsettings.json
---------------------------
{
  "ConnectionStrings": {
    "Default": "Host=Local;Port=5432;Database=db;Username=username;Password=password"
  }
}

dotnet build

Run migrations with explicit project flags (very common fix)
---------------------------------------------------------------------------
dotnet tool run dotnet-ef migrations add InitialCreate --project MyApi.csproj --startup-project MyApi.csproj --context AppDbContext


MyApi>dotnet tool run dotnet-ef migrations add InitialCreate --context AppDbContext
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'

