using System;
using System.Collections.Generic;

namespace DoctorService.Models;

public partial class Prescription
{
    public Guid PrescriptionId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid PatientId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime Date { get; set; }

    public Guid AppointmentId { get; set; }
}
