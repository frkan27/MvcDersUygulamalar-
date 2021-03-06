﻿using Newtonsoft.Json;
using Rabbit.BLL.RabbitMq;
using Rabbit.BLL.Repository;
using Rabbit.Model.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.Consumer
{
  public class Consumer
    {
        private readonly RabbitMqService _rabbitMqService;
        public EventingBasicConsumer ConsumerEvent;

        public Consumer(string queueName)
        {
            _rabbitMqService = new RabbitMqService();
            var connection = _rabbitMqService.GetRabbitMqConnection();
            var channel = connection.CreateModel();
            ConsumerEvent = new EventingBasicConsumer(channel);

            ConsumerEvent.Received += (model, ea) =>
              {
                  var body = ea.Body;
                  var message = Encoding.UTF8.GetString(body);
                  if (queueName == "MailLog")
                  {
                      var data = JsonConvert.DeserializeObject<List<MailLog>>(message);
                  }
                  else if (queueName == "Customer")
                  {
                      var data = JsonConvert.DeserializeObject<List<Customer>>(message);
                      var repo = new CustomerRepo();
                      foreach (var item in data)
                      {
                          repo.Insert(new Customer()
                          {
                              Address=item.Address,
                              Email=item.Email,
                              Name=item.Name,
                              Surname=item.Surname,
                              Phone=item.Phone,
                              RegisterDate=item.RegisterDate
                          })
                      }
                  }
              };
            channel.BasicConsume(queueName, true, ConsumerEvent);
        }
    }
}
