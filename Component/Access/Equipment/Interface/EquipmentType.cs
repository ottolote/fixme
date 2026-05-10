using FixMe.Access.Equipment.Interface.Common;

namespace FixMe.Access.Equipment.Interface
{
    public class EquipmentType
    {
        public string? EquipmentTypeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsSupported { get; set; } = true;
    }
}
