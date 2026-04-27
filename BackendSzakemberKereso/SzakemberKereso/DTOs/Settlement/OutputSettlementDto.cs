namespace SzakemberKereso.DTOs.Settlement
{
    public class OutputSettlementDto
    {
        public int Id { get; set; }
        public int PostalCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CountyName { get; set; } = string.Empty;
    }
}
