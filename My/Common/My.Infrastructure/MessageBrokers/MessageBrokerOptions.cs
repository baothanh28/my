using My.Infrastructure.MessageBrokers.AzureEventGrid;
using My.Infrastructure.MessageBrokers.AzureEventHub;
using My.Infrastructure.MessageBrokers.AzureQueue;
using My.Infrastructure.MessageBrokers.AzureServiceBus;
using My.Infrastructure.MessageBrokers.Kafka;
using My.Infrastructure.MessageBrokers.RabbitMQ;

namespace My.Infrastructure.MessageBrokers;

public class MessageBrokerOptions
{
    public string Provider { get; set; }

    public RabbitMQOptions RabbitMQ { get; set; }

    public KafkaOptions Kafka { get; set; }

    public AzureQueueOptions AzureQueue { get; set; }

    public AzureServiceBusOptions AzureServiceBus { get; set; }

    public AzureEventGridOptions AzureEventGrid { get; set; }

    public AzureEventHubOptions AzureEventHub { get; set; }

    public bool UsedRabbitMQ()
    {
        return Provider == "RabbitMQ";
    }

    public bool UsedKafka()
    {
        return Provider == "Kafka";
    }

    public bool UsedAzureQueue()
    {
        return Provider == "AzureQueue";
    }

    public bool UsedAzureServiceBus()
    {
        return Provider == "AzureServiceBus";
    }

    public bool UsedAzureEventGrid()
    {
        return Provider == "AzureEventGrid";
    }

    public bool UsedAzureEventHub()
    {
        return Provider == "AzureEventHub";
    }

    public bool UsedFake()
    {
        return Provider == "Fake";
    }
}
