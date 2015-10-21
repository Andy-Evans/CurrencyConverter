using System;
using System.Data;
using System.IO;

namespace CurrencyConverter
{
    internal class Program
    {
        public static string Country { get; set; }
        public static double Amount { get; set; }
        public static double Result { get; set; }
        public static double Fee { get; set; }

        private static void Main(string[] args)
        {
            while (true)
            {

                Console.Write("Amount (GBP) : ");
                Amount = Convert.ToDouble(Console.ReadLine());

                Console.Write("CountryCode (USD,EUR,JPY) : ");
                Country = Console.ReadLine();

                if (Amount > 10000)
                {
                    Console.WriteLine("Amount exceeds max limit");
                }
                else
                {
                    DoConversion();

                    Console.WriteLine("Amount : " + Result + " " + Country);
                    Console.WriteLine("Fee (GBP) = " + Fee);

                    var total = Amount + Fee;
                    Console.WriteLine("Total (GBP) = " + total);
                }

                Console.Write("Continue ? y/n : ");
                if (Console.ReadLine() == "n")
                {
                    return;
                }
                Console.WriteLine();
            }
        }

        public static void DoConversion()
        {
            // Get the data
            var data = GetData();
            double charge = 0;

            // Figure out our charge for this transaction
            var charges = data.Tables[0].Rows;

            bool foundIt = false;

            foreach (DataRow row in charges)
            {
                if (!foundIt && Amount <= Convert.ToDouble(row[0]))
                {
                    charge = Convert.ToDouble(row[1]);
                    foundIt = true;
                }
            }

            double rate = 0;
            foundIt = false;

            // Figure out the conversion rate
            var rates = data.Tables[1].Rows;

            foreach (DataRow row in rates)
            {
                if (!foundIt && Country == row[0].ToString())
                {
                    rate = Convert.ToDouble(row[1]);
                    foundIt = true;
                }
            }

            if (rate == 0)
            {
                Console.WriteLine("Cannot find conversion rate for country : " + Country);
                return;
            }

            // Do the calculations
            Result = Amount * rate;
            Fee = Amount * (charge / 100);
        }

        public static DataSet GetData()
        {
            var charges = File.ReadAllLines("Charges.txt");

            DataTable table1 = new DataTable("charges");
            table1.Columns.Add("limit");
            table1.Columns.Add("percentage");

            foreach (var charge in charges)
            {
                var row = charge.Split(' ');
                table1.Rows.Add(row[0], row[1]);
            }

            var rates = File.ReadAllLines("Rates.txt");
            DataTable table2 = new DataTable("rates");
            table2.Columns.Add("countryCode");
            table2.Columns.Add("rate");

            foreach (var rate in rates)
            {
                var row = rate.Split(' ');
                table2.Rows.Add(row[0], row[1]);
            }

            DataSet ratesAndCharges = new DataSet("RatesAndCharges");
            ratesAndCharges.Tables.Add(table1);
            ratesAndCharges.Tables.Add(table2);

            return ratesAndCharges;
        }
    }
}
