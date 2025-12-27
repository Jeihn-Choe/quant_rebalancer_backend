namespace QuantRebalancer.Domain.Entities
{
    public class AutoTradingTarget
    {
        public int Id { get; set; }

        // TickerMaster 와 연결할 외래키
        public required string TickerCode { get; set; }

        // 네비게이션 속성 (EF Core 연동용)
        public TickerMaster? Ticker { get; set; }

        // 섹터(AssetType) 내에서의 비중 (0.0 ~ 1.0)
        // ex: US 섹터 내에서의 ETF 0.5, MSFT 0.5 등
        public decimal Weight { get; set; }
        public bool IsActive { get; set; }
    }
}
