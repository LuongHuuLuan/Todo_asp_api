
<h1  align="left">Welcome to Todo_asp_api: Learning ASP.net core 8.0 👋</h1>

  

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

  

## Overview

> Todo_asp_api is a project designed to help you understand how to use the C#, ASP.Net core and create a simple API using ASP.net core. This project covers essential CRUD (Create, Read, Update, Delete) functionalities and jwt authentication and authorization through jwt token.

  

## Key Topics

  

*  **Understanding C#, ASP.NET core:** Gain insights into ASP.Net's fundamentals and how it facilitates web api development.

*  **CRUD Functionality:** Implement CRUD operations (Create, Read, Update, Delete) for managing a specific resource (e.g., products).

*  **JWT Authentication:** Understand and implement jwt authentication to secure the API.

  

## Getting Started

This project use the ASP.net core 8.0, Entity framework, MySql (SqlServer or PostgreSQL)

  

## Install

  

**Note:** You need to install [Visual Studio](https://visualstudio.microsoft.com/) and some necessary packages before run project:
* Installation guide in Visual Studio: Go to "Tools > NuGet Package Manager > Manage NuGet Packages for Solution" on the toolbar. In the Browse tab, search for the "package name" and install, for example "Microsoft.EntityFrameworkCore.Tools" and "MySql.EntityFrameworkCore".

* [MySql.EntityFrameworkCore](https://www.nuget.org/packages/MySql.EntityFrameworkCore)
* [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)
* [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL)
* [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools)
* [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)
  

## **Steps**

1. Download the Source Code from GitHub:

```sh

git  clone  https://github.com/LuongHuuLuan/Todo_asp_api.git

```

Once this command is complete, you will have a directory named Todo_asp_api containing the project's source code open it in Visual Studio code.

  

2. Migrate and update database:

  

In this project use database provider. Prioritized MySql provider
You just need to run the command in "Tools > NuGet Package Manager > Package Manage Console" to update the database:

```sh

Add-Migration InitialMigration --Context TodoDBContext
Update-Database --Context TodoDBContext

```

If you want to use PostgreSQL or SQL Server
Please uncomment the code corresponding to the database you use in program.cs and change the contexts in the controller to the corresponding provider. re run Migrate and update database with context name provider

```sh

//var connectionStringSqlServer = builder.Configuration.GetConnectionString("SqlServerConnection");
//builder.Services.AddDbContext<TodoDBContextSqlServer>(options =>
//{
//    options.UseSqlServer(connectionStringSqlServer);
//});

//var connectionStringPostgreSql = builder.Configuration.GetConnectionString("PostgreSqlConnection");
//builder.Services.AddDbContext<TodoDBContextPostgreSQL>(options =>
//{
//    options.UseNpgsql(connectionStringPostgreSql);
//});

// Change all context in controllers
  private readonly TodoDBContext _context;
//private readonly TodoDBContextPostgreSQL _postgreContext;
//private readonly TodoDBContextSqlServer _sqlServerContext;

```
Done. Click run project.
Parent/id endpoints is authorize api

## Author

  

👤 **Luong Huu Luan**

  

* Github: [@LuongHuuLuan](https://github.com/LuongHuuLuan)

  

## Show your support

  

Give a ⭐️ if this project helped you!

  

***

_This README was generated with ❤️ by [readme-md-generator](https://github.com/kefranabg/readme-md-generator)_