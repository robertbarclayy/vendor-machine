using System;
using VendorMachine.Domain.Entities;

namespace VendorMachine.System
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();

            while (!vendorMachine.IsValid())
            {
                Console.WriteLine("Coins are 0.01, 0.05, 0.10, 0.25, 0.50 and 1.00");
                Console.WriteLine("Products are Coke (cost 1.50), Water (cost: 1.00) and Pastelina (cost: 0.30)");
                Console.Write("Input: ");
                vendorMachine.InputSystem(Console.ReadLine());
            }

            vendorMachine.CreateProductInput();

            vendorMachine.OutputSystem();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
