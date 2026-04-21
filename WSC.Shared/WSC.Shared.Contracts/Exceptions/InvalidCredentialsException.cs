namespace WSC.Shared.Contracts.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid credentials. Check EmailId or Password!")
        {
        }

        public InvalidCredentialsException(string message) : base(message)
        {
        }
    }
}
