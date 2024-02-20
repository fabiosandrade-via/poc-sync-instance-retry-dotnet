using poc_async_spot_instance_dlq_api.Enumerator;
using poc_async_spot_instance_dlq_api.Models;
using poc_spot_instance_dlq_broker;
using Polly.Retry;
using System.Net;

namespace poc_async_spot_instance_dlq_api.Service
{
    public class SpotInstanceService : ISpotInstanceService
    {
        private readonly ILogger<SpotInstanceService> _logger;
        private readonly IConfiguration _configuration;
        private readonly AsyncRetryPolicy _resiliencePolicy;

        HttpClient httpClient;
        string urlApiSpotInstance = string.Empty;
        SpotInstanceModel spotInstanceModel;

        public SpotInstanceService(ILogger<SpotInstanceService> logger,
                                   IConfiguration configuration,
                                   AsyncRetryPolicy resiliencePolicy)
        {
            _logger = logger;
            _configuration = configuration;
            _resiliencePolicy = resiliencePolicy;

            httpClient = new HttpClient();
            urlApiSpotInstance = _configuration["UrlApiSpotInstance"];
        }
        public async Task<SpotInstanceModel> ExecuteInsertAsync(ArchitectureModel architectureModel)
        {
            spotInstanceModel = new SpotInstanceModel();
            architectureModel.TypeOperation = TypeOperation.Insert;

            try
            {
                spotInstanceModel = await _resiliencePolicy.ExecuteAsync<SpotInstanceModel>(async () =>
                {
                    var response = httpClient
                        .PostAsJsonAsync(urlApiSpotInstance, architectureModel);
                    return await response.Result.Content.ReadFromJsonAsync<SpotInstanceModel>();
                });

                string logMessage = $"* {DateTime.Now:HH:mm:ss} * " +
                                    $"StatusCode = {spotInstanceModel.StatusCode} | " +
                                    $"Mensagem = {spotInstanceModel.Message}";
                spotInstanceModel = GetSpotInstanceModel(spotInstanceModel, logMessage, false);
            }
            catch (Exception ex)
            {
                string logMessage = $"# {DateTime.Now:HH:mm:ss} # " +
                                    $"Padrão DLQ iniciado na operação de {architectureModel.TypeOperation.ToString()} devido a: {ex.GetType().FullName} | {ex.Message}";
                spotInstanceModel = GetSpotInstanceModel(spotInstanceModel, logMessage, true);
                spotInstanceModel.Message = ex.Message;
                ProducerBrokerKafka.Send<ArchitectureModel>(architectureModel);
            }

            return await Task.FromResult(spotInstanceModel);
        }
        public async Task<SpotInstanceModel> ExecuteUpdateAsync(ArchitectureModel architectureModel)
        {
            spotInstanceModel = new SpotInstanceModel();
            architectureModel.TypeOperation = TypeOperation.Update;

            try
            {
                spotInstanceModel = await _resiliencePolicy.ExecuteAsync<SpotInstanceModel>(async () =>
                {
                    var response = httpClient
                        .PutAsJsonAsync(urlApiSpotInstance, architectureModel);
                    return await response.Result.Content.ReadFromJsonAsync<SpotInstanceModel>();
                });

                string logMessage = $"* {DateTime.Now:HH:mm:ss} * " +
                                    $"StatusCode = {spotInstanceModel.StatusCode} | " +
                                    $"Mensagem = {spotInstanceModel.Message}";
                spotInstanceModel = GetSpotInstanceModel(spotInstanceModel, logMessage, false);
            }
            catch (Exception ex)
            {
                string logMessage = $"# {DateTime.Now:HH:mm:ss} # " +
                                    $"Padrão DLQ iniciado na operação {architectureModel.TypeOperation.ToString()} devido a: {ex.GetType().FullName} | {ex.Message}";
                spotInstanceModel = GetSpotInstanceModel(spotInstanceModel, logMessage, true);
                spotInstanceModel.Message = ex.Message;
                ProducerBrokerKafka.Send<ArchitectureModel>(architectureModel);
            }

            return await Task.FromResult(spotInstanceModel);
        }
        private SpotInstanceModel GetSpotInstanceModel(SpotInstanceModel spotInstanceModel, string logMessage, bool error)
        {
            if (error)
            {
                _logger.LogError(logMessage);
            }
            else
            {
                _logger.LogInformation(logMessage);
            }

            spotInstanceModel.Logs.Add(logMessage);
            spotInstanceModel.StatusCode = HttpStatusCode.OK;

            return spotInstanceModel;
        }
    }
}
