namespace SzakemberKereso.DTOs.Expert
{
    public class ExpertFilterParams
    {
        public int? OccupationId { get; set; }
        public decimal? MinFixedPrice { get; set; }
        public decimal? MaxFixedPrice { get; set; }
        public bool AllowUnitBased { get; set; } = true;
        public string? UnitName { get; set; }

        public string? CountyName { get; set; }
        public string? SettlementName { get; set; }
    }
}
