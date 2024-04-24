using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantOpeningApi.Models
{
    public class Restaurant
    {
        [Key]      
        public string Id { get; set; }


        [Required]
        [MaxLength(500)]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; }
        public string OperatingTime { get; set; }

        // Navigation property for RestaurantTime
        public List<RestaurantTime> restaurantTimes { get; set; } = new List<RestaurantTime>();
    }
}
