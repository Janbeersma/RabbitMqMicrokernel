using RabbitMQ.Client;
using System.Text;
using System;
using Newtonsoft.Json;

namespace RabbitMQ
{
    static class Program
    {
        static void Main(string[] args)
        {
            //Maakt een connectiefabriek
            var factory = new ConnectionFactory
            {
                //De Uri waarop de docker omgeving staat met het poortnummer waarop RabbitMQ luisterd
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            //Maakt een connectie naar een van de endpoints default worden geen parameters meegegeven
            //en de connectie wordt terug gegeven door de EndpointResolverFactory
            using var connection = factory.CreateConnection();

            //Maakt en returned een nieuw kannaal, sessie en model
            using var channel = connection.CreateModel();

            //Benaamd een queue en geeft de configuratie aan voor deze queue
            channel.QueueDeclare("modelgen-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);


            for (int i = 0; i < 50; i++)
            {
                //Maak een message aan die gestuurd kan worden naar de queue
                var message = new { Name = "Producer", Message = "Hallo konijn" };

                //Serialize de message object als Json en sla dit op in de var body
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("", "modelgen-queue", null, body);
            }
        }
    }
}
