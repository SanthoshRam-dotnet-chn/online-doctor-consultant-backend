using DoctorService.Models;

namespace DoctorService.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<Prescription> GetPrescriptionByIdAsync(Guid prescriptionId);
        Task AddPrescriptionAsync(Prescription prescription);
    }
}
