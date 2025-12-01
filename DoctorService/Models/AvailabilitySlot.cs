using System;
using System.Collections.Generic;

namespace DoctorService.Models;

public partial class AvailabilitySlot
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsAvailable { get; set; }
}
