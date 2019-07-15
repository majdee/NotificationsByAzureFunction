using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NotificationsByAzureFunction
{
    public static class SmsReceiver
    {
        [FunctionName("SmsReceiver")]
        public static async Task Run([QueueTrigger("smsQueue", Connection = "AzureWebJobsStorage")]
            NotificationDispatcherViewModel queueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {queueItem}");

            var accountSid = System.Environment.GetEnvironmentVariable("TWILO_ACCOUNT_SID");
            var authToken = System.Environment.GetEnvironmentVariable("TWILO_AUTH_TOKEN");
            var twilioPhoneNumber = System.Environment.GetEnvironmentVariable("TWILO_PHONE_NUMBER");

            TwilioClient.Init(accountSid, authToken);

            foreach (var receiver in queueItem.Receivers)
            {
                var message = MessageResource.Create(
                    body: "From Shams App",
                    @from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(receiver)
                );

                log.LogInformation($"Sending SMS with sid {message.Sid}");
            }
          
        }
    }
}
