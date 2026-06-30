using DevExpress.Xpf.Core;
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



namespace ZAS_ERP.PopupNotificatios
{
    /// <summary>
    /// Interaction logic for frmCreatePopup.xaml
    /// </summary>
    public partial class frmCreatePopup : DXWindow
    {
        
        public bool isCheck;
        ERP_BL.PopupNotificatios.popupNotificatinRepo PopupRepo = new ERP_BL.PopupNotificatios.popupNotificatinRepo();
        ERP_BL.PopupNotificatios.popupNotifications popupNotifications = new ERP_BL.PopupNotificatios.popupNotifications();
        List<string> first = new List<string> { "12", "14", "18", "20", "25", "30" };
        public static string heading = "";
        public static string description = "";

        public frmCreatePopup()
        {
            InitializeComponent();

       
            
        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
          
            cmbxFont.ItemsSource = first;

          
        }
       

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    heading = txtNotificationsHeading.Text;
            //    description = popupTextboxText.Text;
            //    this.Close();
            //}
            //catch (Exception ex)
            //{

            //    DXMessageBox.Show(ex.Message);
            //}


            try
            {

                if (DXMessageBox.Show("Do you really want to add Notifications?", "Notifications...", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                {
                    if (!string.IsNullOrEmpty(popupTextboxText.Text) && !string.IsNullOrEmpty(txtNotificationsHeading.Text))
                    {

                        popupNotifications.Title = txtNotificationsHeading.Text;
                        popupNotifications.popupText = popupTextboxText.Text;

                        popupNotifications.FontSize = popupTextboxText.FontSize;
                        popupNotifications.fontWeight = popupTextboxText.FontWeight.ToString();
                        popupNotifications.Italic = popupTextboxText.FontStyle.ToString();

                        popupNotifications.FontSizeHeading = txtNotificationsHeading.FontSize;
                        popupNotifications.fontWeightHeading = txtNotificationsHeading.FontWeight.ToString();
                        popupNotifications.ItalicHeading = txtNotificationsHeading.FontStyle.ToString();


                        popupNotifications.TitleColorCode = txtNotificationsHeading.Foreground.ToString();
                        popupNotifications.TextColorCode = popupTextboxText.Foreground.ToString();
                        //popupNotifications.TitleColorCode = ClrPcker_Background.Color.R.ToString() + "," + ClrPcker_Background.Color.G.ToString() + "," + ClrPcker_Background.Color.B.ToString();
                        PopupRepo.AddPopupNotifications(popupNotifications);
                        this.Close();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(popupTextboxText.Text) && string.IsNullOrEmpty(txtNotificationsHeading.Text))
                        {
                            MessageBox.Show("Please Add NOTIFICATION & HEADING  Text to show a Notification to User's");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(popupTextboxText.Text))
                                MessageBox.Show("Please Add NOTIFICATION Text to show a Notification to User's");
                            if (string.IsNullOrEmpty(txtNotificationsHeading.Text))
                                MessageBox.Show("Please Add HEADING Text to show a Notification to User's");
                        }

                    }
                }
                //var response = MessageBox.Show("Do you really want to add Notifications?", "Notifications...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
                //if (response == MessageBoxResult.No)
                //{

                //}
                //else
                //{

                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

        }

        private void ClrPcker_Background_ColorChanged(object sender, RoutedEventArgs e)
        {
         
            if (txtNotificationsHeading.IsSelectionActive)
            {
                var red = ClrPcker_Background.Color.R;
                var green = ClrPcker_Background.Color.G;
                var blue = ClrPcker_Background.Color.B;
                txtNotificationsHeading.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
            }

            else
            {
                var red = ClrPcker_Background.Color.R;
                var green = ClrPcker_Background.Color.G;
                var blue = ClrPcker_Background.Color.B;
                popupTextboxText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
            }  
        }
      

        private void BtnBold_Click(object sender, RoutedEventArgs e)
        {
           


            if (ct!=null && ct.Name == "H")
            {
                if (!string.IsNullOrEmpty(txtNotificationsHeading.Text))
                {
                    txtNotificationsHeading.FontWeight = FontWeight.FromOpenTypeWeight(800); /*System.Windows.FontWeight.FromOpenTypeWeight(int.Parse(popup.fontWeight));*/
                    btnBold.Visibility = Visibility.Collapsed;
                    btnNormal.Visibility = Visibility.Visible;
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(popupTextboxText.Text))
                {
                    //var SelectedText = popupTextboxText.SelectedText;

                    //FontWeight.FromOpenTypeWeight(800) = SelectedText.ToString();
                    //var bold = FontWeight.FromOpenTypeWeight(800);          
                    popupTextboxText.FontWeight = FontWeight.FromOpenTypeWeight(800); /*System.Windows.FontWeight.FromOpenTypeWeight(int.Parse(popup.fontWeight));*/
                    btnBold.Visibility = Visibility.Collapsed;
                    btnNormal.Visibility = Visibility.Visible;


                }
            }
                
        }
        private void BtnNormal_Click(object sender, RoutedEventArgs e)
        {
            if (ct!=null && ct.Name == "H")
            {
                if (!string.IsNullOrEmpty(txtNotificationsHeading.Text))
                {
                    txtNotificationsHeading.FontWeight = FontWeight.FromOpenTypeWeight(100); /*System.Windows.FontWeight.FromOpenTypeWeight(int.Parse(popup.fontWeight));*/
                    btnBold.Visibility = Visibility.Visible;
                    btnNormal.Visibility = Visibility.Collapsed;
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(popupTextboxText.Text))
                {
                    popupTextboxText.FontWeight = FontWeight.FromOpenTypeWeight(100); /*System.Windows.FontWeight.FromOpenTypeWeight(int.Parse(popup.fontWeight));*/
                    btnBold.Visibility = Visibility.Visible;
                    btnNormal.Visibility = Visibility.Collapsed;
                    // popupTextboxText.FontSize = 30;
                }

            }
           
        }

      

        private void CmbxFont_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (ct!=null && ct.Name == "H")
            {
                if (!string.IsNullOrEmpty(txtNotificationsHeading.Text))
                {

                    switch (cmbxFont.SelectedIndex)
                    {
                        case 0:
                            txtNotificationsHeading.FontSize = 12;
                            break;
                        case 1:
                            txtNotificationsHeading.FontSize = 14;
                            break;
                        case 2:
                            txtNotificationsHeading.FontSize = 18;
                            break;
                        case 3:
                            txtNotificationsHeading.FontSize = 20;
                            break;
                        case 4:
                            popupTextboxText.FontSize = 25;
                            break;
                        case 5:
                            txtNotificationsHeading.FontSize = 30;
                            break;
                    }
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(popupTextboxText.Text))
                {

                    switch (cmbxFont.SelectedIndex)
                    {
                        case 0:
                            popupTextboxText.FontSize = 12;
                            break;
                        case 1:
                            popupTextboxText.FontSize = 14;
                            break;
                        case 2:
                            popupTextboxText.FontSize = 18;
                            break;
                        case 3:
                            popupTextboxText.FontSize = 20;
                            break;
                        case 4:
                            popupTextboxText.FontSize = 25;
                            break;
                        case 5:
                            popupTextboxText.FontSize = 30;
                            break;
                    }
                }
            }
                
        }

        private void CmbxFont_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                if (ct!=null && ct.Name == "H")
                {
                    var text = cmbxFont.Text;
                    if (text != "")
                        txtNotificationsHeading.FontSize = Convert.ToDouble(text);
                    return;
                }
                else
                {
                    var text = cmbxFont.Text;
                    if(text!="")
                      popupTextboxText.FontSize = Convert.ToDouble(text);
                    return;
                }
  
            }
        }

        private void BtnItalic_Click(object sender, RoutedEventArgs e)
        {
            if (ct != null && ct.Name == "H")
            {
                txtNotificationsHeading.FontStyle = FontStyles.Italic;
                btnItalic.Visibility = Visibility.Collapsed;
                btnNonItalic.Visibility = Visibility.Visible;
            }
          
            else
            {
                if (!string.IsNullOrEmpty(popupTextboxText.Text))
                {

                    popupTextboxText.FontStyle = FontStyles.Italic;
                    btnItalic.Visibility = Visibility.Collapsed;
                    btnNonItalic.Visibility = Visibility.Visible;
                }

            }
   
        }

        private void BtnNonItalic_Click(object sender, RoutedEventArgs e)
        {
            if (ct != null &&ct.Name == "H")
            {
                txtNotificationsHeading.FontStyle = FontStyles.Normal;
                btnItalic.Visibility = Visibility.Visible;
                btnNonItalic.Visibility = Visibility.Collapsed;
            }
         
            else
            {
                if (!string.IsNullOrEmpty(popupTextboxText.Text))
                {

                    popupTextboxText.FontStyle = FontStyles.Normal;
                    btnItalic.Visibility = Visibility.Visible;
                    btnNonItalic.Visibility = Visibility.Collapsed;
                }

            }

        }
        private Control ct;
        private void TxtNotificationsHeading_GotFocus(object sender, RoutedEventArgs e)
        {
            ct = (Control)sender;
            ct.Name = "H";
        }

        private void PopupTextboxText_GotFocus(object sender, RoutedEventArgs e)
        {
            ct = (Control)sender;
            ct.Name = "N";
        }

    }
}
