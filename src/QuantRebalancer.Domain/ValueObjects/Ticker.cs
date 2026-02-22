using System.Text.RegularExpressions;

namespace QuantRebalancer.Domain.ValueObjects;

public readonly record struct Ticker
{
    private static readonly Regex BasicPattern = new(@"^[A-Z0-9\-.]+$", RegexOptions.Compiled);

    public string Value { get; }

    public Ticker(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Ticker는 필수입니다.", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (!BasicPattern.IsMatch(normalized))
            throw new ArgumentException($"유효하지 않은 Ticker 형식입니다 : {value}", nameof(value));

        Value = normalized;
    }

    public override string ToString() => Value;
}


