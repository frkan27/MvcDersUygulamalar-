using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.BLL.RabbitMq
{
   public  class RabbitMqService
    {
        private readonly string _hostName = "localhost",
            _username = "furkan",
            _password = "123456";

        public IConnection GetRabbitMqConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = _hostName,
                VirtualHost = "/",
                UserName = _username,
                Password = _password,
                Uri = new Uri($"amqp://{_username}:{_password}@{_hostName}")
            };
            return connectionFactory.CreateConnection();
        }
    }
}
