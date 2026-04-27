using SzakemberKereso.DTOs.Settlement;

namespace SzakemberKereso.DTOs.ResidentialAddress
{
    public class InputResidentialAddressDto
    {
        public InputSettlementDto Settlement { get; set; } = null!;
        public string StreetAddress { get; set; } = string.Empty; //parsed in the AddressService
    }
}
