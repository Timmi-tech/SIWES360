namespace SIWES360.Infrastructure.Services
{
    public class EmailConfiguration
    {
        public const string Section = "EmailSettings";
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string FromEmail { get; set; } = default!;
        public string SmtpPassword { get; set; } = default!;
        public string SenderUsername { get; set; } = default!;

        //public int ConfirmationCodeExpiryMinutes { get; set; } = 30;
        //public int PasswordResetTokenExpiryMinutes { get; set; } = 30;
    }
}