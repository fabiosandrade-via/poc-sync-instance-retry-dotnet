using poc_sync_spot_instance_retry_api.Models;
using Polly;
using System.Net;

namespace poc_sync_spot_instance_retry_api.Background
{
    public class Worker : IWorker
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly AsyncPolicy _resiliencePolicy;

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration,
            AsyncPolicy resiliencePolicy)
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
            spotInstanceModel.GeneralThreshold = threshold;

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

                    spotInstanceModel = GetSpotInstance(spotInstanceModel, "Acesso a spot instance executada com sucesso.", HttpStatusCode.OK, logMessage);
                    stoppingToken.Cancel();
                    contThreshold = threshold;
                }
                catch (Exception ex)
                {
                    string logMessage = $"# {DateTime.Now:HH:mm:ss} # " +
                                        $"Falha ao invocar a API: {ex.GetType().FullName} | {ex.Message}";

                    spotInstanceModel = GetSpotInstance(spotInstanceModel, "Acesso a spot instance indisponível.", HttpStatusCode.BadRequest, logMessage);
                }

                contThreshold++;
                await Task.Delay(1000, stoppingToken.Token);
            }

            return await Task.FromResult<SpotInstanceModel>(spotInstanceModel);
        }
        private SpotInstanceModel GetSpotInstance(SpotInstanceModel spotInstanceModel, string message, HttpStatusCode httpStatusCode, string logMessage)
        {
            spotInstanceModel.Message = message;
            spotInstanceModel.StatusCode = httpStatusCode;

            if (httpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation(logMessage);
            }
            else
            {
                _logger.LogError(logMessage);
            }

            spotInstanceModel.Logs.Add(logMessage);
            return spotInstanceModel;
        }
    }
}
