using System;
using System.Text;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using Mapster;
using MediatR;
using Newtonsoft.Json;
using Order.Application.Commands;
using Order.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Order.Api.RabbitMq
{
    public class EventBusRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public EventBusRabbitMqConsumer(IRabbitMqConnection rabbitMqConnection, IOrderRepository orderRepository, IMediator mediator)
        {
            _connection = rabbitMqConnection;
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public void Consume()
        {
            Console.WriteLine("Consume is started");
            using var channel = _connection.CreateModel();
            
            // durable: false -> dar memory  , true -> dar database
            // exclusive: permission mide baraye estefade be baghie connectionha 
            // vaqti ma darim declare mikonim, yani mikhaym in queue ro bekhunim
            channel.QueueDeclare(queue: EventBusConstant.BasketCheckoutQueue, durable: false, exclusive: false, autoDelete: false, null); // elam mikone ke ye queue hast

            var consumer = new EventingBasicConsumer(channel);

            // inja darim attach mikonim in event ro vaqti recived etefagh biofte chi beshe
            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: EventBusConstant.BasketCheckoutQueue, autoAck: true, consumer: consumer);
            
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
        
        private async void ReceivedEvent(object sender, BasicDeliverEventArgs eventArgs)
        {
            Console.WriteLine($"ReceivedEvent {eventArgs.RoutingKey}");
            if (eventArgs.RoutingKey == EventBusConstant.BasketCheckoutQueue)
            {
                // get message
                var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
                
                // deserialize it
                BasketCheckoutEvent basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);
                
                // map this event to command
                CheckoutOrderCommand command = basketCheckoutEvent.Adapt<CheckoutOrderCommand>();

                var response = await _mediator.Send(command);
            }
        }
        
        public void Disconnect()
        {
            Console.WriteLine("Disconnecting from rabbitmq.");
            _connection.Dispose();
        }
    }
}