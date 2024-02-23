using Confluent.Kafka;

namespace poc_consumer_spot_instance_srv
{
    public class ConsumerBrokerKafka
    {
        public ConsumerBrokerKafka()
        {
            this.Consumer();
        }
        private async void Consumer()
        {
            var config = new ClientConfig()
            {
                BootstrapServers = "broker:9092",
            };

            var consumerConfig = new ConsumerConfig(config);
            consumerConfig.GroupId = "group-keyvault2";
            consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            consumerConfig.EnableAutoCommit = false;
            string topic = "poc-spot-topic";

            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
            {
                consumer.Subscribe(topic);

                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);
                         Console.WriteLine($"Consumo de registro com a chave {cr.Message.Key} e valor {cr.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl-C was pressed.
                }
                finally
                {
                    consumer.Close();
                }

                await Task.CompletedTask;
            }
        }
    }
}