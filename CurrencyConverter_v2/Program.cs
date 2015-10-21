using System;
using Converter;

namespace CurrencyConverter_v2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Amount (GBP) : ");
                var amount = Convert.ToDouble(Console.ReadLine());

                Console.Write("CountryCode (USD,EUR,JPY) : ");
                var country = Console.ReadLine();

                var currencyConverter = new CurrencyConversion(new CurrencyConversion.RatesAndChargesRepository());
                
                if (currencyConverter.CanConvert(amount))
                {
                    var result = currencyConverter.DoConversion(amount, country);

                    Console.WriteLine("Amount : " + result.Result + " " + country);
                    Console.WriteLine("Fee (GBP) = " + result.Fee);
                    Console.WriteLine("Total (GBP) = " + result.Total);
                }
                else
                {
                    Console.WriteLine("Amount exceeds max limit");
                }

                Console.Write("Continue ? y/n : ");

                if (Console.ReadLine() == "n")
                {
                    return;
                }
                
                Console.WriteLine();
            }
        }
    }
}
