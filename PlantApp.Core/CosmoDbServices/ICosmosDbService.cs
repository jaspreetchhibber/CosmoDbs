using PlantApp.Core.Photos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Core.CosmoDbServices
{
    public interface ICosmosDbService
    {
        Task<string> AddItemAsync(Photo item);
        Task DeleteItemAsync(string id);
        Task<Photo> GetItemAsync(string id);
        Task<IEnumerable<Photo>> GetItemsAsync(string queryString);
    }
}
