// Value Object (record struct)
namespace OSLite.Domain
{
    public readonly record struct Money
    {
        public decimal Value { get; init; }

        public Money(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "O valor monetário não pode ser negativo.");
            }
            Value = value;
        }

        // Sobrecarga de operadores para facilitar cálculos
        public static Money operator +(Money left, Money right) => new(left.Value + right.Value);
        public static Money operator *(Money left, int multiplier) => new(left.Value * multiplier);
    }
}