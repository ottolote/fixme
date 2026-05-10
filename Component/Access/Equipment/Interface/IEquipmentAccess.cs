using System.Threading.Tasks;
using FixMe.Access.Equipment.Interface.Common;

namespace FixMe.Access.Equipment.Interface
{
    public interface IEquipmentAccess
    {
        Task<FilterResponseBase> Filter(FilterRequestBase request);
        Task<StoreResponseBase> Store(StoreRequestBase request);
    }
}
