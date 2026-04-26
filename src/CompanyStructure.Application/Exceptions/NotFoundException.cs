namespace CompanyStructure.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string entity, Guid id)
            : base($"{entity} with identifier '{id}' not found.")
        {
        }
    }
}
