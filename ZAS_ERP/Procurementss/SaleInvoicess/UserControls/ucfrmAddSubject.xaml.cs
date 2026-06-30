using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.SaleInvoicess.UserControls
{
    /// <summary>
    /// Interaction logic for ucfrmAddSubject.xaml
    /// </summary>
    public partial class ucfrmAddSubject : UserControl
    {
        public ucfrmAddSubject()
        {
            InitializeComponent();
        }

        private void TxtComments_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text.Length > 0)
            {
                // tb.Text = Char.ToUpper(tb.Text[0]).ToString();
                var v = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text);
                tb.Text = v;
            }
        }
    }
}
