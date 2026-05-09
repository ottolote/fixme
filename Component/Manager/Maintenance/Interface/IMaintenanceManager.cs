using System.Threading.Tasks;

namespace FixMe.Manager.Maintenance.Interface
{
    public interface IMaintenanceManager
    {
        Task<MatchMaintenancePlanOfferingResponse> MatchMaintenancePlanOffering(MatchMaintenancePlanOfferingRequest request);
        Task<CreatePendingMaintenancePlanResponse> CreatePendingMaintenancePlan(CreatePendingMaintenancePlanRequest request);
        Task<ResolvePendingMaintenancePlanResponse> ResolvePendingMaintenancePlan(ResolvePendingMaintenancePlanRequest request);
        Task<InitiateESigningForMaintenancePlanResponse> InitiateESigningForMaintenancePlan(InitiateESigningForMaintenancePlanRequest request);
        Task<CreateMaintenanceJobSlotsProposalResponse> CreateMaintenanceJobSlotsProposal(CreateMaintenanceJobSlotsProposalRequest request);
        Task<ConfirmMaintenanceProviderSlotsResponse> ConfirmMaintenanceProviderSlots(ConfirmMaintenanceProviderSlotsRequest request);
        Task<ConfirmMaintenanceSlotsProposalResponse> ConfirmMaintenanceSlotsProposal(ConfirmMaintenanceSlotsProposalRequest request);
        Task<AcceptMaintenanceSlotsProposalResponse> AcceptMaintenanceSlotsProposal(AcceptMaintenanceSlotsProposalRequest request);
        Task<CancelMaintenanceJobResponse> CancelMaintenanceJob(CancelMaintenanceJobRequest request);
    }
}