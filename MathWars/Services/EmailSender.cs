using MathWars.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MathWars.Services;

public class EmailSenderService : IEmailSenderService
{
    private const string templatePath = @"EmailTemplates/{0}.html";
    private readonly SMTPConfigModel _smtpConfig;

    public async Task SendTestEmail(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, This is test email subjecy from MathWars", userEmailOptions.PlaceHolders);
        userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("TestEmail"), userEmailOptions.PlaceHolders);

        await SendEmail(userEmailOptions);
    }

    public async Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = UpdatePlaceHolders("Cześć {{UserName}}, potwierdź swojego emiala", userEmailOptions.PlaceHolders);
        userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirmation"), userEmailOptions.PlaceHolders);

        await SendEmail(userEmailOptions);
    }

    public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = UpdatePlaceHolders("Cześć {{UserName}}, zresetuj swoje hasło", userEmailOptions.PlaceHolders);
        userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

        await SendEmail(userEmailOptions);
    }


    public EmailSenderService(IOptions<SMTPConfigModel> smtpConfig)
    {
        _smtpConfig = smtpConfig.Value;
    }

    private async Task SendEmail(UserEmailOptions userEmailOptions)
    {
        MailMessage mail = new MailMessage()
        {
            Subject = userEmailOptions.Subject,
            Body = userEmailOptions.Body,
            From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
            IsBodyHtml = _smtpConfig.IsBodyHTML,
        };

        foreach (var toEmail in userEmailOptions.ToEmails)
        {
            mail.To.Add(toEmail);
        }

        NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

        SmtpClient smtpClient = new SmtpClient()
        {
            Host = _smtpConfig.Host,
            Port = _smtpConfig.Port,
            EnableSsl = _smtpConfig.EnableSSL,
            UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
            Credentials = networkCredential
        };

        mail.BodyEncoding = Encoding.Default;

        await smtpClient.SendMailAsync(mail);
    }

    private string GetEmailBody(string templateName)
    {
        var body = File.ReadAllText(string.Format(templatePath, templateName));
        return body;
    }

    private string UpdatePlaceHolders(string text, List<KeyValuePair<string,string>> keyValuePair)
    {
        if(!string.IsNullOrEmpty(text) && keyValuePair != null)
        {
            foreach (var pairholder in keyValuePair) 
            {
                if (text.Contains(pairholder.Key))
                {
                    text = text.Replace(pairholder.Key, pairholder.Value);
                }
            }
        }
        return text;
    }
}