
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

MyApi>dotnet tool run dotnet-ef database update --context AppDbContext

Add file docker-compose.yml
MyApi> docker compose up -d

Make sure the docker desktop is running.

MyApi> docker compose up -d
[+] Running 16/16
 ✔ postgres Pulled                                                                                                14.5s
   ✔ a77625ddab57 Pull complete                                                                                    1.0s
   ✔ a2f4fb31fafd Pull complete                                                                                    6.7s
   ✔ d6ba2dba483d Pull complete                                                                                    0.8s
   ✔ e65abb0dfda2 Pull complete                                                                                    8.8s
   ✔ 27cce642b31e Pull complete                                                                                    1.3s
   ✔ 52e9beb5a887 Pull complete                                                                                    1.3s
   ✔ 4b9c761c0414 Pull complete                                                                                    1.7s
   ✔ e46c85978ff6 Pull complete                                                                                    8.9s
   ✔ 9d9219b46c76 Pull complete                                                                                    1.4s
   ✔ bd6fc428af63 Pull complete                                                                                    1.4s
   ✔ 0b5d5a42833c Pull complete                                                                                    5.9s
   ✔ 6731da0a1dbc Pull complete                                                                                    5.9s
   ✔ ebdc127e517b Pull complete                                                                                    0.8s
   ✔ 442d2119ce56 Download complete                                                                                0.0s
   ✔ e779d5b62af1 Download complete                                                                                0.5s
[+] Running 3/3
 ✔ Network myapi_default     Created                                                                               0.1s
 ✔ Volume myapi_pgdata       Created                                                                               0.0s
 ✔ Container myapi-postgres  Started                                                                               1.0s


MyApi>dotnet tool run dotnet-ef migrations add InitialCreate2 --context AppDbContext
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'

MyApi>dotnet tool run dotnet-ef database update --context AppDbContext
Build started...
Build succeeded.
Applying migration '20251225143747_InitialCreate2'.
Done.



