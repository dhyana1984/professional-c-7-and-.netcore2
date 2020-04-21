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

        public override string ToString() =>
            $"${Dollars}.{Cents,-2:00}";

        //implicit隐式转换为float
        public static implicit operator float(Currency value) =>
            value.Dollars+(value.Cents/100);
        //
        public static explicit operator Currency(float value)
        {
            checked
            {
                uint dollars = (uint)value;
                ushort cents = Convert.ToUInt16((value - dollars) * 100);
                return new Currency(dollars, cents);
            }
        }
            
    }
}
