namespace WSC.Shared.Contracts.Exceptions
{
    public class InActiveException : Exception
    {
        public InActiveException(string entity, object key) : base($"{entity} with Id {key} is InActive")
        { }
    }
}
