using System.Threading.Tasks;

namespace FixMe.Engine.Matching.Interface
{
    public interface IMatchingEngine
    {
        Task<MatchMaintenancePlanOfferingResponse> MatchMaintenancePlanOffering(MatchMaintenancePlanOfferingRequest request);
    }
}