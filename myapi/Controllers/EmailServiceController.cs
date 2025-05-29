using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using myapi.Models;
using DotNetEnv;

namespace myapi.Controllers
{
    public class EmailServiceController : Controller
    {
        private readonly string _emailFrom;
        private readonly string _emailPassword;

        public EmailServiceController()
        {
            DotNetEnv.Env.Load();

            _emailFrom = Environment.GetEnvironmentVariable("EMAIL_FROM");
            _emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            if (string.IsNullOrEmpty(_emailFrom) || string.IsNullOrEmpty(_emailPassword))
            {
                throw new Exception("Email credentials are not configured properly in the .env file.");
            }
        }

        public void SendLoginEmail(string userIdentifier, string password)
        {
            var emailModel = new EmailModel
            {
                From = _emailFrom,
                To = userIdentifier,
                Subject = "Your Login Credentials",
                Body = $@"
            <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Welcome Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }}
        .email-header {{
            background-color: #2a9d8f;
            color: #ffffff;
            text-align: center;
            padding: 20px;
        }}
        .email-header h2 {{
            margin: 0;
            font-size: 24px;
        }}
        .email-body {{
            padding: 20px;
            color: #333333;
            font-size: 16px;
            line-height: 1.5;
        }}
        .email-body p {{
            margin: 0 0 15px;
        }}
        .email-body table {{
            width: 100%;
            table-layout: fixed;
            border-collapse: collapse;
            margin: 20px 0;
        }}
        .email-body td {{
            padding: 5px 10px;
            font-size: 14px;
            color: #555555;
            word-wrap: break-word;
            overflow-wrap: break-word;
            max-width: 100%;
            vertical-align: top;
        }}
        .email-body td:first-child {{
            text-align: right;
            font-weight: bold;
            color: #333333;  
        }}
        .email-body td:last-child {{
            text-align: left;
        }}
        .email-footer {{
            text-align: center;
            font-size: 12px;
            color: #aaaaaa;
            padding: 20px;
            background-color: #f9f9f9;
            border-top: 1px solid #eeeeee;
        }}
        @media only screen and (max-width: 600px) {{
            .email-container {{
                margin: 10px;
                border-radius: 4px;
            }}
            .email-header h2 {{
                font-size: 20px;
            }}
            .email-body {{
                font-size: 14px;
            }}
            .email-body table {{
                font-size: 12px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <!-- Header -->
        <div class=""email-header"">
            <h2>Welcome to Our Service!</h2>
        </div>
        <!-- Body -->
        <div class=""email-body"">
            <p>Dear Student,</p>
            <p>Thank you for logging in. Here are your login details:</p>
            <table>
                <tr>
                    <td>Username:</td>
                    <td>{userIdentifier}</td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td>{password}</td>
                </tr>
            </table>
            <p>We recommend that you change your password after logging in for the first time.</p>
        </div>
        <!-- Footer -->
        <div class=""email-footer"">
            <p>&copy; 2024 Hostel. All Rights Reserved.</p>
        </div>
    </div>
</body>
</html>
            "
            };

            SendEmail(emailModel);
        }

        private void SendEmail(EmailModel em)
        {
            try
            {
                using (var mm = new MailMessage())
                {
                    mm.Subject = em.Subject;
                    mm.Body = em.Body;
                    mm.IsBodyHtml = true;
                    mm.From = new MailAddress(em.From);
                    mm.To.Add(em.To);

                    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.Credentials = new NetworkCredential(_emailFrom, _emailPassword);
                        smtpClient.Send(mm);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
