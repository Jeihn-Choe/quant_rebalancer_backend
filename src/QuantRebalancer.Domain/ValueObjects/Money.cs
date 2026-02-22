namespace QuantRebalancer.Domain.ValueObjects;

public readonly record struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Money 는 음수일 수 없습니다.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("currency 는 필수입니다.", nameof(currency));

        Amount = decimal.Round(amount, 2, MidpointRounding.ToZero);
        Currency = currency.Trim().ToUpperInvariant();

    }

    public static Money Zero(string currency) => new(0m, currency);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        var result = Amount - other.Amount;
        if (result < 0)
            throw new InvalidOperationException("Money 결과가 음수가 될 수 없음");
        return new Money(result, Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (!Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"통화 불일치 : {Currency} != {other.Currency}");
    }

}