using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Services
{
    public class AzureQueueService : IQueueService
    {
        CloudStorageAccount storageAccount;
        CloudQueueClient queueClient;


        public AzureQueueService()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));
            queueClient = storageAccount.CreateCloudQueueClient();
        }

        public string QueueBaseUrl
        {
            get
            {
                return queueClient.BaseUri.ToString();
            }
        }

        public async Task CreateQueueAsync(string queueName)
        {
            // Retrieve a reference of queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();
        }

        public async Task AddMessageAsync(string queueName, object obj)
        {
            // Retrieve a reference of queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            var message = JsonConvert.SerializeObject(obj);
            CloudQueueMessage msg = new CloudQueueMessage(message);
            await queue.AddMessageAsync(msg);
        }

    }
}
