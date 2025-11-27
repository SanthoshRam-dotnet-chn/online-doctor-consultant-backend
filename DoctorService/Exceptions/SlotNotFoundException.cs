namespace DoctorService.Exceptions
{
    public class SlotNotFoundException : Exception
    {
        public SlotNotFoundException(string message) : base(message) { }
    }
}
