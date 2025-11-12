# BasicBlog.Core

A simple blog application built with ASP.NET Core MVC, featuring user authentication, blog posts, and comments.

## Features

- User registration and login
- Create, edit, and view blog posts
- Comment on blog posts
- User management

## Prerequisites

- .NET 8.0 SDK
- SQL Server LocalDB (comes with Visual Studio or can be installed separately)


## Local Setup Guide

1. Clone the repository:
   ```
   git clone <repository-url>
   cd BasicBlog.Core
   ```

2. Restore dependencies:
   ```
   dotnet restore
   ```

3. Build the project:
   ```
   dotnet build
   ```

4. Run the application:
   ```
   dotnet run
   ```

5. Open your browser and navigate to `http://localhost:5196` to access the application.

The application will automatically apply database migrations and seed initial data on first run.
