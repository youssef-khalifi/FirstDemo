using SynchApp.Dtos;
using SynchApp.Models;

namespace SynchApp.Services.Interfaces;

public interface ICollectionService
{
    Task<IEnumerable<Collection>> GetAllCollectionsAsync();
    Task<Collection> GetCollectionByIdAsync(int id);
    Task<Collection> CreateCollectionAsync(CreateCollectionDto collectionDto);
    Task<Collection> AddRequestToCollection(CreateRequestDto requestDto);
    Task<bool> DeleteCollectionAsync(int id);
}