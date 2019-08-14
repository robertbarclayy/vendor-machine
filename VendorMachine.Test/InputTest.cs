using VendorMachine.Domain.Entities;
using Xunit;

namespace VendorMachine.Test
{
    public class InputTest
    {
        private string VALID_ENTRY= "0.01 0.05 0.10 0.25 0.50 1.00 Coke";
        private string INVALID_ENTRY = "0.30 0.01 0.05 0.10 0.25 0.50 1.00 Coke";

        [Fact]
        public void MustSuccessfullyValidateCurrencySum()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem(VALID_ENTRY);

            Assert.True(vendorMachine.IsValid());
            Assert.Equal((decimal)1.91, (decimal)vendorMachine.EnteredValue());
        }

        [Fact]
        public void MustValidateCoinEntrySuccessfullyTest()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem(VALID_ENTRY);

            Assert.True(vendorMachine.IsValid());
        }

        [Fact]
        public void MustValidateCoinEntryErrorTest()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem(INVALID_ENTRY);

            Assert.False(vendorMachine.IsValid());
        }

        [Fact]
        public void MustValidateWithErrorTextInputProductNew()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem(string.Concat(VALID_ENTRY, " ", "ProductNew"));

            Assert.False(vendorMachine.IsValid());
        }

        [Fact]
        public void MustSuccessfullyValidateRegexCoin()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem(VALID_ENTRY);

            Assert.Equal("0.01 0.05 0.10 0.25 0.50 1.00", vendorMachine.InitialInputCoins);
        }
        
        [Fact]
        public void MustSuccessfullyValidateRegexProduct()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem(VALID_ENTRY);

            Assert.Equal("Coke", vendorMachine.InitialInputProducts);
        }

        [Fact]
        public void MustValidadeCreateProductOneAmountInputValid()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("0.01 Coke");
            vendorMachine.CreateProductInput();

            Assert.Equal("Coke", vendorMachine.ProductSelected.Name);
            Assert.Equal(1, vendorMachine.ProductSelected.Amount);
        }

        [Fact]
        public void MustValidadeCreateProductMultipleAmountInputValid()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("0.01 Coke Coke Coke Coke Coke");
            vendorMachine.CreateProductInput();

            Assert.Equal("Coke", vendorMachine.ProductSelected.Name);
            Assert.Equal(5, vendorMachine.ProductSelected.Amount);
        }

        [Fact]
        public void MustValidadeInsufficientNoCoin()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("0.01 Coke");
            vendorMachine.CreateProductInput();

            Assert.False(vendorMachine.sufficientInputValue());
        }
        
        [Fact]
        public void ShouldReturnExpectedValueWhenCalculatingQuantity()
        {
            Product product1 = new Product("Pastelina", (decimal)1.50, 10);
            Product product2 = new Product("Pastelina", (decimal)1.50, 5);

            //1.50 * 5
            SystemVendorMachine vendorMachine = new SystemVendorMachine();

            Assert.Equal((decimal) 7.5, vendorMachine.amountProductPriceSelected(product1, product2));
            Assert.True((decimal) 7.5 == vendorMachine.amountProductPriceSelected(product1, product2));
        }

        [Fact]
        public void ShouldReturnDifferentValueThanExpected()
        {
            Product product1 = new Product("Pastelina", (decimal) 1.50, 10);
            Product product2 = new Product("Pastelina", (decimal) 1.50, 5);

            SystemVendorMachine vendorMachine = new SystemVendorMachine();

            Assert.False((decimal) 8.5 == vendorMachine.amountProductPriceSelected(product1, product2));
        }

        [Fact]
        public void MustValidadeSufficientNoCoin()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("0.50 1.00 Coke");
            vendorMachine.CreateProductInput();

            Assert.True(vendorMachine.sufficientInputValue());
        }

        [Fact]
        public void ShouldReturnExpectedValueOfRegexCoins()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();

            Assert.Equal("0.50 1.00", vendorMachine.RegexRemoveProducts("0.50 Coke 1.00 Coke Coke"));
            Assert.Equal("0.50 1.00 1.00", vendorMachine.RegexRemoveProducts("0.50 Coke 1.00 1.00 Coke Coke"));
            Assert.Equal("0.50 0.50 1.00", vendorMachine.RegexRemoveProducts("0.50 Coke 0.50 1.00 Coke Coke"));
            Assert.Equal("0.50 1.00 0.05", vendorMachine.RegexRemoveProducts("0.50 Coke 1.00 0.05 Coke"));
        }

        [Fact]
        public void ShouldReturnExpectedValueOfRegexProduct()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();

            Assert.Equal("Coke Coke Coke", vendorMachine.RegexRemoveCoins("0.50 Coke 1.00 Coke Coke"));
            Assert.Equal("Coke Coke Coke", vendorMachine.RegexRemoveCoins("0.50 Coke 1.00 1.00 Coke Coke"));
            Assert.Equal("Coke Coke Coke", vendorMachine.RegexRemoveCoins("0.50 Coke 0.50 1.00 Coke Coke"));
            Assert.Equal("Coke Coke", vendorMachine.RegexRemoveCoins("0.50 Coke 1.00 0.05 Coke"));
        }

        [Fact]
        public void ShouldReturnExpectedValueOfRegexNormalize()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();

            Assert.Equal("Coke Coke Coke Coke", vendorMachine.RegexRemoveCoins("Coke   Coke Coke      Coke"));
            Assert.Equal("Coke Coke Coke Coke", vendorMachine.RegexRemoveCoins(" Coke     Coke Coke      Coke"));
            Assert.Equal("Coke Coke Coke Coke", vendorMachine.RegexRemoveCoins("Coke Coke Coke      Coke"));
            Assert.Equal("Coke Coke Coke Coke", vendorMachine.RegexRemoveCoins(" Coke   Coke    Coke   Coke"));
            Assert.Equal("Coke Coke Coke Coke", vendorMachine.RegexRemoveCoins("Coke Coke Coke Coke"));
        }

        /*
        1. Insert coins and request a product:
        a. Input: 0.50 1.00 Coke
        b. Output: Coke = 0
        */
        [Fact]
        public void InsertCoinsAndRequestProduct()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("0.50 1.00 Coke");

            Assert.True(vendorMachine.IsValid());

            vendorMachine.CreateProductInput();

            Assert.Equal("Output: Coke = 0.00", vendorMachine.OutputSystem());
        }

        /*
        2. Insert too many value for a product:
        a. Input: 1.00 Pastelina CHANGE
        b. Output: Pastelina = 0.70 0.50 0.10 0.10
        */

        [Fact]
        public void InsertTooManyValueForProduct()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("1.00 Pastelina CHANGE");

            Assert.True(vendorMachine.IsValid());

            vendorMachine.CreateProductInput();

            Assert.Equal("Output: Pastelina = 0.70 0.50 0.10 0.10", vendorMachine.OutputSystem());
        }


        /*
        3. When there is no change:
        a. Input: 0.25 0.05 Pastelina CHANGE
        b. Output: Pastelina = 0.00 NO_CHANGE
        */


        [Fact]
        public void WhenThereIsNoChange()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("0.25 0.05 Pastelina CHANGE");

            Assert.True(vendorMachine.IsValid());

            vendorMachine.CreateProductInput();

            Assert.Equal("Output: Pastelina = 0.00 NO_CHANGE", vendorMachine.OutputSystem());
        }

        [Fact]
        public void WhenThereIsNoChange2()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("0.25 0.05 0.05 Pastelina CHANGE");

            Assert.True(vendorMachine.IsValid());

            vendorMachine.CreateProductInput();

            Assert.Equal("Output: Pastelina = 0.05", vendorMachine.OutputSystem());
        }

        /*
        4. When requesting multiple products:
        a. Input: 1.00 Pastelina Pastelina Pastelina
        b. Output: Pastelina=0.70 Pastelina=0.40 Pastelina=0.10
        */

        [Fact]
        public void WhenRequestingMultipleProducts()
        {
            SystemVendorMachine vendorMachine = new SystemVendorMachine();
            vendorMachine.InputSystem("1.00 Pastelina Pastelina Pastelina");

            Assert.True(vendorMachine.IsValid());

            vendorMachine.CreateProductInput();

            Assert.Equal("Output: Pastelina = 0.70 Pastelina = 0.40 Pastelina = 0.10", vendorMachine.OutputSystem());
        }

    }
}
