using System;
using Lib.Racer;

namespace Collections
{
    public class FindCountry
    {
        private string _country;
        public FindCountry(string country) => _country = country;

        //FindCountryPredicate方法的签名和返回类型和Predicate<T>是对应的
        public bool FindCountryPredicate(Racer racer) =>
            racer?.Country == _country;
    }
}
