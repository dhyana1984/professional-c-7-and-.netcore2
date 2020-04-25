using System;
using System.Diagnostics.CodeAnalysis;

namespace Lib.Racer
{
    public class Racer : IComparable<Racer>, IFormattable
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Country { get; }
        public int Wins { get; }

        public Racer(int id, string firstName, string lastName, string country) :
            this(id, firstName, lastName, country, wins: 0) // wins:0是默认值
        {

        }

        public Racer(int id, string firstName, string lastName, string country, int wins)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            Wins = wins;
        }

        public override string ToString() => $"{FirstName} \t{LastName}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (String.IsNullOrEmpty(format)) format = "N";
            switch (format.ToUpper())
            {
                case "N":
                    return ToString();
                case "F":
                    return FirstName;
                case "L":
                    return LastName;
                case "W":
                    return $"{ToString()}, \tWins: {Wins}";
                case "C":
                    return $"{ToString()}, \tCountry: {Country}";
                case "A": //All
                    return $"{ToString()}, \tCountry: {Country}, \tWins:{Wins}";
                default:
                    throw new FormatException(String.Format(formatProvider, $"Format {format} is not supported"));
            }
        }

        public string ToString(string format) => ToString(format, null);

        public int CompareTo([AllowNull] Racer other)
        {
            //先比较LastName
            int compare = LastName?.CompareTo(other?.LastName) ?? -1;
            //如果LastName相同就比较FirstName
            if(compare == 0)
            {
                return FirstName?.CompareTo(other?.FirstName) ?? -1;
            }
            return compare;
        }


    }
}
