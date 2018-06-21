using FinOnVatCheck;
using System.Windows;
using System.Windows.Controls;

namespace FinOnVatCheckTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VLTFinOnVatCheck check = new VLTFinOnVatCheck();
                check.Memberid = Memberid.Text;
                check.UserId = UserId.Text;
                check.Pin = Pin.Text;
                check.ProducerVatNum = ProducerVatNum.Text;
                check.VATNumber = VatNum.Text;
                check.CountryCode = ((ComboBoxItem)comboBoxCountries.SelectedItem).Tag.ToString();
                bool ret = check.CheckVat();

                labelService.Content = check.VatService;
                if (ret)
                {
                    imageValid.Visibility = Visibility.Visible;
                    imageInvalid.Visibility = Visibility.Hidden;

                    labelAddress.Visibility = Visibility.Visible;
                    labelCompanyName.Visibility = Visibility.Visible;
                    //labelRequestDate.Visibility = Visibility.Visible;

                    labelAddress.Content = check.Address;
                    labelCompanyName.Content = check.Name;

                    //labelRequestDate.Content = check.RetDate.ToShortDateString();
                    //textBoxVatNumber.Text = check.VATNumber;
                }
                else
                {
                    imageValid.Visibility = Visibility.Hidden;
                    imageInvalid.Visibility = Visibility.Visible;

                    labelAddress.Visibility = Visibility.Hidden;
                    labelCompanyName.Visibility = Visibility.Hidden;
                    //labelRequestDate.Visibility = Visibility.Hidden;
                }
            }
            catch (System.Exception err)
            {
                System.Diagnostics.Trace.TraceError(err.ToString());
                MessageBox.Show(err.ToString());
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            labelCountryCode.Content = ((ComboBoxItem)comboBoxCountries.SelectedItem).Tag.ToString();
        }
    }
}