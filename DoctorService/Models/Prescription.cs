using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace DoctorService.Models
{
    public class Prescription
    {

        [Key]
        public Guid PrescriptionId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Guid AppointmentId { get; set; }
    }
}
