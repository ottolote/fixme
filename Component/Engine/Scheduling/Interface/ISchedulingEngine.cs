using System.Threading.Tasks;

namespace FixMe.Engine.Scheduling.Interface
{
    public interface ISchedulingEngine
    {
        Task<CreateMaintenanceJobSlotsProposalResponse> CreateMaintenanceJobSlotsProposal(CreateMaintenanceJobSlotsProposalRequest request);
        Task<AcceptMaintenanceSlotsProposalResponse> AcceptMaintenanceSlotsProposal(AcceptMaintenanceSlotsProposalRequest request);
    }
}
