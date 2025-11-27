using DoctorService.Models;

namespace DoctorService.Interfaces
{
    public interface IPrescriptionService
    {
        Task<Prescription> GetPrescriptionByIdAsync(Guid prescriptionId);
        Task<Prescription> CreatePrescriptionAsync(Prescription prescription);
    }
}
