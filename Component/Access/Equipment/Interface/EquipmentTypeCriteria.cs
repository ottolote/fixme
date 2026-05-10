using FixMe.Access.Equipment.Interface.Common;

namespace FixMe.Access.Equipment.Interface
{
    public class EquipmentTypeCriteria : FilterRequestBase
    {
        public string? EquipmentTypeId { get; set; }
        public string? Code { get; set; }
    }
}
