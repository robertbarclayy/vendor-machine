using System;
using System.Globalization;

namespace VendorMachine.Domain.Entities
{
    public class Coin : IEquatable<Coin>
    {
        public Coin(string value)
        {
            decimal change = decimal.Parse(value, CultureInfo.InvariantCulture);

            this.value = Decimal.Round(change, 2, MidpointRounding.AwayFromZero);

        }
        public Coin(decimal value)
        {
            this.value = Decimal.Round((decimal)value, 2, MidpointRounding.AwayFromZero); ;
        }

        public decimal value { get; private set; }

        public bool Equals(Coin other)
        {
            if (other == null)
            {
                return false;
            }

            return value.Equals(other.value);
        }
    }
}
