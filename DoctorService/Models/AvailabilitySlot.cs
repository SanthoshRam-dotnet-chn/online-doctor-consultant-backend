using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorService.Models
{
    public class AvailabilitySlot
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string Status { get; set; } = "Not Available";
    }
}
