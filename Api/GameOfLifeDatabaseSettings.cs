namespace GameOfLife.Api;

public class GameOfLifeDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string GamesCollectionName { get; set; } = null!;
}