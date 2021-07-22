using Infrastructure.Adapter.Email.Interfaces;
using Infrastructure.Adapter.Email.Models;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MimeKit;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Domain.Settings;

namespace Infrastructure.Adapter.Email.Adapters
{
    public class EmailAmazonAdapter : IEmailService
    {
        private readonly IAmazonSimpleEmailService Client;
        private readonly IOptions<EmailOptions> EmailConfiguration;
        private readonly ILogger<EmailAmazonAdapter> Logger;
        public EmailAmazonAdapter(IOptions<EmailOptions> emailConfiguration, 
                                  IAmazonSimpleEmailService client,
                                  ILogger<EmailAmazonAdapter> logger)
        {
            Client = client;
            EmailConfiguration = emailConfiguration;
            Logger = logger;
        }

        public async Task<EmailResponse> SendEmail(EmailRequest emailRequest)
        {
            EmailResponse emailResponse = new EmailResponse();

            var sendRequest = new SendEmailRequest
            {
                Source = EmailConfiguration.Value.SenderAddress,

                Destination = new Destination
                {
                    ToAddresses = emailRequest.Receivers,
                    CcAddresses = emailRequest.CopyReceivers,
                    BccAddresses = emailRequest.CopyReceiversHidden
                },
                Message = new Message
                {
                    Subject = new Content(emailRequest.Subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = emailRequest.HtmlBody
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = emailRequest.TextBody
                        }
                    }
                }
            };
            try
            {
                await Client.SendEmailAsync(sendRequest);
                emailResponse.Success = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Email has not send");
                emailResponse.Success = false;
            }
            finally
            {
                Client.Dispose();
            }

            return emailResponse;
        }


        public async Task<EmailResponse> SendEmailWhitAttachments(EmailRequestAttachment emailRequest)
        {
            EmailResponse emailResponse = new EmailResponse();
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailRequest.HtmlBody,
                TextBody = emailRequest.TextBody
            };

            foreach (var file in emailRequest.Attachments)
            {
                bodyBuilder.Attachments.Add(file.Name, file.File, ContentType.Parse(file.Type));
            }
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(EmailConfiguration.Value.SenderAddress));

            foreach (string receiver in emailRequest.Receivers)
                mimeMessage.To.Add(MailboxAddress.Parse(receiver));

            foreach (string copyes in emailRequest.CopyReceivers)
                mimeMessage.Cc.Add(MailboxAddress.Parse(copyes));

            foreach (string copyes in emailRequest.CopyReceiversHidden)
                mimeMessage.Bcc.Add(MailboxAddress.Parse(copyes));

            mimeMessage.Subject = emailRequest.Subject;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            using (var messageStream = new MemoryStream())
            {
                await mimeMessage.WriteToAsync(messageStream);
                var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(messageStream) };

                try
                {
                    var response = await Client.SendRawEmailAsync(sendRequest);

                    emailResponse.Success = true;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Email has not send");
                    emailResponse.Success = false;
                }
                finally
                {
                    Client.Dispose();
                }
            }
            Logger.LogInformation("Email send!");

            return emailResponse;
        }


        public async Task<EmailResponse> SendEmailWithTemplate(EmailTemplateRequest emailWithTemplateRequest)
        {
            EmailResponse emailResponse = new EmailResponse();

            var sendRequest = new SendTemplatedEmailRequest
            {
                Source = EmailConfiguration.Value.SenderAddress,
                Destination = new Destination
                {
                    ToAddresses = emailWithTemplateRequest.Receivers,
                    CcAddresses = emailWithTemplateRequest.CopyReceivers,
                    BccAddresses = emailWithTemplateRequest.CopyReceiversHidden
                },
                Template = emailWithTemplateRequest.Template,
                TemplateData = JsonSerializer.Serialize(emailWithTemplateRequest.Data)
            };
            try
            {
                await Client.SendTemplatedEmailAsync(sendRequest);
                emailResponse.Success = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Email has not send");
                emailResponse.Success = false;
            }

            Client.Dispose();

            return emailResponse;
        }
    }
}
