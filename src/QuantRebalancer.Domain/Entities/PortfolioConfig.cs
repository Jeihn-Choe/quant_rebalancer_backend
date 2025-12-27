namespace QuantRebalancer.Domain.Entities
{
    public class PortfolioConfig
    {
        public int Id { get; set; }

        public decimal UsRatio { get; set; }
        public decimal KrRatio { get; set; }
        public decimal CoinRatio { get; set; }
        public decimal GoldRatio { get; set; }
        public decimal BondRatio { get; set; }

        public decimal Threshold { get; set; }
    }
}
