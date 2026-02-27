using QuantRebalancer.Domain.ValueObjects;

namespace QuantRebalancer.Domain.Aggregates.AutoTradingTarget
{
    public sealed class AutoTradingTarget
    {
        public Ticker Ticker { get; }
        public bool IsEnabled { get; private set; }


        public AutoTradingTarget(Ticker ticker, bool isEnabled)
        {
            if (ticker.Equals(default))
                throw new ArgumentException("Ticker 는 필수입니다.", nameof(ticker));

            Ticker = ticker;
            IsEnabled = isEnabled;
        }

        public void SetEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

    }
}