using System.Threading.Tasks;

namespace FixMe.Access.Equipment.Interface
{
    public interface IEquipmentAccess
    {
        Task<Equipment?> Filter(EquipmentCriteria request);
        Task<EquipmentType?> Filter(EquipmentTypeCriteria request);
        Task<PendingRegistration?> Filter(PendingRegistrationCriteria request);
        Task<PendingRegistration> Store(PendingRegistration request);
        Task<Equipment> Store(Equipment request);
    }
}
