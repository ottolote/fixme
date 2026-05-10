using System.Threading.Tasks;

namespace FixMe.Access.Maintenance.Interface
{
    public interface IMaintenanceAccess
    {
        Task<MaintenancePlanOffering> MaintenancePlanOfferingFilter(MaintenancePlanOfferingCriteria request);
        Task<MaintenancePlan> MaintenancePlanFilter(MaintenancePlanCriteria request);
        Task<MaintenancePlan> MaintenancePlanStore(MaintenancePlan request);
        Task<MaintenanceJobSlot> MaintenanceJobSlotFilter(MaintenanceJobSlotCriteria request);
        Task<MaintenanceJobSlot> MaintenanceJobSlotStore(MaintenanceJobSlot request);
        Task<MaintenanceJobSlotsProposal> MaintenanceJobSlotsProposalStore(MaintenanceJobSlotsProposal request);
        Task<MaintenanceJobSlotsProposal> MaintenanceJobSlotsProposalFilter(MaintenanceJobSlotsProposalCriteria request);
        Task<MaintenanceJob> MaintenanceJobStore(MaintenanceJob request);
        Task<MaintenanceJob> MaintenanceJobFilter(MaintenanceJobCriteria request);
    }
}
