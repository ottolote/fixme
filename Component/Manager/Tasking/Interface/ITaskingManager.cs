using System.Threading.Tasks;

namespace FixMe.Manager.Tasking.Interface
{
    public interface ITaskingManager
    {
        Task<CreateBackofficeTaskResponse> CreateBackofficeTask(CreateBackofficeTaskRequest request);
        Task<ConfirmMaintenanceProviderSlotsResponse> ConfirmMaintenanceProviderSlots(ConfirmMaintenanceProviderSlotsRequest request);
    }
}