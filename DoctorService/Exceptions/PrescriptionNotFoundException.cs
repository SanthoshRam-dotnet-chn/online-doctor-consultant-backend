namespace DoctorService.Exceptions
{
    public class PrescriptionNotFoundException : Exception
    {
        public PrescriptionNotFoundException(string message) : base(message) { }
    }
}
