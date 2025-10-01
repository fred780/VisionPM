# VisionPM: To-Do Task Management App

## Overview

VisionPM is a minimalist and scalable To-Do task management application, featuring a React frontend and a .NET Core backend. The project demonstrates clean architecture best practices, API design, data structure, and layer communication.

## Technologies

- **Frontend:** React + Vite
- **Backend:** .NET Core Web API
- **Database:** EF Core In-Memory (easy to switch to SQLite)
- **Communication:** REST API (JSON)

## Project Structure

- `/backend`: .NET Core API with EF Core In-Memory
- `/frontend`: React frontend with Vite

## Setup Instructions

### Backend (.NET Core)
1. Requirements: .NET 6+ SDK
2. Install dependencies:
	```sh
	dotnet restore
	```
3. Run the API:
	```sh
	dotnet run --project VisionPM.Api
	```
	The API will be available at `http://localhost:5000`
	Swagger UI: `http://localhost:5000/swagger`
4. **Database:**
	- By default, uses EF Core InMemory.
	- To use SQLite, update `appsettings.Development.json` and configure the connection string.

### Frontend (React)
1. Go to the frontend folder:
	```sh
	cd frontend
	```
2. Install dependencies:
	```sh
	npm install
	```
3. Start the app:
	```sh
	npm run dev
	```
	Open `http://localhost:5173` in your browser.
4. The app expects the API at `http://localhost:5000`

## Features

- Create and list tasks
- Clean architecture (API: Controllers, Application, Domain, Infrastructure)
- EF Core with In-Memory or SQLite database
- Minimalist React frontend
- CORS enabled for frontend-backend communication

## Architecture & Design

- **Backend:**
  - Follows Clean Architecture: `Api` (Controllers), `Application` (Interfaces), `Domain` (Entities/DTOs), `Infrastructure` (EF Core)
  - Uses records for DTOs for simplicity and immutability
  - CORS enabled for frontend communication
- **Frontend:**
  - Ultra-minimalist React SPA
  - Only two endpoints used: list and create tasks

## Assumptions & Trade-offs

- **Persistence:** EF Core In-Memory is used for simplicity and testing. For production, switch to SQLite or SQL Server.
- **Authentication:** Not implemented, but the architecture allows easy integration of OAuth.
- **Scalability:** Frontend and backend are decoupled, allowing independent scaling.
- **Validation:** Basic backend validation; can be extended as needed.
- **MVP Features:** Create, list, update, and delete tasks. Future improvements may include priority, due dates, and user assignment.

## Future Improvements

- Further modularize the codebase for better maintainability
- Persistent storage with SQLite or SQL Server
- Authentication and authorization (JWT)
- Unit and integration tests
- User and role management
- Enhanced UI (filters, search, pagination)
- Cloud deployment (Azure/AWS)

## Notes

- Code is commented for clarity.
- Architecture follows separation of concerns and scalability principles.
- Frontend consumes backend REST API via fetch.
