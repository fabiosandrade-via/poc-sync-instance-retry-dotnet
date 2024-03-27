using k8s;
using k8s.Models;

namespace poc_sync_spot_instance_retry_api.Infrastructure
{
    public static class DataInfra
    {
        public static string GetEnviroment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
        public static string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }
        public static string GetNodeName(IConfiguration configuration)
        {
            try
            {
                var selector = $"app={configuration["AppSelector"]}";
                var configAks = KubernetesClientConfiguration.BuildConfigFromConfigFile();
                Kubernetes client = new Kubernetes(configAks);
                var list = client.CoreV1.ListNamespacedPod("default", labelSelector: selector, pretty: true);
                return list.Items.FirstOrDefault().Name();
            }
            catch
            {
                return "Erro na busca";
            }
        }
    }
}
