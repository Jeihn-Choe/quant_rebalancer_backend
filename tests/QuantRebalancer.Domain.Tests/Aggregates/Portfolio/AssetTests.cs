using FluentAssertions;
using QuantRebalancer.Domain.Aggregates.Portfolio;
using QuantRebalancer.Domain.Enums;
using QuantRebalancer.Domain.ValueObjects;

public class AssetTests
{
    [Fact]
    public void Should_Create_Asset_With_Valid_Values()
    {
        // Given
        var ticker = new Ticker("AAPL");
        var assetType = AssetType.UsStock;
        var currentValue = new Money(1_000_000m, "KRW");
        var isAutoTarget = true;

        // When
        var asset = new Asset(ticker, assetType, currentValue, isAutoTarget);

        // Then
        asset.Ticker.Should().Be(ticker);
        asset.AssetType.Should().Be(assetType);
        asset.CurrentValue.Should().Be(currentValue);
        asset.IsAutoTarget.Should().Be(isAutoTarget);
    }

    [Fact]
    public void Should_Throw_When_Ticker_Is_Default()
    {
        // Given
        var ticker = default(Ticker);
        var assetType = AssetType.UsStock;
        var currentValue = new Money(1_000_000m, "KRW");
        var isAutoTarget = true;
        // When
        var act = () => new Asset(ticker, assetType, currentValue, isAutoTarget);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("ticker");

    }

    [Fact]
    public void Should_Throw_When_Current_Value_Is_Default()
    {
        // Given
        var ticker = new Ticker("AAPL");
        var assetType = AssetType.UsStock;
        var currentValue = default(Money);
        var isAutoTarget = true;

        // When
        var act = () => new Asset(ticker, assetType, currentValue, isAutoTarget);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("currentValue");
    }

    [Fact]
    public void Should_Update_Current_Value()
    {
        // Given
        var asset = new Asset(
            new Ticker("AAPL"),
            AssetType.UsStock,
            new Money(1_000_000m, "KRW"),
            true);

        var newValue = new Money(2_000_000m, "KrW");
        // When
        asset.UpdateCurrentValue(newValue);

        // Then
        asset.CurrentValue.Should().Be(new Money(2_000_000m, "KRW"));
    }

    [Fact]
    public void Should_Throw_Current_Value_Is_Default()
    {
        // Given
        var asset = new Asset(
            new Ticker("AAPL"),
            AssetType.UsStock,
            new Money(1_000_000m, "KRW"),
            true);

        var newValue = default(Money);

        // When
        var act = () => asset.UpdateCurrentValue(newValue);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("newValue");
    }
}