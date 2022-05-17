using Newtonsoft.Json;
using RestaurantCollection.WebApi.DataAccess;
using RestaurantCollection.WebApi.DTO;
using RestaurantCollection.WebApi.DTO.Forms;
using RestaurantCollection.WebApi.Models;
using RestaurantCollection.WebApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RestaurantCollection.WebApi.Manager
{
    public class RestaurantManager: IRestaurantManager
    {
        private readonly IRepository _repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantManager" /> class.
        /// </summary>
        /// <param name="repository"></param>
        public RestaurantManager(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<RestaurantResponse> CreateAsync(CreateForm request)
        {
            try
            {
                //TODO: automapper should be used here
                var restaurant = RestaurantMapper.MapToBusinessRestaurant(request);
                var result = await _repository.AddRestaurant(restaurant);
                if(result == null)
                {
                    return new RestaurantResponse { Status = RestaurantStatusEnum.BadRequest};
                }

                return new RestaurantResponse
                {
                    Status = RestaurantStatusEnum.Created,
                    Restaurant = result
                };
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error while creating restaurant: {ex.Message}.";
                return new RestaurantResponse { Status = RestaurantStatusEnum.Error, ErrorMessage = errorMessage };
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets the email invalid.
        /// </summary>
        /// <param name="id">restaurant id.</param>
        public void UpdateRestaurant(int id)
        {
            var restaurant = _repository.GetRestaurants().ConfigureAwait(false).GetAwaiter().GetResult()
                .Where(x => x.Id == id).SingleOrDefault();
            if (restaurant != null)
            {
                _repository.UpdateRestaurant(restaurant);
            }
        }

        /// <summary>
        /// Gets all restaurants.
        /// </summary>
        public async Task<List<RestaurantSearchResult>> GetAllRestaurantAsync()
        {
            var restaurantSearchResult = new List<RestaurantSearchResult>();
            var restaurants = await _repository.GetRestaurants(); 
            foreach (var restaurant in restaurants)
            {
                restaurantSearchResult.Add(
                    new RestaurantSearchResult
                    {
                        Rating = restaurant.AverageRating,
                        City = restaurant.City,
                        Cost = restaurant.EstimatedCost,
                        Name = restaurant.Name,
                        Votes = restaurant.Votes,
                    });
            }
            return restaurantSearchResult;
        }

        /// <summary>
        /// Search restaurant.
        /// </summary>
        public async Task<List<RestaurantSearchResult>> SearchAsync(string city, int? id)
        {
            var restaurantSearchResult = new List<RestaurantSearchResult>();

            var searchParam = new RestaurantQueryModel { City = city, Id = id.GetValueOrDefault() };

            var restaurants = await _repository.GetRestaurants(searchParam);
            foreach (var restaurant in restaurants)
            {
                restaurantSearchResult.Add(
                    new RestaurantSearchResult
                    {
                        Rating = restaurant.AverageRating,
                        City = restaurant.City,
                        Cost = restaurant.EstimatedCost,
                        Name = restaurant.Name,
                        Votes = restaurant.Votes,
                    });
            }
            return restaurantSearchResult;
        }

        /// <summary>
        /// Delete restaurant.
        /// </summary>
        public bool Delete(int id)
        {
            var restaurant = _repository.GetRestaurants().ConfigureAwait(false).GetAwaiter().GetResult()
                .Where(x => x.Id == id).SingleOrDefault();
            if (restaurant == null)
            {
                return false;
            }                
            _repository.DeleteRestaurant(restaurant);
            return true;
        }

        /// <summary>
        /// Search restaurant.
        /// </summary>
        public async Task<List<RestaurantSearchResult>> SortAsync()
        {
            var restaurantSearchResult = new List<RestaurantSearchResult>();

            var restaurants = await _repository.GetRestaurantsSorted();
            foreach (var restaurant in restaurants)
            {
                restaurantSearchResult.Add(
                    new RestaurantSearchResult
                    {
                        Rating = restaurant.AverageRating,
                        City = restaurant.City,
                        Cost = restaurant.EstimatedCost,
                        Name = restaurant.Name,
                        Votes = restaurant.Votes,
                    });
            }
            return restaurantSearchResult;
        }
    }
}
