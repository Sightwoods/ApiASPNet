﻿using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Services;

public class PagosService
{
    private readonly IMongoCollection<Pagos> _pagosCollection;
    public PagosService(IOptions<SistemasDistribuidosDatabaseSettings> sistemasDistribuidosDatabaseSettings)
    {
        var mongoClient = new MongoClient(
        sistemasDistribuidosDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
        sistemasDistribuidosDatabaseSettings.Value.DatabaseName);

        _pagosCollection = mongoDatabase.GetCollection<Pagos>(
       sistemasDistribuidosDatabaseSettings.Value.PagosCollectionName);
    }
    public async Task<List<Pagos>> GetAsync() =>
           await _pagosCollection.Find(_ => true).ToListAsync();
    public async Task<Pagos?> GetAsync(string matricula) =>
        await _pagosCollection.Find(x => x.NumeroMatricula == matricula).FirstOrDefaultAsync();
    
    public async Task CreateAsync(Pagos newPago) =>
        await _pagosCollection.InsertOneAsync(newPago);
    
    public async Task UpdateAsync(string matricula, Pagos updatedPago) =>
        await _pagosCollection.ReplaceOneAsync(x => x.NumeroMatricula == matricula, updatedPago);

    public async Task RemoveAsync(string matricula) =>
        await _pagosCollection.DeleteOneAsync(x => x.NumeroMatricula == matricula);
}