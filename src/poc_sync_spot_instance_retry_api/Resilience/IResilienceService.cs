using poc_sync_spot_instance_retry_api.Models;

namespace poc_sync_spot_instance_retry_api.Resilience
{
    public interface IResilienceService
    {
        Task<SpotInstanceModel> ExecuteAsync();
    }
}