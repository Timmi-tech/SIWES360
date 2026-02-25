namespace SIWES360.Application.Common.Models
{
    public sealed class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation failures occurred.")
        {
            Errors = errors;
        }
    }
}