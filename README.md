# Conway Game of Life API

This project implements Conway's Game of Life as a .NET solution with a clean separation between the game logic and a RESTful API.

## Project Structure

- **Api/**  
  ASP.NET Core Web API project exposing endpoints to interact with the Game of Life.  
  - `GameController.cs` (or similar): Handles HTTP requests.
  - `GameService.cs`: Business logic for managing game state.
  - `GameModel.cs`, `CellPosition.cs`: Data models.
  - `GameOfLifeDatabaseSettings.cs`: Database configuration.
  - `appsettings.json`: Application configuration.
  - `Api.csproj`: Project file with dependencies (e.g., MongoDB.Driver, MathNet.Numerics, Newtonsoft.Json).

- **Game/**  
  .NET class library containing the core Game of Life logic.  
  - `Board.cs`: Represents the game board.
  - `State.cs`: Represents the state of the game.
  - `CellPosition.cs`: Represents a cell's position.
  - `Game.csproj`: Project file.

- **Tests/**  
  Unit tests for the Game library using MSTest.  
  - `GameTests.cs`, `StateTests.cs`: Test cases for game logic.
  - `Tests.csproj`: Project file.

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [MongoDB](https://www.mongodb.com/) (if using database features)

### Build and Run

1. **Restore dependencies:**
   ```sh
   dotnet restore
   ```

2. **Build the solution:**
   ```sh
   dotnet build
   ```

3. **Run the API:**
   ```sh
   cd Api
   dotnet run
   ```
   The API will be available at `http://localhost:5181` (see launchSettings.json).

4. **Run tests:**
   ```sh
   cd Tests
   dotnet test
   ```

## API Overview

The API exposes endpoints to:

- Start a new game
- Get the current state of the board
- Advance to the next generation
- Set or clear cells
- Retrieve game history

See Api.http for example requests.

## Technologies Used

- ASP.NET Core Web API
- .NET 8.0/9.0
- MongoDB (via MongoDB.Driver)
- MathNet.Numerics
- Newtonsoft.Json
- MSTest (for unit testing)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Open a pull request

## License

MIT License

---

*This project is for educational and demonstration purposes.*
```
