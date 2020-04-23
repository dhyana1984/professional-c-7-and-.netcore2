using System;
namespace OperatorAndCasting
{
    public struct Currency
    {
        public uint Dollars { get; }
        public ushort Cents { get; }

        public Currency(uint dollars, ushort cents)
        {
            Dollars = dollars;
            Cents = cents;
        }

        public static string GetCurrencyUnit() => "Dollar";

        public override string ToString() =>
            $"${Dollars}.{Cents,-2:00}";

        public static explicit operator Currency(float value)
        {
            checked
            {
                uint dollars = (uint)value;
                ushort cents = Convert.ToUInt16((value - dollars) * 100);
                return new Currency(dollars, cents);
            }
        }

        public static implicit operator float(Currency value) =>
            value.Dollars +(value.Cents / 100.0f);
        
        public static implicit operator Currency (uint value) =>
            new Currency(value, 0);

        public static implicit operator uint (Currency value) =>
            value.Dollars;
            
    }
}
