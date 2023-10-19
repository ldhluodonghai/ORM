namespace Usermanger.KafkaDomain.Entities
{
    public class Events
    {
        public string TopicName { get; set; }

        public string Message { get; set; }

        public DateTime EventTime { get; set; }

    }
}
