using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Currency_Calc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Show Show;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Show = new Show();
            Currency.Focus();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await Show.OnLoad(sender, e);
        }

        private void Amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Amount.Text))
            {
                double num = 0;
                if (double.TryParse(Amount.Text, out num))
                {
                    if (Convert.ToInt32(Amount.Text) > 1)
                    {
                        double amn = Convert.ToDouble(Amount.Text);
                        Amount1.Content = Amount2.Content = Amount3.Content = Amount4.Content = amn;
                        XUSD.Content = Math.Round(Convert.ToDouble(USD.Content) * amn, 2) + " UAH";
                        XEUR.Content = Math.Round(Convert.ToDouble(EUR.Content) * amn, 2) + " UAH";
                        XRUB.Content = Math.Round(Convert.ToDouble(RUB.Content) * amn, 2) + " UAH";
                        XVal.Content = Math.Round(Convert.ToDouble(Show.Val) * amn, 2) + " UAH";
                        Amount1.Visibility = Amount2.Visibility = Amount3.Visibility = XUSD.Visibility = XEUR.Visibility = XRUB.Visibility = Visibility.Visible;
                        if (Valuta.Content != null)
                        {
                            Amount4.Visibility = XVal.Visibility = ValutaCost.Visibility = Visibility.Visible;
                        }
                    }
                    else if (Convert.ToInt32(Amount.Text) <= 1)
                    {
                        Amount1.Content = Amount2.Content = Amount3.Content = Amount4.Content = null;
                        XUSD.Content = XEUR.Content = XRUB.Content = XVal.Content = null;
                        Amount1.Visibility = Amount2.Visibility = Amount3.Visibility = XUSD.Visibility = XEUR.Visibility = XRUB.Visibility = Visibility.Hidden;
                    }

                }
                else
                {
                    Amount.Text = Convert.ToString(Amount.Text).Remove(Amount.Text.Length - 1);
                    Amount.SelectionStart = Amount.Text.Length;
                }
            }
            else
            {
                Amount1.Content = Amount2.Content = Amount3.Content = Amount4.Content = null;
                XUSD.Content = XEUR.Content = XRUB.Content = XVal.Content = null;
            }
        }

        private void Currency_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Currency.Text.Length == 3)
            {
                if (Currency.Text == "USD" || Currency.Text == "EUR" || Currency.Text == "RUB")
                {
                    return;
                }
                else
                {
                    Show.Valuta = Convert.ToString(Currency.Text);
                    Show.ValutaGetRate();
                    if (Show.Val != 0)
                    {
                        Valuta.Content = "1 " + Convert.ToString(Currency.Text) + " costs:";
                        ValutaCost.Content = Convert.ToString(Currency.Text) + " costs:";
                        Val.Content = Convert.ToString(Show.Val) + " UAH";
                        Val.Content = Convert.ToString(Val.Content).Replace(",", ".");
                        if (!String.IsNullOrEmpty(Amount.Text))
                        {
                            if (Convert.ToInt32(Amount.Text) > 1)
                            {
                                if (!String.IsNullOrEmpty(Amount.Text))
                                {
                                    Amount4.Content = Convert.ToDouble(Amount.Text);
                                    XVal.Content = Math.Round(Convert.ToDouble(Show.Val) * Convert.ToDouble(Amount4.Content), 2) + " UAH";
                                }
                                Valuta.Visibility = Val.Visibility = Amount4.Visibility = ValutaCost.Visibility = XVal.Visibility = Visibility.Visible;
                            }
                            else if (Convert.ToInt32(Amount.Text) <= 1)
                            {
                                Valuta.Visibility = Val.Visibility = Amount4.Visibility = ValutaCost.Visibility = XVal.Visibility = Visibility.Hidden;
                            }
                        }
                        else
                        {
                            Valuta.Visibility = Val.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        Show.Valuta = null;
                        Valuta.Content = null;
                        ValutaCost.Content = null;
                    }
                }
            }
            else if (Currency.Text.Length <= 2)
            {
                Valuta.Content = Val.Content = Amount4.Content = ValutaCost.Content = XVal.Content = null;
                Valuta.Visibility = Val.Visibility = Amount4.Visibility = ValutaCost.Visibility = XVal.Visibility = Visibility.Hidden;
                Show.Val = 0;
            }
        }

        private void Currency_Lang(object sender, RoutedEventArgs e)
        {
            InputLanguage original;
            original = InputLanguage.CurrentInputLanguage;
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN");
            var language = InputLanguage.FromCulture(culture);
            if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                InputLanguage.CurrentInputLanguage = language;
            else
                InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
        }

        private void Currency_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Currency_Lang(Currency.Text, e);
            if (e.Key >= Key.A && e.Key <= Key.Z || e.Key == Key.Tab)
            {

            }
            else
            {
                e.Handled = true;
            }
        }
    }
}