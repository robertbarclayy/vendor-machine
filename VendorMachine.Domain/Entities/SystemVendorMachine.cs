using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VendorMachine.Domain.Entities
{
    public class SystemVendorMachine
    {
        public SystemVendorMachine()
        {
            Products = new List<Product>()
            {
                new Product("Coke", (decimal) 1.50, 10),
                new Product("Water", (decimal) 1.00,10),
                new Product("Pastelina", (decimal) 0.30, 10)
            };

            Coins = new List<Coin>()
            {
                new Coin((decimal) 0.01),
                new Coin((decimal) 0.05),
                new Coin((decimal) 0.10),
                new Coin((decimal) 0.25),
                new Coin((decimal) 0.50),
                new Coin((decimal) 1.00)
            };

            InitialInputCoins = string.Empty;
            InitialInputProducts = string.Empty;

            CHANGE = false;
        }
        public bool IsValid()
        {
            if (GetInputCoin() == null || GetInputProduct() == null)
                return false;

            foreach (Coin coin in GetInputCoin())
            {
                if (!Coins.Contains(coin))
                {
                    return false;
                }
            }

            foreach (string product in GetInputProduct())
            {
                if (product.Equals("CHANGE")) continue;

                if (!Products.Contains(new Product(product, (decimal) 0, 0)))
                {
                    return false;
                }
            }

            return true;
        }
        public void CreateProductInput()
        {
            int countCoke = InitialInputProducts.Split("Coke").Length - 1;
            int countWater = InitialInputProducts.Split("Water").Length - 1;
            int countPastelina = InitialInputProducts.Split("Pastelina").Length - 1;
            int countChange = InitialInputProducts.Split("CHANGE").Length - 1;

            if (countChange > 0) CHANGE = true;


            if (countCoke > 0)
            {
                ProductSelected = new Product("Coke", EnteredValue(), countCoke);
            }
            else if (countWater > 0)
            {
                ProductSelected = new Product("Water", EnteredValue(), countWater);
            }
            else
            {
                ProductSelected = new Product("Pastelina", EnteredValue(), countPastelina);
            }
        }
        public void InputSystem(string value)
        {
            InitialInputCoins = RegexRemoveProducts(value);
            InitialInputProducts = RegexRemoveCoins(value);
        }
        public string OutputSystem()
        {
            //If there are not enough coins to provide the change the vendor machine should write to the standard output NO_COINS.
            if (!sufficientInputValue())
            {
                return WriteOutputSystem("NO_COINS");
            }

            //If the product is not available, the vendor machine should write to standard output. NO_PRODUCT
            if (!haveQuantityInstock()) //falta teste
            {
                return WriteOutputSystem("NO_PRODUCT");
            }

            return MoneyChange();
        }
        private string MoneyChange()
        {
            return ProductSelected.Amount > 1 ? ManyProductMoneyChangeLocalize() : OneProductMoneyChangeLocalize();
        }
        private string ManyProductMoneyChangeLocalize()
        {
            string output = string.Empty;

            decimal enteredValue = EnteredValue();

            for (int i = 0; i != ProductSelected.Amount; i++)
            {
                enteredValue = enteredValue - getProductListState.Price;

                getProductListState.Amount = getProductListState.Amount - 1;

                output += $"{ProductSelected.Name} = {enteredValue} ";
            }

            return WriteOutputSystem(output);
        }
        private string OneProductMoneyChangeLocalize()
        {
            decimal moneychange = EnteredValue() - amountProductPriceSelected(getProductListState, ProductSelected);

            getProductListState.Amount = getProductListState.Amount - ProductSelected.Amount;

            if (moneychange == 0)
            {
                return WriteOutputSystem($"{ProductSelected.Name} = {moneychange}" + (CHANGE ? " NO_CHANGE" : ""));
            }


            if (Coins.Contains(new Coin(moneychange)))
            {
                return WriteOutputSystem($"{ProductSelected.Name} = {moneychange}");
            }

            string output = $"Pastelina = {moneychange}";

            while (moneychange != 0)
            {
                if (moneychange > (decimal)1.00)
                {
                    moneychange = moneychange - (decimal)1.00;
                    output += " 1.00";
                }
                else if (moneychange < (decimal)1.00 && moneychange >= (decimal)0.50)
                {
                    moneychange = moneychange - (decimal)0.50;
                    output += " 0.50";
                }
                else if (moneychange < (decimal)0.50 && moneychange >= (decimal)0.25)
                {
                    moneychange = moneychange - (decimal)0.25;
                    output += " 0.25";
                }
                else if (moneychange < (decimal)0.25 && moneychange >= (decimal)0.10)
                {
                    moneychange = moneychange - (decimal)0.10;
                    output += " 0.10";
                }
                else if (moneychange < (decimal)0.10 && moneychange >= (decimal)0.05)
                {
                    moneychange = moneychange - (decimal)0.05;
                    output += " 0.05";
                }
                else if (moneychange < (decimal)0.05 && moneychange >= (decimal)0.01)
                {
                    moneychange = moneychange - (decimal)0.01;
                    output += " 0.01";
                }
            }

            return WriteOutputSystem(output);
        }
        private Product getProductListState => Products.FirstOrDefault(x => x.Name.Equals(ProductSelected.Name));
        private bool haveQuantityInstock()
        {
            return (ProductSelected.Amount <= getProductListState.Amount);
        }
        public string WriteOutputSystem(string value)
        {
            return ($"Output: {value}").Replace(",", ".").TrimStart().TrimEnd();
        }
        public bool sufficientInputValue()
        {
            return (EnteredValue() >= amountProductPriceSelected(getProductListState, ProductSelected));
        }
        public decimal amountProductPriceSelected(Product product, Product currentProduct)
        {
            return product.Price * currentProduct.Amount;
        }
        public string RegexRemoveCoins(string value)
        {
            return NormalizeRegex(Regex.Replace(value, "[^a-z A-Z]+", ""));
        }
        public string RegexRemoveProducts(string value)
        {
            return NormalizeRegex(Regex.Replace(value, "[a-zA-Z]+", ""));
        }
        public string NormalizeRegex(string value)
        {
            return (Regex.Replace(value, "^ +| +$|( )+", " ")).TrimStart().TrimEnd();
        }
        public List<Coin> GetInputCoin()
        {
            if (string.IsNullOrEmpty(InitialInputCoins))
                return null;

            return (from x in InitialInputCoins.Split(' ')
                    select new Coin(x)).ToList();

        }
        public List<string> GetInputProduct()
        {
            if (string.IsNullOrEmpty(InitialInputProducts))
                return null;

            return (from x in InitialInputProducts.Split(' ')
                    select x).ToList();

        }
        public List<Product> Products { get; }
        public List<Coin> Coins { get; }
        public decimal EnteredValue() => GetInputCoin().Sum(x => x.value);
        public string InitialInputCoins { get; private set; }
        public string InitialInputProducts { get; private set; }
        public Product ProductSelected { get; private set; }
        public bool CHANGE { get; set; }
    }
}
