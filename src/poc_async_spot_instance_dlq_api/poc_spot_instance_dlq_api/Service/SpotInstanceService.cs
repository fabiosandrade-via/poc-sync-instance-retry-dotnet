using poc_async_spot_instance_dlq_api.Models;
using Polly.Retry;
using Polly.Wrap;
using System.Net;

namespace poc_async_spot_instance_dlq_api.Service
{
    public class SpotInstanceService : ISpotInstanceService
    {
        private readonly ILogger<SpotInstanceService> _logger;
        private readonly IConfiguration _configuration;
        private readonly AsyncRetryPolicy _resiliencePolicy;

        public SpotInstanceService(ILogger<SpotInstanceService> logger,
            IConfiguration configuration,
            AsyncRetryPolicy resiliencePolicy)
        {
            _logger = logger;
            _configuration = configuration;
            _resiliencePolicy = resiliencePolicy;
        }
        public async Task<SpotInstanceModel> ExecuteAsync()
        {
            var httpClient = new HttpClient();
            string urlApiSpotInstance = _configuration["UrlApiSpotInstance"];
            int threshold = Convert.ToInt32(_configuration["Threshold"]);

            CancellationTokenSource stoppingToken = new CancellationTokenSource();
            SpotInstanceModel spotInstanceModel = new SpotInstanceModel();

            int contThreshold = 0;

            while (!stoppingToken.IsCancellationRequested && contThreshold < threshold)
            {
                try
                {
                    spotInstanceModel = await _resiliencePolicy.ExecuteAsync<SpotInstanceModel>(() =>
                    {
                        return httpClient
                            .GetFromJsonAsync<SpotInstanceModel>(urlApiSpotInstance);
                    });

                    string logMessage = $"* {DateTime.Now:HH:mm:ss} * " +
                                        $"StatusCode = {spotInstanceModel.StatusCode} | " +
                                        $"Mensagem = {spotInstanceModel.Message}";
                    _logger.LogInformation(logMessage);
                    spotInstanceModel.Logs.Add(logMessage);
                    contThreshold = threshold;
                }
                catch (Exception ex)
                {
                    string logMessage = $"# {DateTime.Now:HH:mm:ss} # " +
                                        $"Padrão DLQ iniciado devido a: {ex.GetType().FullName} | {ex.Message}";
                    _logger.LogError(logMessage);
                    spotInstanceModel.Message = ex.Message;
                    spotInstanceModel.Logs.Add(logMessage);
                    spotInstanceModel.StatusCode = HttpStatusCode.OK;
                }

                contThreshold++;
                await Task.Delay(1000, stoppingToken.Token);
            }

            return await Task.FromResult(spotInstanceModel);
        }
    }
}
