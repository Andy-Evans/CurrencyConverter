using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                var converter = new CurrencyConverter();

                Console.Write("Amount : ");
                converter.Amount = Convert.ToDouble(Console.ReadLine());

                Console.Write("CountryCode : ");
                converter.Country = Console.ReadLine();

                if (converter.Amount > 10000)
                {
                    Console.WriteLine("Amount exceeds max limit");
                }
                else
                {
                    converter.Convert();

                    Console.WriteLine("Amount = " + converter.Result);
                    Console.WriteLine("Fee = " + converter.Fee);

                    var total = converter.Amount + converter.Fee;
                    Console.WriteLine("Total = " + total);
                }

                Console.Write("Continue y/n : ");
                if (Console.ReadLine() == "n")
                {
                    return;
                }
                Console.WriteLine();
            }
        }
    }
}
