using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantOpeningApi.Models
{
    public class RestaurantTime
    {
        [Key]     
        public string Id { get; set; }


        [Required]
        [MaxLength(15)]
        [Display(Name = "Opening Day")]
        public string OpeningDay { get; set; }

        [Required]
        [Display(Name = "Opening Time")]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }

        //Foreign key referencing the Restaurant table as parent
        public string RestaurantId { get; set; }

        //Navigation property for Restaurant
        public Restaurant Restaurant { get; set; }
    }
}
