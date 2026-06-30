using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucFrmAddTemplate.xaml
    /// </summary>
    public partial class ucFrmAddTemplate : UserControl
    {
        public Window addTemplateWindow = new Window();
        FieldsRepo fieldsRepo = new FieldsRepo();
        public Template template = new Template();
        public bool editFlag = false;
        public ucFrmAddTemplate()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            populatefIelds();
        }

        public void populatefIelds()
        {
            //Populating Combobox Module Type
            for (int i = 0; i <= (int)ERP_BL.Enums.TransactionItemType.Admin_Bill; i++)
            {
                cmbxModule.Items.Add(((ERP_BL.Enums.TransactionItemType)i).ToString());
            }

            //Populating Combobox COA Type
            for (int i = 0; i <= (int)ERP_BL.Enums.COA_AccountType.Other_Expense; i++)
            {
                cmbxCoaType.Items.Add(((ERP_BL.Enums.COA_AccountType)i).ToString());
            }

            if (editFlag == true)
            {
                txtTemplateName.Text = template.Name;
                cmbxModule.Text = template.transactionType.ToString();

                if(template.transactionType == ERP_BL.Enums.TransactionItemType.Admin_Bill)
                {
                    if(template.Coa_AccountType != null)
                    {
                        cmbxCoaType.IsEnabled = true;
                        cmbxCoaType.Text = template.Coa_AccountType.ToString();
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTemplateName.Text))
            {
                DXMessageBox.Show("Please enter Template Name!");
                txtTemplateName.Focus();
                return;
            }
            if(cmbxModule.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Module!");
                cmbxModule.Focus();
                return;
            }
            //if((ERP_BL.Enums.TransactionItemType)cmbxModule.SelectedIndex == ERP_BL.Enums.TransactionItemType.Admin_Bill)
            //{
            //    if(cmbxCoaType.SelectedIndex < 0)
            //    {
            //        DXMessageBox.Show("Please select COA Type!");
            //        cmbxCoaType.Focus();
            //        return;
            //    }
            //}
            
            template.Name = txtTemplateName.Text;
            template.transactionType = (ERP_BL.Enums.TransactionItemType)cmbxModule.SelectedIndex;

            //if ((ERP_BL.Enums.TransactionItemType)cmbxModule.SelectedIndex == ERP_BL.Enums.TransactionItemType.Admin_Bill)
            //{
            //    template.Coa_AccountType = (ERP_BL.Enums.COA_AccountType)cmbxCoaType.SelectedIndex;
            //}

            if (editFlag == false && template.Id == 0)
            {
                fieldsRepo.AddTemplate(template);
                DXMessageBox.Show("Successfully Added!");
                addTemplateWindow.Close();
            }
            else if (editFlag == true && template.Id != 0)
            {
                fieldsRepo.UpdateTemplate(template);
                DXMessageBox.Show("Updated Successfully!");
                addTemplateWindow.Close();
            }
        }

        private void CmbxModule_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if ((ERP_BL.Enums.TransactionItemType)cmbxModule.SelectedIndex == ERP_BL.Enums.TransactionItemType.Admin_Bill)
            //{
            //    cmbxCoaType.IsEnabled = true;
            //}
            //else
            //{
            //    cmbxCoaType.IsEnabled = false;
            //}
        }
    }
}
