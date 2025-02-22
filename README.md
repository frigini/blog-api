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

## Token Generation
The API provides a simple way to generate a JWT token.

## Generating a Token
To obtain a JWT, send a GET request to:

```http
GET /api/v1/auth/token
```

Response
```json
{
  "token": "your-jwt-token"
}
```

## Using the Token
Once you receive the token, include it in the Authorization header as a Bearer token in all API requests:

```http
GET /api/posts
Authorization: Bearer your-jwt-token
```

## Swagger Authentication
To test authenticated endpoints via Swagger:

1. Open http://localhost:8080/swagger.
2. Click Authorize (lock icon).
3. Enter your token as:

```nginx
Bearer your-jwt-token
```

4. Click Authorize and close the modal.

Now, you can send authenticated requests through Swagger.

## API Endpoints

- `GET /api/v1/posts`
- `POST /api/v1/posts`
- `GET /api/v1/posts/{id}`
- `POST /api/v1/posts/{id}/comments`