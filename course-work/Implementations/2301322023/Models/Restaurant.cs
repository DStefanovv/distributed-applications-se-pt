using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public double Rating { get; set; }

        public DateTime OpeningTime { get; set; }
    }
}
