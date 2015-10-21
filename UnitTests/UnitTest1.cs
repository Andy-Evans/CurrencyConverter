using System.Data;
using Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanConvertUnderLimit()
        {
            var currencyConverter = new Converter.CurrencyConversion(null);

            var result = currencyConverter.CanConvert(999);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CannotConvertOverLimit()
        {
            var currencyConverter = new Converter.CurrencyConversion(null);

            var result = currencyConverter.CanConvert(10001);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Convert150GBP_To_EUR()
        {
            var currencyConverter = new Converter.CurrencyConversion(;

            var result = currencyConverter.DoConversion(150,"EUR");

            Assert.AreEqual(150, result.Result);
            Assert.AreEqual(150, result.Fee);
            Assert.AreEqual(150, result.Total);
        }
    }

    public class FakeRateAndChargeRepo : CurrencyConversion.IRatesAndChargesRepository
    {
        public DataSet GetData()
        {
            throw new System.NotImplementedException();
        }
    }
}
