using QuantRebalancer.Domain.Enums;

namespace QuantRebalancer.Domain.Entities
{
    public class TradeLog
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public TradeSide Side { get; set; } // Buy/Sell
        public required string Code { get; set; } // TickerCode
        public decimal Price { get; set; } // 체결가
        public decimal Qty { get; set; } // 수량
        public required string Reason { get; set; } // 매매 사유 (e.g., "SmartFilling", "Rebalance")
    }
}
