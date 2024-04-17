using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantOpeningApi.Models
{
    public class RestaurantTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [MaxLength(15)]
        [Display(Name = "Opening Day")]
        public string OpeningDay { get; set; }

        [Required]
        [Display(Name = "Opening Time")]
        public TimeOnly OpeningTime { get; set; }

        [Required]
        [Display(Name = "Closing Time")]
        public TimeOnly ClosingTime { get; set; }
    }
}
