using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMQ
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        public bool _disposed;

        public RabbitMqConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;
        
        public bool TryConnect()
        {
            int retryForAvailability = 0;
            
            do
            {
                try
                {
                    Console.WriteLine($"Start to connect to rabbit mq for {retryForAvailability} times.");
                    _connection = _connectionFactory.CreateConnection();
                    
                    Console.WriteLine($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");
                    
                    break;
                }
                catch (BrokerUnreachableException ex)
                {
                    Console.WriteLine(ex.Message);
                    
                    Thread.Sleep(4000);
                    
                    Console.WriteLine($"Failed to connect to rabbit mq for {retryForAvailability} times.");
                    retryForAvailability++;
                }
            } while (retryForAvailability < 5);
            
            

            return IsConnected;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No rabbitMq connection");
            }

            return _connection.CreateModel();
        }
        
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            
            try
            {
                _connection.Dispose();
                _disposed = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}