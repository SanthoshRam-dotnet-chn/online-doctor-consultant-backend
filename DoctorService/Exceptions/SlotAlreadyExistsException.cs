namespace DoctorService.Exceptions
{
    public class SlotAlreadyExistsException : Exception
    {
        public SlotAlreadyExistsException(string message) : base(message) { }
    }
}
