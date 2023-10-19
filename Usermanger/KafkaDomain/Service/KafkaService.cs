using Confluent.Kafka;
using Newtonsoft.Json;

namespace Usermanger.KafkaDomain.Service
{
    public interface IKafkaService
    {
        Task PublishAsync<T>(string topicName, T message) where T : class, new();

        Task SubscribeAsync<T>(IEnumerable<string> topics, Action<T> messageFunc, CancellationToken cancellationToken = default) where T : class, new();
    }
    public class KafkaService : IKafkaService
    {
        public static string KAFKA_SERVERS = "127.0.0.1:9092";

        public async Task PublishAsync<T>(string topicName, T message) where T : class,new()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = KAFKA_SERVERS,
                BatchSize = 16384, // 修改批次大小为16K
                LingerMs = 20 // 修改等待时间为20ms
            };
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                await producer.ProduceAsync(topicName, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = JsonConvert.SerializeObject(message)
                }); 
            }
        }

        public async Task SubscribeAsync<T>(
            IEnumerable<string> topics, Action<T> messageFunc, CancellationToken cancellationToken = default) where T : class, new()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = KAFKA_SERVERS,
                GroupId = "Consumer",
                EnableAutoCommit = false, // 禁止AutoCommit
                Acks = Acks.Leader, // 假设只需要Leader响应即可
                AutoOffsetReset = AutoOffsetReset.Earliest // 从最早的开始消费起
            };
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topics);
                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cancellationToken);
                            Console.WriteLine($"Consumed message '{consumeResult.Message?.Value}' at: '{consumeResult?.TopicPartitionOffset}'.");
                            if (consumeResult.IsPartitionEOF)
                            {
                                Console.WriteLine($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} 已经到底了：{consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                                continue;
                            }
                            T messageResult = null;
                            try
                            {
                                messageResult = JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);
                            }
                            catch (Exception ex)
                            {
                                var errorMessage = $" - {DateTime.Now:yyyy-MM-dd HH:mm:ss}【Exception 消息反序列化失败，Value：{consumeResult.Message.Value}】 ：{ex.StackTrace?.ToString()}";
                                Console.WriteLine(errorMessage);
                                messageResult = null;
                            }
                            if (messageResult != null/* && consumeResult.Offset % commitPeriod == 0*/)
                            {
                                messageFunc(messageResult);
                                try
                                {
                                    consumer.Commit(consumeResult);
                                }
                                catch (KafkaException e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            }

            await Task.CompletedTask;
        }
    }
}
