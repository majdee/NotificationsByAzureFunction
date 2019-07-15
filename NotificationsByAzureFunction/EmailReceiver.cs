using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationsByAzureFunction
{
    public static class EmailReceiver
    {
        [FunctionName("EmailReceiver")]
        public static async Task Run([QueueTrigger("emailQueue", Connection = "AzureWebJobsStorage")]
            NotificationDispatcherViewModel queueItem, ILogger log)
        {
            log.LogInformation($"Email queue trigger function processed: {queueItem}");

            var apiKey = System.Environment.GetEnvironmentVariable("SendGridKey");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("smajdee@gmail.com", "Majdee Zoabi"),
                Subject = "Hello World SendGrid!",
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };

            foreach (var sendToEmail in queueItem.Receivers)
            {
                msg.AddTo(new EmailAddress(sendToEmail));
            }
           
            var response = await client.SendEmailAsync(msg);

            log.LogInformation($"Sending email status is {response.StatusCode}");
        }
    }
}
