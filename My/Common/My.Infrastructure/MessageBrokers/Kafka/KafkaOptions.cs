﻿using System.Collections.Generic;

namespace My.Infrastructure.MessageBrokers.Kafka;

public class KafkaOptions
{
    public string BootstrapServers { get; set; }

    public string GroupId { get; set; }

    public Dictionary<string, string> Topics { get; set; }
}
