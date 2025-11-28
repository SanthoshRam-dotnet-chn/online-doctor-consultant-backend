using DoctorService.Exceptions;
using DoctorService.Interfaces;
using DoctorService.Models;

namespace DoctorService.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;

        public PrescriptionService(IPrescriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Prescription> CreatePrescriptionAsync(Prescription prescription)
        {
            prescription.PrescriptionId = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(prescription.Description))
                throw new BadRequestException("Description is required.");

            if (prescription.Description.Length > 500)
                throw new BadRequestException("Description cannot exceed 500 characters.");

            await _repository.AddPrescriptionAsync(prescription);
            return prescription;
        }

        public async Task<Prescription> GetPrescriptionByIdAsync(Guid prescriptionId)
        {
            var prescription = await _repository.GetPrescriptionByIdAsync(prescriptionId);
            if (prescription == null)
                throw new PrescriptionNotFoundException("Prescription not found.");
            return prescription;
        }
    }
}
