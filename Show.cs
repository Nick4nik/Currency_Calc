using System.Threading.Tasks;
using System.Windows;

namespace Currency_Calc
{
    public class Show : PropertyChangedBase
    {
        protected Do Do;

        public Show()
        {
            Do = new Do();
            Do.PropertyChanged += (s, e) =>
            {
                OnPropertyChanged(e.PropertyName);
            };
        }

        public async Task OnLoad(object sender, RoutedEventArgs e)
        {
            await GetRate();
        }

        public async Task GetRate()
        {
            await Do.GetRate();
        }

        public async Task ValutaGetRate()
        {
            await Do.ValutaGetRate();
        }

        public double USD
        {
            get { return Do.USD; }
        }
        public double EUR
        {
            get { return Do.EUR; }
        }
        public double RUB
        {
            get { return Do.RUB; }
        }
        public double Val
        {
            get { return Do.Val; }
            set { Do.Val = value; }
        }
        public string Valuta
        {
            set { Do.Valuta = value; }
        }

        public double FromValue
        {
            get { return Do.FromValue; }
            set { Do.FromValue = value; }
        }
        public double ToValue
        {
            get { return Do.ToValue; }
            set { Do.ToValue = value; }
        }
        public string From
        {
            get { return Do.From; }
            set { Do.From = value; }
        }
        public string To
        {
            get { return Do.To; }
            set { Do.To = value; }
        }
    }
}
