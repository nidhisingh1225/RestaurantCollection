using RestaurantCollection.WebApi.DTO.Forms;
using RestaurantCollection.WebApi.Models;

namespace RestaurantCollection.WebApi.Utility
{
    public static class RestaurantMapper
    {
        public static Restaurant MapToBusinessRestaurant(CreateForm request)
        {
            return new Restaurant
            {
                AverageRating = request.Rating,
                City = request.City,
                EstimatedCost = request.Cost,
                Name = request.Name,
                Votes = request.Votes,
            };
        }
    }
}
