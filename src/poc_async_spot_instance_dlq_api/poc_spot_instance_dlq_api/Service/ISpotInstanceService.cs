using poc_async_spot_instance_dlq_api.Models;

namespace poc_async_spot_instance_dlq_api.Service
{
    public interface ISpotInstanceService
    {
        Task<SpotInstanceModel> ExecuteAsync();
    }
}