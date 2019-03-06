using Rabbit.BLL.RabbitMq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Rabbit.Web.UI.Models
{
    public class Publisher
    {
        private readonly RabbitMqService _rabbitMqService;
        private const string DefaultQueue = "wissen1";

        public Publisher(string queueName,string message)
        {
            if (string.IsNullOrEmpty(queueName))
                queueName = DefaultQueue;
            _rabbitMqService = new RabbitMqService();

            using (var connection = _rabbitMqService.GetRabbitMqConnection())
            {
                using (var channel=connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);
                    channel.BasicPublish(string.Empty, queueName, null, Encoding.UTF8.GetBytes(message));
                }
            }
            
        }
    }
}