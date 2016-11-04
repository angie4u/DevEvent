using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Services
{
    public interface IQueueService
    {
        string QueueBaseUrl { get; }

        Task CreateQueueAsync(string queueName);

        Task AddMessageAsync(string queueName, object message);
    }
}
