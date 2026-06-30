using DevExpress.Xpf.Core;
using ERP_BL.FilesAndDocs;
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

namespace ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls
{
    /// <summary>
    /// Interaction logic for ucAirlineAdd.xaml
    /// </summary>
    public partial class ucAirlineAdd : UserControl
    {
        Airline airline = new Airline();
        public int typeId = 0;
        VisitingRecordRepo recordRepo = new VisitingRecordRepo();
        public bool editFlag = false;

        public ucAirlineAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && typeId > 0)
            {
                airline = recordRepo.GetAirline(typeId);
                txtAirlineName.Text = airline.Name;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            airline.Name = txtAirlineName.Text;
            //taskRepo = new ToDoTaskRepo();
            if (editFlag == false)
            {
                recordRepo.AddAirline(airline);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                recordRepo.UpdateAirline(airline);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
