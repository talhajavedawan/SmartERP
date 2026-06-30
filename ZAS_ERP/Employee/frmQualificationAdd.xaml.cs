using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.HR;
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

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for frmQualificationAdd.xaml
    /// </summary>
    public partial class frmQualificationAdd : Window
    {
        public bool isSave = false;
        public bool isDegree = true;
        public List<Qualification> qualList = new List<Qualification>();
        public Qualification qualification = new Qualification();
        List<DegreeType> degreeTpeLst = new List<DegreeType>();
        public frmQualificationAdd()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //foreach (int i in Enum.GetValues(typeof(ERP_BL.Enums.DegreeType)))
            //{
            //    degreeTpeLst.Add(((ERP_BL.Enums.DegreeType)i));

            //}

            //cmbxDegreeType.ItemsSource = degreeTpeLst;

            DegreeTypes degreeTypes = new DegreeTypes();
            cmbxDegreeType.ItemsSource = degreeTypes.GetDegreesList();

        }

        private void TxtPercent_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                float percentAge = 0;

                if (String.IsNullOrEmpty(txtObtMarks.Text) && String.IsNullOrEmpty(txtTotMarks.Text))
                {
                    return;
                }
                float obtMarks = Convert.ToSingle(txtObtMarks.Text);
                float totMarks = Convert.ToSingle(txtTotMarks.Text);
                if (totMarks >= obtMarks)
                {
                    percentAge = obtMarks / totMarks * 100;
                }

                txtPercent.Text = percentAge.ToString();
            }
            catch
            { }
            
        }

        private void SaveDegree_Click(object sender, RoutedEventArgs e)
        {
            Qualification qual = new Qualification();
            
            if (isDegree == false)
            {
                qual.DegreeTitle = txtCertTitle.Text;
                // qual.DegreeType = DegreeType.Certification;
                qual.DegreeType = "Certification";
                qual.IsValid = (bool)chckIsValid.IsChecked;
                qual.validTill = (DateTime)validTill.DateTime;
                qual.Score = txtScore.Text;

            }
            else
            {
                qual.DegreeTitle = txtDegreeTitle.Text;
                //if (cmbxDegreeType.SelectedItem != null)
                //{
                //    qual.DegreeType = (ERP_BL.Enums.DegreeType)cmbxDegreeType.SelectedItem;
                //}
                qual.DegreeType = (string)cmbxDegreeType.EditValue;

            }


            qual.Specialization = txtSubject.Text;
            if (!String.IsNullOrEmpty(txtObtMarks.Text))
            {
                qual.MarksObtained = Convert.ToSingle(txtObtMarks.Text);
                if (!string.IsNullOrEmpty(txtTotMarks.Text))
                { qual.MarksTotal = Convert.ToSingle(txtTotMarks.Text); }
                else
                {
                    qual.MarksTotal = 0;
                }

                
                if (!String.IsNullOrEmpty(txtPercent.Text))
                { qual.MarksPercentage = Convert.ToSingle(txtPercent.Text); }
                
                else{ qual.MarksPercentage = 0; }



                }
            else
            {
                qual.MarksObtained = 0;
                qual.MarksTotal = 0;
                qual.MarksPercentage = 0;
            }
           

            qual.Division =  txtDiv.Text;
            qual.StartYear = dateStarting.DateTime;
            qual.PassingYear = datePassing.DateTime;
            qual.Institute =     txtInst.Text;
            qual.Location =  txtCountry.Text;
            qual.IsLatest = (bool)chckIsLatest.IsChecked;
            qual.IsDistinction = (bool)chckIsDistint.IsChecked;
            qual.DistDetails =  txtDist.Text;
            qual.IsCompleted = (bool)chckIsComp.IsChecked;
            
            


            qualification = qual;
            //qualList.Add(qual);
            this.Close();
            isSave = true;
        }

        private void DegreeSwitch_Checked(object sender, RoutedEventArgs e)
        {
            isDegree = false;
            //MessageBox.Show("Checked");
            grpCertTitle.Visibility    = Visibility.Visible;
            grpDegreeTitle.Visibility = Visibility.Collapsed;

            grpSubjects.Visibility = Visibility.Collapsed;
            grpPercent.Visibility = Visibility.Collapsed;
            grpLatest.Visibility = Visibility.Collapsed;

            grpValid.Visibility = Visibility.Visible;
            grpDist.Visibility = Visibility.Collapsed;
        }

        private void DegreeSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            isDegree = true;
            grpCertTitle.Visibility = Visibility.Collapsed;
            grpDegreeTitle.Visibility = Visibility.Visible;
            //  MessageBox.Show("Unchecked");
            grpSubjects.Visibility = Visibility.Visible;
            grpPercent.Visibility = Visibility.Visible;
            grpLatest.Visibility = Visibility.Visible;

            grpValid.Visibility = Visibility.Collapsed;
            grpDist.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isSave == true)
            {
            }
            else
            {
            }
        }
    }
}
