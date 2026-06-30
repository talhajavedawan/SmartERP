using DevExpress.Xpf.Core;
using ERP_BL.Documents;
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

namespace ZAS_ERP.FilesAndDocss.Documentss
{
    /// <summary>
    /// Interaction logic for ucFrmDocumentAuthorityAdd.xaml
    /// </summary>
    public partial class ucFrmDocumentAuthorityAdd : UserControl
    {
        public bool editFlag = false;
        DocumentRepo documentRepo = new DocumentRepo();
        DocumentAuthority documentAuthority = new DocumentAuthority();
        public int documentAuthorityId = 0;
        public ucFrmDocumentAuthorityAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lookupDocumentTemplate.ItemsSource = documentRepo.GetAllDocumentTemplate();
            if (editFlag == true)
            {
                documentAuthority = documentRepo.GetDocumentAuthority(documentAuthorityId);

                if (documentAuthority.documentTemplate != null)
                    lookupDocumentTemplate.Text = documentAuthority.documentTemplate.TemplateName;

                txtDocumentAuthority.Text = documentAuthority.AuthorityName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lookupDocumentTemplate.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Document Template!");
                lookupDocumentTemplate.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtDocumentAuthority.Text))
            {
                DXMessageBox.Show("Please Enter Document Authority!");
                txtDocumentAuthority.Focus();
                return;
            }

            documentAuthority.documentTemplate_Id = (lookupDocumentTemplate.SelectedItem as DocumentTemplate).Id;
            documentAuthority.AuthorityName = txtDocumentAuthority.Text;



            if (editFlag == false)
            {
                documentRepo.AddDocumentAuthority(documentAuthority);
                DXMessageBox.Show("Added Succesfully!");
            }
            else
            {
                documentRepo.UpdateDocumentAuthority(documentAuthority);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

    }
}
