using FluentAssertions;
using QuantRebalancer.Domain.Enums;
using QuantRebalancer.Domain.Services;

namespace QuantRebalancer.Tests.Services
{
    public class PortfolioCalculatorTests
    {
        [Fact]
        public void CalculateCurrentAmounts_Distribute_Total_Asset_Correctly()
        {
            // 1. Arrage : 테스트에 필요한 데이터와 객체 준비
            var calculator = new PortfolioCalculator();

            // API 에서 받아왔다고 가정하는 자산 목록 (자산 타입, 현재 평가액)
            var assets = new List<(AssetType Type, decimal Amount)>
            {
                (AssetType.KR, 200m), // 삼성전자
                (AssetType.KR, 100m), // 현대차
                (AssetType.US, 500m), // 테슬라
                (AssetType.COIN, 200m), // 비트코인
            };

            // 2. ACT : 실제로 기능을 실행 ( 만들어야하는 메서드 (계산기 로직) 호출)
            // 반환값은 (자산타입) -> (현재금액, 비중) 형태의 Dictionary라고 가정
            var result = calculator.CalculateCurrentPortfolio(assets);

            // 3. Assert
            // US 검증 (500, 전체 1000 중 50%)
            result[AssetType.US].Amount.Should().Be(500m);
            result[AssetType.US].Ratio.Should().Be(0.5m);


            // KR 검증 (200+100 = 300, 전체 1000 중 30%)
            result[AssetType.KR].Amount.Should().Be(300m);
            result[AssetType.KR].Ratio.Should().Be(0.3m);

            // COIN 검증 (200, 전체 1000중 20%)
            result[AssetType.COIN].Amount.Should().Be(200m);
            result[AssetType.COIN].Ratio.Should().Be(0.2m);
        }
    }
}
