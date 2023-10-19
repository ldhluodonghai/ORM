
using Confluent.Kafka;

using Usermanger.KafkaDomain.Entities;

namespace Usermanger.KafkaDomain.Service
{
    public class KafkaProducer
    {
        /* private readonly string topic = "simpletalk_topic";
         public Task StartAsync(CancellationToken cancellationToken)
         {
             var conf = new ConsumerConfig
             {
                 GroupId = "st_consumer_group",
                 BootstrapServers = "localhost:9092",
                 AutoOffsetReset = AutoOffsetReset.Earliest
             };
             using (var builder = new ConsumerBuilder<Ignore,
                 string>(conf).Build())
             {
                 builder.Subscribe(topic);
                 var cancelToken = new CancellationTokenSource();
                 try
                 {
                     while (true)
                     {
                         var consumer = builder.Consume(cancelToken.Token);
                         Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                     }
                 }
                 catch (Exception)
                 {
                     builder.Close();
                 }
             }
             return Task.CompletedTask;
         }
         public Task StopAsync(CancellationToken cancellationToken)
         {
             return Task.CompletedTask;
         }*/
        public async Task MyConsumer()
        {
            KafkaService.KAFKA_SERVERS = "kafka1:9091,kafka2:9092,kafka3:9093";
            var kafkaService = new KafkaService();
            for (int i = 0; i < 50; i++)
            {
                var eventData = new Events
                {
                    TopicName = "testtopic",
                    Message = $"This is a message from Producer, Index : {i + 1}",
                    EventTime = DateTime.Now
                };
                await kafkaService.PublishAsync(eventData.TopicName, eventData);
            }
            Console.WriteLine("Publish Done!");
            Console.ReadKey();
        }
    }
}
