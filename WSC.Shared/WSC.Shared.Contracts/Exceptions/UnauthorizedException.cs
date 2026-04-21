namespace WSC.Shared.Contracts.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized access.")
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
