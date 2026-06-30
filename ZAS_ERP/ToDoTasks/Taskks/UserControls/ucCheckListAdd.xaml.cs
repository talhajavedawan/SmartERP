using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks.Taskss;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Printing;
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
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucCheckListAdd.xaml
    /// </summary>
    public partial class ucCheckListAdd : UserControl
    {
        public Checklist checklist = new Checklist();
        UsersRepo UsersRepo = new UsersRepo();
        public ucCheckListAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(checklist != null)
            {
                if (checklist.creator != null)
                {


                    if (checklist.creator.employee != null && checklist.creator.employee.person != null)
                    {
                        txtName.Text = checklist.creator.employee.person.FName + " " + checklist.creator.employee.person.LName;
                        if (checklist.creator.employee.person.Signature != null)
                            ConvertByteToBmp(checklist.creator.employee.person.Signature);
                    }
                }
                else
                {
                    if (SYSTEM_STATIC.currentUser != null && SYSTEM_STATIC.currentUser.employee != null && SYSTEM_STATIC.currentUser.employee.person != null)
                    {
                        txtName.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
                        if (SYSTEM_STATIC.currentUser.employee.person.Signature != null)
                            ConvertByteToBmp(SYSTEM_STATIC.currentUser.employee.person.Signature);
                    }
                }

                if (checklist.creationDate != null)
                    txtDated.Text = checklist.creationDate.Value.Day + "/"+ checklist.creationDate.Value.Month + "/" + checklist.creationDate.Value.Year;

            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Received Quantity in Checklist") != null)
                grdCheckListItems.Columns["ReceivedQuantity"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            else
                grdCheckListItems.Columns["ReceivedQuantity"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Received Weight in Checklist") != null)
                grdCheckListItems.Columns["ReceivedWeight"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            else
                grdCheckListItems.Columns["ReceivedWeight"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Packing Dimensions in Checklist") != null)
                grdCheckListItems.Columns["PackingDimensions"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            else
                grdCheckListItems.Columns["PackingDimensions"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Packing Style in Checklist") != null)
                grdCheckListItems.Columns["packingStyle"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            else
                grdCheckListItems.Columns["packingStyle"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
        }

        private void grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            if (((GridControl)sender).View.ActiveEditor != null)
            {
                e.Handled = true;
                TextBox tb = (TextBox)e.OriginalSource;
                var i = tb.CaretIndex;
                tb.Text += "\n";
                tb.CaretIndex = i + 1;
            }
        }

        private void view_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            grdTaskComments.SetCellValue(e.RowHandle, "userId", SYSTEM_STATIC.currentUser.id);
        }

        private void view_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            
        }
        
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }


        public void ConvertByteToBmp(byte[] bytesArr)        {            try            {                MemoryStream stream = new MemoryStream();                stream.Write(bytesArr, 0, bytesArr.Length);                stream.Position = 0;                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);                BitmapImage returnImage = new BitmapImage();                returnImage.BeginInit();                MemoryStream ms = new MemoryStream();                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);                ms.Seek(0, SeekOrigin.Begin);                returnImage.StreamSource = ms;                returnImage.EndInit();

                imgUserSignature.Source = returnImage;            }            catch (Exception ex)            {                throw ex;            }        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            var prod = grdCheckListItems.SelectedItem as ProcurementProduct;

            if (prod != null && prod.Id > 0)
            {


                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveProcurementProductsAttachmentCategories();
                    //var pos = System.Windows.Input.Mouse.GetPosition(this);
                    //grdAttach1.TranslatePoint(pos, grdCheckListItems);
                    grdAttach1.Visibility = Visibility.Visible;
                }
            }
            else
            {
                DXMessageBox.Show("This item need to be saved first!");
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            var prod = grdCheckListItems.SelectedItem as ProcurementProduct;

            if (prod != null && prod.Id > 0)
            {
                var Idd = prod.Id;
                string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
                string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
                if (cmbCategory1.SelectedItem != null)
                {
                    if (Idd != 0)
                    {
                        try
                        {

                            int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                            OpenFileDialog fileDialog = new OpenFileDialog();
                            fileDialog.Multiselect = false;
                            fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                            string sourceFile = @"";
                            string exePath = System.Environment.GetCommandLineArgs()[0];
                            string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                            destination += "Attachments\\ProcurementProducts\\ToUpload\\";
                            //string destination = @"D:\MovedFiles\new\";
                            System.IO.Directory.CreateDirectory(destination);
                            if (fileDialog.ShowDialog() == true) // Test result.
                            {
                                imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                                btnAttachNew.ToolTip = "Uploading";
                                btnAttachNew.IsEnabled = true;

                                btnAttachment.Content = "Uploading File . . .";
                                sourceFile = fileDialog.FileName;
                                destination += Idd + "_" + TransactionItemType.ProcurementProducts.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                                if (sourceFile.Length < 74)
                                {
                                    System.IO.File.Move(sourceFile, destination);

                                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                                    {
                                        ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                        var result = attachment.startUploading(TransactionItemType.ProcurementProducts);
                                        if (result.Item1)
                                        {
                                            AttachmentsRepo repo = new AttachmentsRepo();
                                            //Attachment attachmen= new Attachment();
                                            repo.Add(System.IO.Path.GetFileName(result.Item2), Idd, TransactionItemType.ProcurementProducts, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                            UsersRepo.Add(TransactionInfo.Attachment_Uploaded, prod.Id, (int)TransactionItemType.ProcurementProducts, "Added a new attachment");

                                            this.Dispatcher.Invoke(() =>
                                            {
                                                imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                                btnAttachNew.ToolTip = "Attach";
                                                btnAttachNew.IsEnabled = true;
                                                btnAttachment.Content = "Select";
                                            });



                                        }
                                        else
                                        {
                                            this.Dispatcher.Invoke(() =>
                                            {
                                                System.IO.File.Move(destination, sourceFile);
                                                DXMessageBox.Show("Error while Uploading Attachment, Try Again", "Try again");
                                                imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                                btnAttachNew.ToolTip = "Attach";
                                                btnAttachNew.IsEnabled = true;
                                                btnAttachment.Content = "Select";
                                            });
                                        }
                                    });
                                    thread.Start();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid File name size");
                                    return;
                                }

                                //DXMessageBox.Show("Attachment Uploaded");


                            }


                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show(ex.ToString());
                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                            btnAttachNew.ToolTip = "Attach";
                            btnAttachNew.IsEnabled = true;
                        }
                        finally
                        {



                        }
                    }
                    else
                        return;
                }
                else
                {
                    DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }

        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            var prod = grdCheckListItems.SelectedItem as ProcurementProduct;

            if (prod != null && prod.Id > 0)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(prod.Id, TransactionItemType.ProcurementProducts);
                    grdAttachments.Visibility = Visibility.Visible;
                }
            }
        }

        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
        }

        private void BtnCloseAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);

                        if (str.Contains("ProcurementProducts"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.ProcurementProducts);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else if (str.Contains("Sale_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Offer"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Offer);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Invoice"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Invoice);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Purchase_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Purchase_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Receipt"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Receipt);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Payments"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Payments);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Bill"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Bill);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("LoansAdvances"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.LoansAdvances);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else if (str.Contains("TravelingRecord"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.TravelingRecord);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }

                    });
                    thread.Start();
                    //grdProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                    return;



            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            //print.PrintVisual(grdCheckList, "Checklist");
            
            print.PrintTicket.PageMediaType = System.Printing.PageMediaType.MultiLayerForm;
            print.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
            Size pageSize = new Size(print.PrintableAreaWidth +30, print.PrintableAreaHeight + 250);
            grdCheckList.Measure(pageSize);
            grdCheckList.Arrange(new Rect(0, 0, pageSize.Height, pageSize.Height -270));
            print.PrintVisual(grdCheckList, "Checklist");

            //if (print.ShowDialog() == true)
            //{
            //    //PrintCapabilities capabilities = print.PrintQueue.GetPrintCapabilities(print.PrintTicket);

            //    //double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / grdCheckList.ActualWidth,
            //    //                        capabilities.PageImageableArea.ExtentHeight / grdCheckList.ActualHeight);

            //    //Transform oldTransform = grdCheckList.LayoutTransform;

            //    //grdCheckList.LayoutTransform = new ScaleTransform(scale, scale);

            //    //Size oldSize = new Size(grdCheckList.ActualWidth, grdCheckList.ActualHeight);
            //    //Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
            //    //grdCheckList.Measure(sz);
            //    //((UIElement)grdCheckList).Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight),
            //    //    sz));

            //    //print.PrintVisual(grdCheckList, "Checklist");
            //    //grdCheckList.LayoutTransform = oldTransform;
            //    //grdCheckList.Measure(oldSize);

            //    //((UIElement)grdCheckList).Arrange(new Rect(new Point(0, 0),
            //    //    oldSize));



            //    //PrintDialog printDlg = new PrintDialog();
            //    //printDlg.PrintVisual(grdCheckList, "Checklist");
            //    //PrintDialog dlg = new PrintDialog();
            //    //var pd = new PrintDialog();
            //    //var pageSize = new Size(8.26 * 96, 11.69 * 96);
            //    //FrameworkElement fe = (grdCheckList as FrameworkElement);
            //    //fe.Measure(new Size(Int32.MaxValue, Int32.MaxValue));
            //    //Size visualSize = fe.DesiredSize;
            //    //fe.Arrange(new Rect(new Point(0, 0), visualSize));
            //    //MemoryStream stream = new MemoryStream();
            //    //string pack = "pack://temp.xps";
            //    //Uri uri = new Uri(pack);
            //    //DocumentPaginator paginator;
            //    //XpsDocument xpsDoc;
            //    //using (Package container = Package.Open(stream, FileMode.Create))
            //    //{
            //    //    PackageStore.AddPackage(uri, container);
            //    //    using (xpsDoc = new XpsDocument(container, CompressionOption.Fast, pack))
            //    //    {
            //    //        XpsSerializationManager rsm =
            //    //          new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
            //    //        rsm.SaveAsXaml(grdCheckList);
            //    //        paginator = ((IDocumentPaginatorSource)
            //    //          xpsDoc.GetFixedDocumentSequence()).DocumentPaginator;
            //    //        paginator.PageSize = visualSize;
            //    //    }
            //    //    PackageStore.RemovePackage(uri);
            //    //}
            //    //if ((bool)dlg.ShowDialog().GetValueOrDefault())
            //    //{
            //    //    dlg.PrintDocument(paginator, "");
            //    //}
            //}
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdCheckListItems_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GrdCheckListItems_ColumnsPopulated(object sender, RoutedEventArgs e)
        {
            var colAttach = grdCheckListItems.Columns["AttachNew"];
            colAttach.CellTemplate = (DataTemplate)this.Resources["AttachmentButton"];
            colAttach.Width = 70;
            colAttach.Name = "AttachNew";

            var colAttachList = grdCheckListItems.Columns["AttachmentList"];
            colAttachList.CellTemplate = (DataTemplate)this.Resources["AttachmentListButton"];
            colAttachList.Width = 70;
            colAttachList.Name = "AttachmentList";
        }

        private void grdCheckListItems_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {

        }
    }
}
