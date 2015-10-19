using System;
using System.Data;
using System.IO;

namespace CurrencyConverter
{
    public class CurrencyConverter
    {
        public string Country { get; set; }
        public double Amount { get; set; }
        public double Result { get; set; }
        public double Fee { get; set; }

        public void Convert()
        {
            // Get the data
            var data = GetData();
            double charge = 0;

            // Figure out our charge for this transaction
            var charges = data.Tables[0].Rows;

            bool foundIt = false;

            foreach (DataRow row in charges)
            {
                if (!foundIt && Amount <= System.Convert.ToDouble(row[0]))
                {
                    charge = System.Convert.ToDouble(row[1]);
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
                    rate = System.Convert.ToDouble(row[1]);
                    foundIt = true;
                }
            }

            if (rate == 0)
            {
                Console.WriteLine("Cannot find conversion rate for country : " + Country);
                return;
            }

            // Do the calculations
            Result = Amount*rate;
            Fee = Amount * (charge / 100);
        }

        public DataSet GetData()
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