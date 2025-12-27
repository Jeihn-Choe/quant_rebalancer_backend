using QuantRebalancer.Domain.Enums;

namespace QuantRebalancer.Domain.Entities
{
    public class TickerMaster
    {
        // PK: 종목코드
        public required string Code { get; set; }

        public AssetType Type { get; set; }
        public required string Name { get; set; }
        public required string TradeMarket { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
