using System;

namespace VendorMachine.Domain.Entities
{
    public class Product : IEquatable<Product>
    {
        public Product(string name, decimal price, int amount)
        {
            Name = name;
            Price = price;
            Amount = amount;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public bool Equals(Product other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(this.Name, other.Name);
        }
    }
}
