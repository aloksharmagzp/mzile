# Delhi Metro Route Finder

A full-stack sample app to find the best Delhi Metro route between two stations.

## Tech Stack
- **Backend:** ASP.NET Core Web API (.NET 8) + Entity Framework Core
- **Database:** SQL Server
- **Frontend:** React (CDN-based single-page app)

## Project Structure
- `backend/DelhiMetroRouteFinder.Api` - .NET 8 Web API project
- `frontend/index.html` - React frontend (no build step)
- `sql/schema.sql` - SQL Server schema script
- `sql/seed.sql` - sample seed data (14 stations, 3 lines)

## Features
- Search route by source + destination station
- Shortest route using Dijkstra (time-based)
- Returns:
  - Best route (station sequence + line colors)
  - Total stations
  - Estimated travel time
  - Fare amount from slab rules
  - Interchange stations and count
  - Distance in km
  - First metro timing (source) and last metro timing (destination)

## API Endpoints
- `GET /api/stations`
- `POST /api/route` with JSON body:
  ```json
  {
    "sourceStationId": 1,
    "destinationStationId": 14
  }
  ```
- `GET /api/fare?stationCount=10`

## Step-by-step Setup

### 1) Create database and seed data
1. Ensure SQL Server is running.
2. Open SQL Server Management Studio (or Azure Data Studio).
3. Run scripts in order:
   - `sql/schema.sql`
   - `sql/seed.sql`

### 2) Configure API connection string
Update `backend/DelhiMetroRouteFinder.Api/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DelhiMetroRouteFinderDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True"
}
```

### 3) Run backend API
```bash
cd backend/DelhiMetroRouteFinder.Api
dotnet restore
dotnet run --urls http://localhost:5000
```

Swagger UI will be available at `http://localhost:5000/swagger` in development mode.

### 4) Run frontend
Use any static file server. Example with Node:
```bash
cd frontend
npx serve -l 5173
```

Open: `http://localhost:5173`

> Frontend expects API at `http://localhost:5000` (see `API_BASE` in `frontend/index.html`).

## Notes
- Sample data includes interchanges between:
  - Rajiv Chowk (Blue ↔ Yellow)
  - Kashmere Gate (Yellow ↔ Violet)
  - Central Secretariat ↔ Mandi House transfer edge
- You can add more stations/connections and fare slabs directly in SQL.
