using System.Threading.Tasks;

namespace FixMe.Manager.Membership.Interface
{
    public interface IMembershipManager
    {
        Task<RegisterAccountResponse> RegisterAccount(RegisterAccountRequest request);
        Task<ConfirmUserEmailResponse> ConfirmUserEmail(ConfirmUserEmailRequest request);
        Task<UpdateUserPasswordResponse> UpdateUserPassword(UpdateUserPasswordRequest request);
        Task<SetUserPreferencesResponse> SetUserPreferences(SetUserPreferencesRequest request);
        Task<PendingRegistration> CreatePendingRegistration(CreatePendingRegistrationRequest request);
        Task<ResolvePendingRegistrationResponse> ResolvePendingRegistration(ResolvePendingRegistrationRequest request);
    }
}