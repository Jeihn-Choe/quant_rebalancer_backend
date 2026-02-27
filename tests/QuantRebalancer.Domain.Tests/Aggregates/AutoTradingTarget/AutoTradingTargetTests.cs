using QuantRebalancer.Domain.ValueObjects;

namespace QuantRebalancer.Domain.Tests.Aggregates.AutoTradingTarget
{
    public class AutoTradingTargetTests
    {

        [Fact]
        public void Should_Create_With_Valid_Values()
        {
            // Given
            var ticker = new Ticker();
            var isEnabled = true;

            // When
            var autoTradingTarget = new AutoTradingTarget(ticker, isEnabled);

            // Then

        }




    }
}