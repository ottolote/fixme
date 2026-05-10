using FixMe.Access.Maintenance.Interface;

namespace FixMe.Access.Maintenance.Service
{
    public sealed class MaintenanceAccess : IMaintenanceAccess
    {
        private readonly MaintenanceResource _resource;

        public MaintenanceAccess()
            : this(MaintenanceResource.Shared)
        {
        }

        public MaintenanceAccess(MaintenanceResource resource)
        {
            _resource = resource;
        }

        public Task<MaintenancePlanOffering?> MaintenancePlanOfferingFilter(MaintenancePlanOfferingCriteria request) =>
            Task.FromResult(_resource.MaintenancePlanOfferingFilter(request));

        public Task<MaintenancePlan?> MaintenancePlanFilter(MaintenancePlanCriteria request) =>
            Task.FromResult(_resource.MaintenancePlanFilter(request));

        public Task<MaintenancePlan> MaintenancePlanStore(MaintenancePlan request) =>
            Task.FromResult(_resource.MaintenancePlanStore(request));

        public Task<MaintenanceJobSlot?> MaintenanceJobSlotFilter(MaintenanceJobSlotCriteria request) =>
            Task.FromResult(_resource.MaintenanceJobSlotFilter(request));

        public Task<MaintenanceJobSlot> MaintenanceJobSlotStore(MaintenanceJobSlot request) =>
            Task.FromResult(_resource.MaintenanceJobSlotStore(request));

        public Task<MaintenanceJobSlotsProposal> MaintenanceJobSlotsProposalStore(MaintenanceJobSlotsProposal request) =>
            Task.FromResult(_resource.MaintenanceJobSlotsProposalStore(request));

        public Task<MaintenanceJobSlotsProposal?> MaintenanceJobSlotsProposalFilter(MaintenanceJobSlotsProposalCriteria request) =>
            Task.FromResult(_resource.MaintenanceJobSlotsProposalFilter(request));

        public Task<MaintenanceJob> MaintenanceJobStore(MaintenanceJob request) =>
            Task.FromResult(_resource.MaintenanceJobStore(request));

        public Task<MaintenanceJob?> MaintenanceJobFilter(MaintenanceJobCriteria request) =>
            Task.FromResult(_resource.MaintenanceJobFilter(request));
    }
}
