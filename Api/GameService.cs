using GameOfLife.Game;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
// using Newtonsoft.Json;
using System.Text.Json;

namespace GameOfLife.Api;

public class GameService
{
  private readonly IMongoCollection<GameModel> _gamesCollection;

  public GameService(
      IOptions<GameOfLifeDatabaseSettings> gameOfLifeDatabaseSettings)
  {
    var mongoClient = new MongoClient(
        gameOfLifeDatabaseSettings.Value.ConnectionString);

    var mongoDatabase = mongoClient.GetDatabase(
        gameOfLifeDatabaseSettings.Value.DatabaseName);

    _gamesCollection = mongoDatabase.GetCollection<GameModel>(
        gameOfLifeDatabaseSettings.Value.GamesCollectionName);
  }

  public List<GameModel> GetGames()
  {
    var test = _gamesCollection.Find(_ => true).ToList();
    return test;
  }

  public GameModel GetGameById(string id)
  {
    var test = _gamesCollection.Find(x => x.Id == id).FirstOrDefault();
    return test;
  }

  public async Task<GameModel> GetAsync(string id)
  {
    var test = await _gamesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    return test;
  }

  public async Task CreateAsync(GameModel newGame) =>
      await _gamesCollection.InsertOneAsync(newGame);

  // public void Create(Board newBoard) => _boardsCollection.InsertOne(newBoard);

  // public async Task UpdateAsync(string id, Board updatedBoard) =>
  //     await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBoard);

  // public async Task RemoveAsync(string id) =>
  //     await _booksCollection.DeleteOneAsync(x => x.Id == id);
}