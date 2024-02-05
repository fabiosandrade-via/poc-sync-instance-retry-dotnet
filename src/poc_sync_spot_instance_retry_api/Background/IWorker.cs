using poc_sync_spot_instance_retry_api.Models;

namespace poc_sync_spot_instance_retry_api.Background
{
    public interface IWorker
    {
        Task<SpotInstanceModel> ExecuteAsync();
    }
}