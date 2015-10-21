using System;
using System.Data;
using System.IO;
using CurrencyConverter_v2;

namespace Converter
{
    public class CurrencyConversion
    {
        private readonly IRatesAndChargesRepository _ratesAndChargesRepository;

        public CurrencyConversion(IRatesAndChargesRepository ratesAndChargesRepository)
        {
            _ratesAndChargesRepository = ratesAndChargesRepository;
        }

        public bool CanConvert(double amount)
        {
            return (amount <= 1000);
        }

        public ConversionResult DoConversion(double amount, string country)
        {
            // Get the data
            var data = _ratesAndChargesRepository.GetData();
            double charge = 0;

            // Figure out our charge for this transaction
            var charges = data.Tables[0].Rows;

            bool foundIt = false;

            foreach (DataRow row in charges)
            {
                if (!foundIt && amount <= Convert.ToDouble(row[0]))
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
                if (!foundIt && country == row[0].ToString())
                {
                    rate = Convert.ToDouble(row[1]);
                    foundIt = true;
                }
            }

            if (rate == 0)
            {
                Console.WriteLine("Cannot find conversion rate for country : " + country);
                return null;
            }

            var result = amount*rate;
            var fee = amount*(charge/100);
            var total = amount + fee;

            // Do the calculations
            return new ConversionResult
            {
                Fee = fee,
                Result = result,
                Total = total
            };
        }


        public interface IRatesAndChargesRepository
        {
            DataSet GetData();
        }

        public class RatesAndChargesRepository : IRatesAndChargesRepository
        {
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
}