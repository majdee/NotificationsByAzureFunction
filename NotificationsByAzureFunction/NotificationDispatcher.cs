using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NotificationsByAzureFunction
{
    public static class NotificationDispatcher
    {
        [FunctionName("NotificationDispatcher")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest httpRequest,
            [Queue("smsQueue"), StorageAccount("AzureWebJobsStorage")] IAsyncCollector<NotificationDispatcherViewModel> smsQueue,
            [Queue("emailQueue"), StorageAccount("AzureWebJobsStorage")] IAsyncCollector<NotificationDispatcherViewModel> emailQueue,
            ILogger log)
        {
            log.LogInformation("Notification HTTP trigger function processed a request.");

            var requestContent = await httpRequest.ReadAsStringAsync();
            var notificationDispatcherViewModel = JsonConvert.DeserializeObject<NotificationDispatcherViewModel>(requestContent);

            if (!notificationDispatcherViewModel.IsValid)
            {
                return new BadRequestObjectResult("Invalid input...");
            }

            switch (notificationDispatcherViewModel.NotifyBy.ToLower())
            {
                case "email":
                    await emailQueue.AddAsync(notificationDispatcherViewModel);
                    break;
                case "sms":
                    await smsQueue.AddAsync(notificationDispatcherViewModel);
                    break;
                default:
                    log.LogInformation("Invalid notification type, message will be ignored!");
                    break;
            }

            return new OkObjectResult("Notification will be sent soon.");
        }

    }
}
