using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReservation.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public DateTime ReservationDateTime { get; set; }

        public int NumberOfGuests { get; set; }

        [MaxLength(250)]
        public string SpecialRequests { get; set; }
    }
}
