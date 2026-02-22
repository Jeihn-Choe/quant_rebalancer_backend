namespace QuantRebalancer.Domain.ValueObjects;

public readonly record struct Ratio
{
    public decimal Value { get; }

    public Ratio(decimal value)
    {
        if (value < 0m || value > 1m)
            throw new ArgumentOutOfRangeException(nameof(value), "Ratio 는 0~1 범위여야 함.");

        Value = decimal.Round(value, 6, MidpointRounding.ToZero);
    }

    public static Ratio Zero => new(0m);
    public static Ratio One => new(1m);

    public static Ratio operator +(Ratio a, Ratio b) => new(a.Value + b.Value);
    public static Ratio operator -(Ratio a, Ratio b) => new(a.Value - b.Value);
    public static void EnsureTotalIsOne(IEnumerable<Ratio> ratios, decimal tolerance = 0.0001m)
    {
        var sum = ratios.Sum(x => x.Value);
        if (Math.Abs(sum - 1m) > tolerance)
            throw new InvalidOperationException($"Ratio 합계는 1이어야 합니다. 현재 :{sum}");
    }
}