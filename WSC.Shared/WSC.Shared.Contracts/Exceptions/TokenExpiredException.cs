namespace WSC.Shared.Contracts.Exceptions
{
    public class TokenExpiredException : Exception
    {
        public TokenExpiredException() : base("Token has expired. Please Login Again!")
        {
        }
        public TokenExpiredException(string message) : base(message)
        {
        }
    }
}
