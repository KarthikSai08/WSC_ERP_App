namespace WSC.Shared.Contracts.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, object key) : base($"{entity} with Id : {key} was Not Found!!")
        { }
    }
}
