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
   dotnet run [--launch-profile https]
   ```
   The API will be available at `http://localhost:5181` (see launchSettings.json).

4. **Run tests:**
   ```sh
   cd Tests
   dotnet test
   ```

## API Functional Requirements:

### Endpoints
1. Upload Board State
  - Accept a new board state (2D grid of cells).
  - Return a unique identifier for the stored board.
2. Get Next State
  - Given a board ID, return the next generation state of the board.
3. Get N States Ahead
  - Given a board ID and a number N, return the board state after N generations.
4. Get Final State
  - Return the final stable state of the board (i.e., when it no longer changes or cycles).
  - If the board does not reach a stable conclusion within a reasonable number of iterations, return a suitable error message.


OpenApi specification can be viewed here: `http://localhost:5181/openapi/v1.json`
- Example requests can be made using tools like Postman or cURL.
See Api.http for example requests.

## Technologies Used

- ASP.NET Core Web API
- .NET 8.0/9.0
- MongoDB (via MongoDB.Driver)
- MSTest (for unit testing)

## Design Patterns Used:
This solution employs several design patterns and architectural principles to ensure maintainability, scalability, and testability:
### 1. Domain-Driven Design (DDD)
  Domain Model: The Game project contains the core business logic with Board, State, and CellPosition classes
  Value Objects: CellPosition represents a coordinate pair as an immutable value
  Entities: Board and State represent the core business entities
### 2. Repository Pattern
  GameService acts as a repository for GameModel entities
  Abstracts data access through MongoDB operations (GetAsync, CreateAsync)
  Provides a clean interface for data persistence
### 3. Service Layer Pattern
  GameService encapsulates business logic and data access
  Acts as an intermediary between the controller and data layer
  Handles MongoDB operations and data transformation
### 4. MVC Pattern (Model-View-Controller)
  Controller: GameController handles HTTP requests and responses
  Model: GameModel represents the data structure for API operations
  View: JSON responses serve as the view layer
### 5. Dependency Injection Pattern
  Services are registered in Program.cs using the built-in DI container
  GameService is injected into GameController
  Configuration is injected using IOptions<GameOfLifeDatabaseSettings>
### 6. Factory Method Pattern
  Static methods like ToMatrix() and ToSparseMatrix() in Board class
  Convert between different data representations (jagged arrays to matrices)
### 7. State Pattern
  The State class manages the game state transitions
  Next() method implements the Conway's Game of Life rules
  State changes are immutable (returns new State instances)

## Architectural Patterns:
This solution follows several architectural patterns to ensure a clean separation of concerns and maintainability:
### 1. Layered Architecture
  Presentation Layer: API controllers and HTTP handling
  Business Logic Layer: Game domain model and services
  Data Access Layer: MongoDB operations and data models
### 2. Clean Architecture Principles
  Separation of concerns between Game domain and API infrastructure
  Domain logic is independent of external dependencies
  Clear boundaries between layers
### 3. Configuration Pattern
  GameOfLifeDatabaseSettings class for external configuration
  Database connection strings and collection names are configurable
  Uses IOptions<T> pattern for strongly-typed configuration

## Data Patterns:
This solution employs several data patterns to efficiently represent and manipulate the Game of Life state:
### 1. Sparse Matrix Representation
  Efficient storage of only non-zero (live) cells
  CellPosition list for memory-efficient game state storage
  Conversion utilities between dense and sparse representations
### 2. Immutable State Transitions
  Game state changes create new State instances
  Prevents accidental state mutations
  Enables easy state comparison and cycle detection

## Testing Patterns:
This solution employs unit testing patterns to ensure the correctness of the Game of Life logic:
### 1. Unit Testing with MSTest
  Test-driven development approach
  Tests for specific game scenarios (empty state, block pattern, blinker)
  Isolated testing of domain logic
### 2. Test Data Patterns
  Well-known Conway's Game of Life patterns for testing
  Edge cases like empty boards and oscillators

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
