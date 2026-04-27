using SzakemberKereso.DTOs.Settlement;

namespace SzakemberKereso.DTOs.ResidentialAddress
{
    public class OutputResidentialAddressDto
    {
        public string StreetName { get; set; } = string.Empty;
        public int BuildingNumber { get; set; }
        public string PublicAreaType { get; set; } = string.Empty;
        public OutputSettlementDto? Settlement { get; set; }
    }
}
