using System.Threading.Tasks;
using FixMe.Access.Agreement.Interface;
using AgreementModel = FixMe.Access.Agreement.Interface.Agreement;

namespace FixMe.Access.Agreement.Service
{
    public class AgreementAccess : IAgreementAccess
    {
        private readonly AgreementResource resource;

        public AgreementAccess()
            : this(AgreementResource.Shared)
        {
        }

        public AgreementAccess(AgreementResource resource)
        {
            this.resource = resource;
        }

        public Task<AgreementModel?> Filter(AgreementCriteria request)
        {
            return resource.Filter(request);
        }

        public Task<AgreementModel> Store(AgreementModel request)
        {
            return resource.Store(request);
        }
    }
}
