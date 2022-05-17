namespace RestaurantCollection.WebApi.DTO
{
    public class RestaurantSearchResult
    {
        public string Rating { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Votes { get; set; }
    }
}
