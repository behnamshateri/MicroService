using System;
using RabbitMQ.Client;
using System.Text;
using EventBusRabbitMQ.Events;
using Newtonsoft.Json;


namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMqProducer
    {
        private readonly IRabbitMqConnection _connection;

        public EventBusRabbitMqProducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void PublishBasketCheckout(string queueName, BasketCheckoutEvent publishModel)
        {
            using (var channel = _connection.CreateModel())
            {
                // durable: false -> dar memory  , true -> dar database
                // exclusive: permission mide baraye estefade be baghie connectionha 
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false); // elam mikone ke ye queue hast
                var message = JsonConvert.SerializeObject(publishModel);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties(); // properties haro sakhtan
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                
                channel.ConfirmSelect(); // tayidie publisher
                channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: true, basicProperties:properties, body:body);  // ye message ro publish mikone
                channel.WaitForConfirmsOrDie();  // vaymise ta tayidie migire ke hameye message ha publish shode

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent to rabbit mq."); // vaqti ye acks az broker miad samte ma in call mishe baraye tayidie
                };
                
                channel.ConfirmSelect();
            }
        }
        
    }
}