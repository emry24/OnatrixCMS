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
            Subject = "We have received your message",
            HtmlBody = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);'>
                            <div style='background-color: #4CAF50; padding: 20px; border-top-left-radius: 10px; border-top-right-radius: 10px;'>
                                <h1 style='color: #ffffff; text-align: center;'>Thank you, {email}!</h1>
                            </div>
                        <div style='padding: 20px; color: #333333;'>
                            <p>We have received your message and our team will get back to you shortly.</p>
                            <p style='color: #777777;'>In the meantime, feel free to explore our website or contact us for any urgent matters.</p>
                        </div>
                        <div style='background-color: #f9f9f9; padding: 10px; text-align: center; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px;'>
                            <p style='font-size: 12px; color: #999999;'>© 2024 Onatrix Inc. All rights reserved.</p>
                        </div>
                        </div>
                    </body>
            </html>",
            PlainText = $"Thank you, {email}! We have received your message and our team will get back to you shortly. Feel free to contact us if you need immediate assistance."
            //HtmlBody = $"<html><body><h1>Thank you, {email}. We will get back to you shortly.</h1></body></html>",
            //PlainText = $"Thank you, {email}. We will get back to you shortly."
        };
    }
}
