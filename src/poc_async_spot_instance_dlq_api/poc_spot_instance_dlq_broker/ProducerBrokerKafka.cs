using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace poc_spot_instance_dlq_broker
{
    public abstract class ProducerBrokerKafka
    {
        public static void Send<T>(T entity) where T : class
        {
            try
            {
                string topic = "pocspottopic";

                var config = new ClientConfig()
                {
                    BootstrapServers = "127.0.0.1:9092",

                };

                using (var producer = new ProducerBuilder<string, string>(config).Build())
                {
                    var key = "pocspot";
                    var val = JObject.FromObject(new { entity }).ToString(Formatting.None);

                    string valSubst = val.Replace("{\"entity", "");
                    valSubst = valSubst.Remove(0, 2);
                    valSubst = valSubst.Remove(valSubst.Length - 1);

                    Console.WriteLine($"Produzindo mensagem: {key} {val}");

                    producer.Produce(topic, new Message<string, string> { Key = key, Value = valSubst },
                        (deliveryReport) =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Falha para enviar mensagem: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Mensagem produzida para: {deliveryReport.TopicPartitionOffset}");
                            }
                        });

                    producer.Flush(TimeSpan.FromSeconds(10));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao produzir a mensagem no tópico: {0}", ex.Message);
            }
        }
    }
}