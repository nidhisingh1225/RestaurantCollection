using RestaurantCollection.WebApi.DTO;
using RestaurantCollection.WebApi.DTO.Forms;
using RestaurantCollection.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantCollection.WebApi.Manager
{
    public interface IRestaurantManager
    {
        Task<RestaurantResponse> CreateAsync(CreateForm request);
        void UpdateRestaurant(int id);
        Task<List<RestaurantSearchResult>> GetAllRestaurantAsync();
        Task<List<RestaurantSearchResult>> SearchAsync(string city, int? id);
        bool Delete(int id);
        Task<List<RestaurantSearchResult>> SortAsync();
    }
}
;