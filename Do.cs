using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Currency_Calc
{
    /// <summary>
    /// Модель курс доллара
    /// </summary>
    public class Do : PropertyChangedBase
    {
        protected double usd, eur, rub, val;
        protected string valuta;

        public double USD
        {
            get
            {
                GetRate();
                return usd;
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
                GetRate();
                return eur;
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
                GetRate();
                return rub;
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

        public static class Document
        {
            public static XDocument xdoc = XDocument.Load(uri: "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange");
        }

        public async Task GetRate()
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
                    }
                    if (Convert.ToInt32(r030.Value) == 978)
                    {
                        XElement rate = currency.Element("rate");
                        EUR = Convert.ToDouble(rate.Value, formatProvider);
                    }
                    if (Convert.ToInt32(r030.Value) == 643)
                    {
                        XElement rate = currency.Element("rate");
                        RUB = double.Parse(rate.Value, formatProvider);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task ValutaGetRate()
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
                    XElement cc = currency.Element("cc");
                    if (Convert.ToString(cc.Value) == Valuta)
                    {
                        XElement rate = currency.Element("rate");
                        Val = double.Parse(rate.Value, formatProvider);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
