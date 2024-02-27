using System;
using AzureFunc.Data;
using AzureFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunc
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureDbContext _db;
        public OnQueueTriggerUpdateDatabase(AzureDbContext db)
        {
            _db = db;
        }

        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("SalesRequestInBound", Connection = "AzureWebJobsStorage")]SalesRequest myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            myQueueItem.Status = "Submitted";
            _db.SalesRequests.Add(myQueueItem);
            _db.SaveChanges();
        }
    }
}
