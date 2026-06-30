using ERP_BL.Fields;
using System;
using System.Collections.Generic;
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

namespace ZAS_ERP.TemplateFields
{
    /// <summary>
    /// Interaction logic for ucFrmBill.xaml
    /// </summary>
    public partial class ucFrmBill : UserControl
    {
        public ucFrmBill()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FieldsRepo fieldsRepo = new FieldsRepo();
            ModuleFields moduleFields = new ModuleFields();

            moduleFields = fieldsRepo.GetFieldsByTemplateNModule(1, ERP_BL.Enums.TransactionItemType.Inquiry);
            if(moduleFields != null)
            {
                var fields = moduleFields.fields;
                foreach(var _field in fields)
                {
                    foreach (var _child in mainGrid.Children)
                    {
                        var element = _child as Control/*.GetType().Name*/;

                        if (element.Name.EndsWith(_field.Tag))
                        {
                            element.Visibility = Visibility.Visible;
                        }
                    }
                }
                
            }

            

            
        }
    }
}
