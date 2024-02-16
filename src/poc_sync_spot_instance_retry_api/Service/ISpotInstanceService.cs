using poc_sync_spot_instance_retry_api.Models;

namespace poc_sync_spot_instance_retry_api.Service
{
    public interface ISpotInstanceService
    {
        Task<SpotInstanceModel> ExecuteAsync();
    }
}