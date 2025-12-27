using QuantRebalancer.Domain.Enums;

namespace QuantRebalancer.Domain.Services
{
    public class PortfolioCalculator
    {
        // 결과값을 담을 작은 구조체
        public record SectorResult(decimal Amount, decimal Ratio);

        public Dictionary<AssetType, SectorResult> CalculateCurrentPortfolio(List<(AssetType Type, decimal Amount)> assets)
        {
            // 1. 전체 자산 총액 계산
            decimal totalAmount = assets.Sum(a => a.Amount);
            if (totalAmount == 0) return new Dictionary<AssetType, SectorResult>();

            // 2. 자산 타입별로 그룹화하여 합산 및 비중 계산
            return assets
                .GroupBy(x => x.Type)
                .ToDictionary(group => group.Key,
                group =>
                {
                    decimal sectorSum = group.Sum(x => x.Amount);
                    decimal ratio = sectorSum / totalAmount;
                    return new SectorResult(sectorSum, ratio);
                });
        }
    }
}
