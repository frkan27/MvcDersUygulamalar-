using Rabbit.Model.Entities;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rabbit.Consumer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static Consumer _consumer;
        private void Form1_Load(object sender, EventArgs e)
        {
            _consumer = new Consumer("Customer");
            _consumer.ConsumerEvent.Received += ConsumerEvent_Receiver;
            ConsumerEvent_Receiver(sender, new BasicDeliverEventArgs());
        }

        private void ConsumerEvent_Receiver(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
