using Microsoft.AspNetCore.Identity.UI.Services;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Application.Common.Models;

namespace SIWES360.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<Result> SendInviteSupervisorEmailAsync(
            string email,
            string fullName,
            string invitationLink,
            CancellationToken ct)
        {
            var subject = "You have been invited as a SIWES Supervisor";
            var body = $@"
                <html>
                <body style='margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5'>
                    <div style='max-width:600px;margin:40px auto;background-color:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,0.1)'>
                        <div style='background-color:#90EE90;padding:30px;text-align:center'>
                            <h1 style='color:#ffffff;margin:0;font-size:32px;font-weight:bold'>SIWES360</h1>
                        </div>
                        <div style='padding:40px 30px'>
                            <h2 style='color:#333333;margin-top:0'>Supervisor Invitation</h2>
                            <p style='color:#555555;line-height:1.6'>Hello {fullName},</p>
                            <p style='color:#555555;line-height:1.6'>You have been invited to SIWES360 as a Supervisor.</p>
                            <p style='color:#555555;line-height:1.6'>Please set your password using the button below:</p>
                            <div style='text-align:center;margin:30px 0'>
                                <a href='{invitationLink}' style='background-color:#90EE90;color:#ffffff;padding:14px 40px;text-decoration:none;border-radius:5px;font-weight:bold;display:inline-block'>Set Password</a>
                            </div>
                            <p style='color:#888888;font-size:14px;line-height:1.6'>If you did not expect this invitation, you can ignore this email.</p>
                        </div>
                        <div style='background-color:#f9f9f9;padding:20px;text-align:center;border-top:1px solid #eeeeee'>
                            <p style='color:#999999;font-size:12px;margin:0'>&copy; 2024 SIWES360. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await _emailSender.SendEmailAsync(email, subject, body);
            return Result.Success();
        }
    }
}
