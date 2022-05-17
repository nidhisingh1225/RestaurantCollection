//using RestaurantCollection.WebApi.DTO.Common;
using RestaurantCollection.WebApi.DTO.Forms;
using RestaurantCollection.WebApi.Manager;
//using RestaurantCollection.WebApi.DTO.ViewModels;
using RestaurantCollection.WebApi.Models;
using RestaurantCollection.WebApi.Utility;
using ServiceStack;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace RestaurantCollection.WebApi.Controllers
{
    [RoutePrefix("api/")]
    public class RestaurantController : ApiController
    {
        private readonly IRestaurantManager _manager;
        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantController" /> class.
        /// </summary>
        /// <param name="manager"></param>
        public RestaurantController(IRestaurantManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Create new restaurant 
        /// </summary>
        /// <returns></returns>
        [HttpPost,Route("restaurant")]
        public async Task<IHttpActionResult> PostAsync(CreateForm request)
        {
            if(request == null)
            {
                return BadRequest(GenericErrorCode.RequestBodyEmpty.ToDescription());
            }              

            var response = await _manager.CreateAsync(request);
            switch (response.Status)
            {
                case RestaurantStatusEnum.Created:
                    return Created(response.Restaurant.Id.ToString(), response.Restaurant);
                case RestaurantStatusEnum.BadRequest:
                    return BadRequest(response.ErrorMessage ?? "Bad request");
                default:
                    return InternalServerError();
            }
        }

        /// <summary>
        /// Updates restaurant that have specified id.
        /// </summary>
        /// <param name="id">restaurant id</param>
        /// <param name="request">update request</param>
        /// <returns></returns>
        [HttpPut, Route("restaurant/{id}")]
        public IHttpActionResult PutAsync([FromUri] string id, [FromBody] UpdateForm request)
        {
            if (request == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();
            
            _manager.UpdateRestaurant(id.ToInt());

            return Ok();
        }

        /// <summary>
        /// Get all restaurants.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("restaurant")]
        public HttpResponseMessage Get()
        {
           var result =  _manager.GetAllRestaurantAsync();

           return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Search restaurant.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("restaurant/query/{id?}")]
        [Route("restaurant/query/{city?}")]
        public HttpResponseMessage Search([FromUri] string city, [FromUri] int? id)
        {
            var result = _manager.SearchAsync(city,id);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Delete restaurant.
        /// </summary>
        /// <returns></returns>
        [HttpDelete, Route("restaurant/{id}")]
        public HttpResponseMessage DeleteAsync([FromUri] int id)
        {
            var result = _manager.Delete(id);
            if (!result)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, result);

            }
            return Request.CreateResponse(HttpStatusCode.NoContent, result);
        }

        /// <summary>
        /// Sort restaurant- by default rating.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("restaurant/sort")]
        public HttpResponseMessage SortAsync()
        {
            var result = _manager.SortAsync();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
