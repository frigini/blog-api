# Blog API

A RESTful API for managing a simple blogging platform built with .NET 8, following clean architecture principles and best practices.

## Features

- Create and retrieve blog posts
- Add comments to blog posts
- Clean Architecture implementation
- Docker support
- PostgreSQL database
- Swagger documentation

## Prerequisites

- Docker and Docker Compose
- .NET 8 SDK (for local development)
- Make (optional, for using Makefile commands)

## Getting Started

1. Clone the repository:
```bash
git clone <repository-url>
cd blog-api
```

2. Build the application:
```bash
make build
```

3. Run the application:
```bash
make run
```

The API will be available at `http://localhost:8080` and Swagger documentation at `http://localhost:8080/swagger`.

## API Endpoints

- `GET /api/posts`