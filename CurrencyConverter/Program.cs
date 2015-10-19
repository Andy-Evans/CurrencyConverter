using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var converter = new CurrencyConverter();

            converter.Country = "EUR";
            converter.Amount = 250.50;

            converter.Convert();

            Console.WriteLine("Amount = " + converter.Result);
            Console.WriteLine("Charge = " + converter.Charge);

            Console.ReadKey();
        }
    }
}
