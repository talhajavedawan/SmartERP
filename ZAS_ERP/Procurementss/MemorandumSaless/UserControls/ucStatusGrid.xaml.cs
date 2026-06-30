using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
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
using ERP_BL;

using ERP_BL.Config;
using ERP_BL.Enums;

namespace ZAS_ERP.Procurementss.MemorandumSaless
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class ucStatusGrid : UserControl
    {



        MemorandumSaleRepo repo = new MemorandumSaleRepo();
        List<MemorandumSaleStatus> statuss = new List<MemorandumSaleStatus>();
        MemorandumSaleStatus status = new MemorandumSaleStatus();
        public ucStatusGrid()
        {
            InitializeComponent();
            
        }

        private void ucMemorandumSaleStatusGrid_Loaded(object sender, RoutedEventArgs e)
        {
            loadItemgrid();
        }

        private void loadItemgrid()
        {
            statuss = repo.getAllMemorandumSaleStatus();
            grdStatus.ItemsSource = statuss;
            grdStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            grdStatus.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdStatus.Columns.GetColumnByFieldName("backcolor").Visible = false;
            grdStatus.Columns.GetColumnByFieldName("forecolor").Visible = false;
            //txtStatus.Text = "";
            //cpStatus.Text = ""; 
            //grdemployee.Columns.GetColumnByFieldName("person").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("address").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("contact").Visible = false;
            ////grdemployee.Columns.GetColumnByFieldName("Companies").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Desig").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Disability").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("DisDescription").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("MaritalStatus").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Status").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("JoinDate").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("BasicPay").Visible = false;
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "EmpId" });
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.FName" });
            //grdemployee.Columns.GetColumnByFieldName("person.FName").Header = "First Name";
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.LName" });
            //grdemployee.Columns.GetColumnByFieldName("person.LName").Header = "Last Name";

        }

        
        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if(txtStatus.Text == "")
            {
                MessageBox.Show("Enter Status Name");
                txtStatus.Focus();
            }
            
            else 
            {
               
                    status.Status = txtStatus.Text.Trim();
                    if (chkisactive.IsChecked == true)
                        status.isActive = true;
                    else
                        status.isActive = false;
                    status.backcolor = cpStatus.Text.Trim();
                if (status.Id == 0)
                {
                    repo.addStatus(status);

                    MessageBox.Show("New MemorandumSale Status '" + txtStatus.Text + "' Added", "Congratulations");
                    //var myWindow = Window.GetWindow(this);
                    //myWindow.Close();
                    loadItemgrid();
                }
                else if (status.Id != 0)
                {
                    repo.updateStatus(status);
                    loadItemgrid();
                    btnStatusSave.Content = "Save";
                    MessageBox.Show("MemorandumSale Status '" + txtStatus.Text + "' has been Updated", "Congratulations");
                }
            }
        }
        //int empid;
        
        private void ColorEdit_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (cpStatus.Color.R <= 120 || cpStatus.Color.G <= 120 || cpStatus.Color.B <= 120)
            {
                var myColor = "#FFFFFFFF";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                //txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;

            }
            else
            {
                var myColor = "#FF000000";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                //txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;
            }
            txtStatus.Background = new SolidColorBrush(cpStatus.Color);
        }

        private void grdStatus_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            status = grdStatus.SelectedItem as MemorandumSaleStatus;
            
            if (status.isActive == false)
                chkisactive.IsChecked = false;
            else
                chkisactive.IsChecked = true;
            //cpStatus.Text = status.backcolor;
            var myColor = status.backcolor;
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
            System.Windows.Media.Color Color = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            //txtStatus.Foreground = new SolidColorBrush(newColor);
            
            cpStatus.Color = Color;
            txtStatus.Text = status.Status;
        }

        private void btnNewMemorandumSaleStatus_Click(object sender, RoutedEventArgs e)
        {
            status = new MemorandumSaleStatus();
            txtStatus.Text = "";
            btnStatusSave.Content = "Save";
            var myColor = "#FFFFFFFF";
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
            System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            //txtStatus.Foreground = new SolidColorBrush(newColor);
            status.forecolor = myColor;
            cpStatus.Color = newColor;
        }

        private void btnEditMemorandumSaleStatus_Click(object sender, RoutedEventArgs e)
        {
            btnStatusSave.Content = "Update";
        }
    }
}
