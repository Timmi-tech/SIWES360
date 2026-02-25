namespace SIWES360.Application.Common.Models
{
    public sealed class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException()
            : base("Access to this resource is forbidden.")
        {
        }
    }
}