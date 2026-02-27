using QuantRebalancer.Domain.Enums;
using QuantRebalancer.Domain.ValueObjects;

namespace QuantRebalancer.Domain.Aggregates.Portfolio
{
    public sealed class Asset
    {
        public Ticker Ticker { get; }
        public AssetType AssetType { get; }
        public Money CurrentValue { get; private set; }

        public Asset(Ticker ticker, AssetType assetType, Money currentValue)
        {
            if (ticker.Equals(default))
                throw new ArgumentException("Ticker는 필수입니다.", nameof(ticker));

            if (currentValue.Equals(default))
                throw new ArgumentException("CurrentValue는 필수입니다.", nameof(currentValue));

            Ticker = ticker;
            AssetType = assetType;
            CurrentValue = currentValue;
        }

        public void UpdateCurrentValue(Money newValue)
        {
            if (newValue.Equals(default))
                throw new ArgumentException("newValue는 필수입니다.", nameof(newValue));

            CurrentValue = newValue;
        }

    }
}