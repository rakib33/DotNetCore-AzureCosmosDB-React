using System.ComponentModel.DataAnnotations;

namespace RestaurantOpeningApi.DTOs
{
    public class RestaurantTimeDTO
    {
        public string OpeningDay { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
    }
}
