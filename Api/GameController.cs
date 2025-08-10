using Microsoft.AspNetCore.Mvc;
using Game;

namespace Api;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly GameService _gameService;

    public GameController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id, [FromQuery] int? n)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Game ID is required.");

        var game = await _gameService.GetAsync(id);
        if (game is null)
            return NotFound();

        int[][] denseMatrix;
        try
        {
            denseMatrix = GameModel.ToDenseMatrix(game.CurrentState.Select(c => new int[] { c.Row, c.Col }).ToArray());
        }
        catch (Exception ex)
        {
            return Problem($"Failed to convert game state: {ex.Message}");
        }

        Board board;
        try
        {
            board = new Board(denseMatrix);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid board state: {ex.Message}");
        }

        try
        {
            board.NextGeneration(n ?? 1, true);
        }
        catch (Exception ex)
        {
            return Problem($"Failed to advance generation: {ex.Message}");
        }

        if (n > 1 && !board.IsFinal && !board.HasCycle)
            return StatusCode(500, "Game is not final or has no cycle yet.");

        return Ok(board);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] int[][] initialState)
    {
        if (initialState == null || initialState.Length == 0 || initialState.Any(row => row == null))
            return BadRequest("Initial state must be a non-empty jagged array with no null rows.");

        Board board;
        try
        {
            board = new Board(initialState);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid board state: {ex.Message}");
        }

        var newGame = new GameModel(board);
        await _gameService.CreateAsync(newGame);
        return CreatedAtAction(nameof(Get), new { id = newGame.Id }, newGame);
    }
}