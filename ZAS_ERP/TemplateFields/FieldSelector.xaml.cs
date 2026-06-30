using DevExpress.Xpf.Core;
using ERP_BL.Databases;
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
using System.Windows.Shapes;
using ZAS_ERP.Inquiriess;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;

namespace ZAS_ERP.TemplateFields
{
    /// <summary>
    /// Interaction logic for FieldSelector.xaml
    /// "allTags" and "selectedTags" are the Fields having strings without Spaces used for Back-end
    /// "allFields" and "selectedFields" are the Fields having strings with Spaces to show on front-end
    /// 
    /// </summary>
    public partial class FieldSelector : Window
    {
        FieldsRepo fieldsRepo = new FieldsRepo();
        ModuleFields moduleFields = new ModuleFields();
        List<cmbitem> cmbAddedFields = new List<cmbitem>();
        List<cmbitem> cmbRemovedFields = new List<cmbitem>();

        List<string> allTags = new List<string>();
        List<string> allFields = new List<string>();

        List<string> selectedTags = new List<string>();
        List<string> selectedFields = new List<string>();

        List<Field> fields = new List<Field>();
        List<Field> removedFields = new List<Field>();
        List<Field> addedFields = new List<Field>();
        public FieldSelector()
        {
            InitializeComponent();
        }
        public void populatefIelds()
        {
            //Populating Combobox Module
            for (int i = 0; i <= (int)ERP_BL.Enums.TransactionItemType.Admin_Bill; i++)
            {
                cmbModule.Items.Add(((ERP_BL.Enums.TransactionItemType)i).ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            moduleFields = new ModuleFields();
            populatefIelds();
        }

        private void CmbModule_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var module = (ERP_BL.Enums.TransactionItemType)cmbModule.SelectedIndex;
            cmbTemplate.Text = null;

            lstAllFields.ItemsSource = null;
            lstSelectedFields.ItemsSource = null;

            allTags = new List<string>();
            selectedTags = new List<string>();

            allFields = new List<string>();
            selectedFields = new List<string>();

            if(module == ERP_BL.Enums.TransactionItemType.Admin_Bill)
            {
                ucFrmBillAdd frmBill = new ucFrmBillAdd();
                var mainGrid = frmBill.mainGrid;

                //Populating List box by getting All the Fields of a Usercontrol By Label
                foreach (UIElement element in mainGrid.Children)
                {
                    if (element is Label)
                    {
                        Label label = (Label)element;
                        allTags.Add(label.Tag.ToString());
                        allFields.Add(label.Content.ToString());
                    }
                    else if (element is Grid)
                    {
                        Grid grd = (Grid)element;
                        foreach (UIElement _child in grd.Children)
                        {
                            if (_child is Label)
                            {
                                Label label = (Label)_child;
                                allTags.Add(label.Tag.ToString());
                                allFields.Add(label.Content.ToString());
                            }
                        }
                    }
                }

                lstAllFields.ItemsSource = null;
                lstAllFields.ItemsSource = allFields;
            }

           

            //Populating cmbTemplate by getting the Templates of Selected Module
            var templates = fieldsRepo.GetAllTemplatesByModuleId(module);
            cmbTemplate.ItemsSource = templates;
           
        }

        private void BtnSendRight_Click(object sender, RoutedEventArgs e)
        {
            //Adding one item to Listbox of Selected Fields on button click
            if (cmbTemplate.SelectedIndex > -1)
            {
                var item = (string)lstAllFields.SelectedItem;
                if (item != null)
                {
                    var _item = allTags[allFields.IndexOf(item)];
                    selectedTags.Add(_item);
                    selectedFields.Add(item);
                    

                    lstSelectedFields.ItemsSource = null;
                    lstSelectedFields.ItemsSource = selectedFields;

                    allTags.Remove(_item);
                    allFields.Remove(item);
                    lstAllFields.ItemsSource = null;
                    lstAllFields.ItemsSource = allFields;

                    if(moduleFields != null)
                    {
                        //New Added fields to the existing Template
                        if (fields.FirstOrDefault(x => x.Tag == _item) == null)
                        {
                            Field field = new Field();
                            field.Tag = item;
                            field.ElementName = item;
                            field.LastModified = (DateTime)DateTime.Now;
                            addedFields.Add(field);
                        }
                        else
                        {
                            removedFields.RemoveAll(x=>x.Tag == _item);
                        }
                    }
                }
            }
            else
            {
                DXMessageBox.Show("Please select Template!");
                cmbTemplate.Focus();
                return;
            }

        }

        private void BtnSendLeft_Click(object sender, RoutedEventArgs e)
        {
           
            var item = (string)lstSelectedFields.SelectedItem;

            if (item != null)
            {
                var _item = selectedTags[selectedFields.IndexOf(item)];
                allTags.Add(_item);
                allFields.Add(item);
                lstAllFields.ItemsSource = null;
                lstAllFields.ItemsSource = allFields;
                
                selectedTags.Remove(_item);
                selectedFields.Remove(item);
                lstSelectedFields.ItemsSource = null;
                lstSelectedFields.ItemsSource = selectedFields;

                if(moduleFields.Id > 0)
                {
                    //Fields to be removed from the existing Template
                    removedFields.Add(fields.FirstOrDefault(x=>x.Tag == _item));
                    addedFields.RemoveAll(x=>x.Tag == _item);
                }
            }
           
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            

            if (cmbModule.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Module!");
                cmbModule.Focus();
                return;
            }
            if (cmbTemplate.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Template!");
                cmbTemplate.Focus();
                return;
            }
            if (lstSelectedFields.Items.Count < 1)
            {
                DXMessageBox.Show("Please select fields to add in Template!");
                return;
            }

            moduleFields.template = cmbTemplate.SelectedItem as Template;
            moduleFields.transactionType = (ERP_BL.Enums.TransactionItemType)cmbModule.SelectedIndex;

            moduleFields.fields = fields;

            if (moduleFields.Id == 0)
            {
                
                foreach (var _item in selectedTags)
                {
                    Field field = new Field();
                    field.Tag = _item.ToString();
                    field.ElementName = _item.ToString();
                    field.LastModified = (DateTime)DateTime.Now;
                    fields.Add(field);
                }

                //New template Saved
                fieldsRepo.AddModulesFields(moduleFields);
                MessageBox.Show("Successfully saved!");
            }
            else if(moduleFields.Id > 0)
            {
                //Fields to be removed
                foreach (var _field in removedFields)
                {
                    fields.RemoveAll(x=> x.Tag == _field.Tag);
                }

                //Fields to be added
                foreach (var _field in addedFields)
                {
                    fields.Add(_field);
                }

                //Existing Template updated
                fieldsRepo.UpdateModuleFields(moduleFields, removedFields);
                MessageBox.Show("Successfully updated!");
            }
            

           
        }

        private void BtnInquiryWin_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void MbtnAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAddTemplate addTemplate = new ucFrmAddTemplate();

            addTemplate.addTemplateWindow.Content = addTemplate;
            addTemplate.addTemplateWindow.Height = 300;
            addTemplate.addTemplateWindow.Width = 350;

            addTemplate.addTemplateWindow.ShowDialog();
        }

        private void MbtnTemplateList_Click(object sender, RoutedEventArgs e)
        {
            ucTemplateList templateList = new ucTemplateList();
            Window window = new Window();

            window.Content = templateList;
            window.Show();
        }

        private void CmbTemplate_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(cmbTemplate.SelectedIndex > -1)
            {
                var templateId = (cmbTemplate.SelectedItem as Template).Id;
                moduleFields = fieldsRepo.GetFieldsByTemplateNModule(templateId, (ERP_BL.Enums.TransactionItemType)cmbModule.SelectedIndex);
                if (moduleFields != null && moduleFields.Id > 0)
                {
                    fields = moduleFields.fields;
                    foreach (var _field in moduleFields.fields)
                    {

                        if (!selectedTags.Contains(_field.Tag))
                        {
                            selectedFields.Add(allFields[allTags.IndexOf(_field.Tag)]);
                            selectedTags.Add(_field.Tag);
                        }

                        if (allTags.Contains(_field.Tag))
                        {
                            allFields.RemoveAt(allTags.IndexOf(_field.Tag));
                            allTags.Remove(_field.Tag);
                        }

                    }
                    lstAllFields.ItemsSource = null;
                    lstSelectedFields.ItemsSource = null;
                    lstAllFields.ItemsSource = allFields;
                    lstSelectedFields.ItemsSource = selectedFields;
                }
                else
                {
                    moduleFields = new ModuleFields();
                }
            }
            
        }

        private void MbtnOpenBills_Click(object sender, RoutedEventArgs e)
        {
            ucFrmBill frmBill = new ucFrmBill();
            Window window = new Window();
            window.Content = frmBill;
            window.ShowDialog();
        }
    }
}
