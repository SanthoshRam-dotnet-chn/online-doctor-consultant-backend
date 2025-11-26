namespace AuthService.src.AuthService.Application.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException()
            : base("Email already exists.") { }


        public EmailAlreadyExistsException(string message)
            : base(message) { }
    }
}
