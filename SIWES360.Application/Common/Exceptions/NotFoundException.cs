namespace SIWES360.Application.Common.Models
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string entity, object key)
            : base($"{entity} with id '{key}' was not found.")
        {
        }
    }
}