namespace SIWES360.Application.Common.Models
{
    public class Error
    {
        public string Code { get; }
        public string Message { get; }
        public int? StatusCode { get; }
        public string? Details { get; }

        private Error(string code, string message, int? statusCode = null, string? details = null)
        {
            Code = code;
            Message = message;
            StatusCode = statusCode;
            Details = details;
        }

        // Factory methods for consistency
        public static Error Validation(string code, string message, string? details = null)
            => new(code, message, 400, details);

        public static Error NotFound(string entity, string id)
            => new("NotFound", $"{entity} with Id {id} was not found.", 404);

        public static Error Conflict(string message)
            => new("Conflict", message, 409);

        public static Error Failure(string code, string message, int? statusCode = 500, string? details = null)
            => new(code, message, statusCode, details);

        public override string ToString() => $"{Code}: {Message}";
    }
}