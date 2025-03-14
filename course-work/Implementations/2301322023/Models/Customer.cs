using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
