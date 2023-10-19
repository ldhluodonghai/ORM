
using Usermanger.KafkaDomain.Entities;

namespace Usermanger.KafkaDomain.Service
{
    public class KafkaConsumer
    {

        public async Task MyConsumer()
        {
            KafkaService.KAFKA_SERVERS = "kafka1:9091,kafka2:9092,kafka3:9093";
            var kafkaService = new KafkaService();
            var topics = new List<string> { "testtopic" };
            await kafkaService.SubscribeAsync<Events>(topics, (eventData) =>
            {
                Console.WriteLine($" - {eventData.EventTime: yyyy-MM-dd HH:mm:ss} 【{eventData.TopicName}】- > 已处理");
            });
        }
    }
}
