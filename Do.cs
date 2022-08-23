using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Currency_Calc
{
    /// <summary>
    /// Модель курс доллара
    /// </summary>
    public class Do : PropertyChangedBase
    {
        protected double usd, eur, rub, val, fromvalue, tovalue;
        protected string valuta, to, from;

        public double USD
        {
            get
            {
                using (RatesDbEntitiesEntities1 DB = new RatesDbEntitiesEntities1())
                {
                    GetRate();
                    var value = DB.Value.Where(c => c.Val != 0 && c.Number == 1).FirstOrDefault();
                    if (value == null)
                    {
                        return 0;
                    }
                    return (double)value.Val;
                }
            }
            set
            {
                usd = value;
                OnPropertyChanged("Value");
            }
        }
        public double EUR
        {
            get
            {
                using (RatesDbEntitiesEntities1 DB = new RatesDbEntitiesEntities1())
                {
                    var value = DB.Value.Where(c => c.Val != 0 && c.Number == 2).FirstOrDefault();
                    if (value == null)
                    {
                        return 0;
                    }
                    return (double)value.Val;
                }
            }
            set
            {
                eur = value;
                OnPropertyChanged("Value");
            }
        }
        public double RUB
        {
            get
            {
                using (RatesDbEntitiesEntities1 DB = new RatesDbEntitiesEntities1())
                {
                    GetRate();
                    var value = DB.Value.Where(c => c.Val != 0 && c.Number == 3).FirstOrDefault();
                    if (value == null)
                    {
                        return 0;
                    }
                    return (double)value.Val;
                }
            }
            set
            {
                rub = value;
                OnPropertyChanged("Value");
            }
        }
        public double Val
        {
            get
            {
                ValutaGetRate();
                return val;
            }
            set
            {
                val = value;
            }
        }
        public string Valuta
        {
            get
            {
                return valuta;
            }
            set
            {
                valuta = value;
            }
        }
        public double FromValue
        {
            get
            {
                return fromvalue;
            }
            set
            {
                fromvalue = value;
            }
        }
        public double ToValue
        {
            get
            {
                return tovalue;
            }
            set
            {
                tovalue = value;
            }
        }
        public string To
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
            }
        }
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
            }
        }

        public static class Document
        {
            public static XDocument xdoc = XDocument.Load(uri: "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange");
        }
        
        public async Task GetRate()
        {
            using (RatesDbEntitiesEntities1 DB = new RatesDbEntitiesEntities1())
            {
                try
                {
                    NumberFormatInfo formatProvider = new NumberFormatInfo();
                    formatProvider.NumberDecimalSeparator = ".";

                    XElement root = Document.xdoc.Element("exchange");
                    if (root == null)
                        return;
                    foreach (XElement currency in root.Elements("currency"))
                    {
                        XElement r030 = currency.Element("r030");

                        if (Convert.ToInt32(r030.Value) == 840)
                        {
                            XElement rate = currency.Element("rate");
                            USD = Convert.ToDouble(rate.Value, formatProvider);
                            Rate rate1 = new Rate();
                            Value value1 = new Value();
                            string value = currency.Element("cc").Value;
                            var Val = DB.Rate.Where(s => s.Name == value).FirstOrDefault();
                            if (null == Val)
                            {
                                rate1.Name = Convert.ToString(currency.Element("cc").Value);
                                value1.Val = Convert.ToDouble(rate.Value, formatProvider);
                                value1.Number = 1;
                                DB.Rate.Add(rate1);
                                DB.Value.Add(value1);
                                DB.SaveChanges();
                            }
                        }
                        if (Convert.ToInt32(r030.Value) == 978)
                        {
                            XElement rate = currency.Element("rate");
                            EUR = Convert.ToDouble(rate.Value, formatProvider);
                            Rate rate1 = new Rate();
                            Value value1 = new Value();
                            string value = currency.Element("cc").Value;
                            var Val = DB.Rate.Where(s => s.Name == value).FirstOrDefault();
                            if (null == Val)
                            {
                                rate1.Name = Convert.ToString(currency.Element("cc").Value);
                                value1.Val = Convert.ToDouble(rate.Value, formatProvider);
                                value1.Number = 2;
                                DB.Rate.Add(rate1);
                                DB.Value.Add(value1);
                                DB.SaveChanges();
                            }
                        }
                        if (Convert.ToInt32(r030.Value) == 643)
                        {
                            XElement rate = currency.Element("rate");
                            RUB = double.Parse(rate.Value, formatProvider);
                            Rate rate1 = new Rate();
                            Value value1 = new Value();
                            string value = currency.Element("cc").Value;
                            var Val = DB.Rate.Where(s => s.Name == value).FirstOrDefault();
                            if (null == Val)
                            {
                                rate1.Name = Convert.ToString(currency.Element("cc").Value);
                                value1.Val = Convert.ToDouble(rate.Value, formatProvider);
                                value1.Number = 3;
                                DB.Rate.Add(rate1);
                                DB.Value.Add(value1);
                                DB.SaveChanges();
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            
        }

        public async Task ValutaGetRate()
        {
            using (RatesDbEntitiesEntities1 DB = new RatesDbEntitiesEntities1())
            {
                try
                {
                    NumberFormatInfo formatProvider = new NumberFormatInfo();
                    formatProvider.NumberDecimalSeparator = ".";
                    XElement root = Document.xdoc.Element("exchange");
                    if (root == null)
                        return;
                    foreach (XElement currency in root.Elements("currency"))
                    {
                        Rate rate1 = new Rate();
                        Value value1 = new Value();
                        XElement cc = currency.Element("cc");
                        if (Convert.ToString(cc.Value) == Valuta)
                        {
                            XElement rate = currency.Element("rate");
                            Val = double.Parse(rate.Value, formatProvider);
                            string value = currency.Element("cc").Value;
                            var Val1 = await DB.Rate.Where(s => s.Name == value).FirstOrDefaultAsync();
                            if (null == Val1)
                            {
                                rate1.Name = Convert.ToString(currency.Element("cc").Value);
                                value1.Val = Convert.ToDouble(rate.Value, formatProvider);
                            }
                        }
                        if (Convert.ToString(cc.Value) == To)
                        {
                            XElement rate = currency.Element("rate");
                            ToValue = double.Parse(rate.Value, formatProvider);
                            string value = currency.Element("cc").Value;
                            var Val1 = DB.Rate.Where(s => s.Name == value).FirstOrDefault();
                            if (null == Val1)
                            {
                                rate1.Name = Convert.ToString(currency.Element("cc").Value);
                                value1.Val = Convert.ToDouble(rate.Value, formatProvider);
                            }
                        }
                        if (Convert.ToString(cc.Value) == From)
                        {
                            XElement rate = currency.Element("rate");
                            FromValue = double.Parse(rate.Value, formatProvider);
                            string value = currency.Element("cc").Value;
                            var Val1 = DB.Rate.Where(s => s.Name == value).FirstOrDefault();
                            if (null == Val1)
                            {
                                rate1.Name = Convert.ToString(currency.Element("cc").Value);
                                value1.Val = Convert.ToDouble(rate.Value, formatProvider);
                            }
                        }
                        if (rate1.Name != null)
                        {
                            DB.Rate.Add(rate1);
                            DB.Value.Add(value1);
                            DB.SaveChanges();
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
