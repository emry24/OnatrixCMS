using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using OnatrixCMS.Models;
using System.Text;


namespace OnatrixCMS.Services;

public class EmailService
{
    
   
    private readonly ServiceBusClient _serviceBusClient;

    public EmailService(ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }

    public async Task<bool> SendEmailAsync(string email)
    {
        try
        {
            var emailRequest = GenereateConfirmEmail(email);
            if (emailRequest != null)
            {
                var sender = _serviceBusClient.CreateSender("email_request");
                await sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(emailRequest))));
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
           
        }
        return false;    
    
    }

    private EmailRequestModel GenereateConfirmEmail(string email)
    {
        return new EmailRequestModel
        {
            To = email,
            Subject = "We have received",
            HtmlBody = email,
            PlainText = email,
        };
    }
}
