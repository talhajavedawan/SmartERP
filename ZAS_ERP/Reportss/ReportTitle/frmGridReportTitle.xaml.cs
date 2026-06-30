using DevExpress.Xpf.Core;
using ERP_BL.Enums;
using ERP_BL.Reports;
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
using System.Windows.Shapes;

namespace ZAS_ERP.Reportss.ReportTitle
{
    /// <summary>
    /// Interaction logic for frmGridReportTitle.xaml
    /// </summary>
    public partial class frmGridReportTitle : DXWindow
    {
        int titleId = 0;
        GridReportRepo repo = new GridReportRepo();
        ERP_BL.Reports.ReportTitle title = new ERP_BL.Reports.ReportTitle();
        public frmGridReportTitle()
        {
            InitializeComponent();
        }  
        public frmGridReportTitle( int _titleId)
        {
            InitializeComponent();
            titleId = _titleId;
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTitleTypes();
            if(titleId!=0)
            {
                title=repo.GetReportTitle(titleId);
                txtTitle.Text = title.titleName;
                if(title.isActive==true)
                {
                    checkIsActive.IsChecked = true;
                }
                else
                {
                    checkIsActive.IsChecked = false;
                }
            }
        }
      public void  LoadTitleTypes()
        {
           
            for (int i = 0; i <= (int)ERP_BL.Enums.GridReportType.MemorizedReport; i++)
            {
                
                    if (((ERP_BL.Enums.GridReportType)i).ToString() == GridReportType.StandardReport.ToString())
                    {
                        titleTypes.Items.Add(((ERP_BL.Enums.GridReportType)i).ToString());
                    }
                
                
                    if (((ERP_BL.Enums.GridReportType)i).ToString() == GridReportType.MemorizedReport.ToString())
                    {
                        titleTypes.Items.Add(((ERP_BL.Enums.GridReportType)i).ToString());
                    }

                
              


            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(titleId!=0)
            {
                title.titleName = txtTitle.Text;
                if (checkIsActive.IsChecked == true)
                {
                    title.isActive = true;
                }
                else
                {
                    checkIsActive.IsChecked = false;
                }
                if (titleTypes.Text == GridReportType.StandardReport.ToString())
                {
                    title.gridReportType = GridReportType.StandardReport;
                }
                else
                if (titleTypes.Text == GridReportType.MemorizedReport.ToString())
                {
                    title.gridReportType = GridReportType.MemorizedReport;
                    title.userId = SYSTEM_STATIC.currentUser.id;

                }
                repo.UpdateReportTitle(title);
                DXMessageBox.Show("Title has been updated successfully", "Information");
                this.Close();
            }
            else
            {
                title = new ERP_BL.Reports.ReportTitle();
                title.titleName = txtTitle.Text;
                if (checkIsActive.IsChecked == true)
                {
                    title.isActive = true;
                }
                else
                {
                    checkIsActive.IsChecked = false;
                }
                if ((GridReportType)titleTypes.SelectedIndex == GridReportType.StandardReport)
                {
                    title.gridReportType = GridReportType.StandardReport;
                }
                else
                 if ((GridReportType)titleTypes.SelectedIndex == GridReportType.MemorizedReport)
                {
                    title.gridReportType = GridReportType.MemorizedReport;
                    title.userId = SYSTEM_STATIC.currentUser.id;
                }
                repo.SaveReportTitle(title);
                DXMessageBox.Show("Title has been added successfully", "Information");
                this.Close();
            }

        }
    }
}
