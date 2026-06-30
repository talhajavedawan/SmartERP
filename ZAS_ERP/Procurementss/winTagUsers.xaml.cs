using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for winTagUsers.xaml
    /// </summary>
    public partial class winTagUsers : Window
    {
        public List<ERP_BL.Databases.Employee> Employees { get; set; }
        public List<User> tagUsers = new List<User>();
        public List<User> ccUsers = new List<User>();
        public List<User> tagRecommendationUsers = new List<User>();
        public List<User> ccRecommendationUsers = new List<User>();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        CommentLog lastComment = new CommentLog();
        public winTagUsers()
        {
            InitializeComponent();
        }

        public winTagUsers(List<User> Users, int TransactionId, TransactionItemType transactionItemType)
        {
            InitializeComponent();

            List<cmbitem> items = new List<cmbitem>();
            List<cmbitem> empss = new List<cmbitem>();
            this.DataContext = this;
            // taggedUsers = Users;
            Employees = new List<ERP_BL.Databases.Employee>();

            //foreach (User user in Users)
            //{
            //    //Employees.Add(user.employee);
            //    //items.Add(new cmbitem()
            //    //{
            //    //    id = user.id,
            //    //    name = user.userName
            //    //});
            //    //empss.Add(new cmbitem()
            //    //{
            //    //    id = user.employee.EmpId,
            //    //    name = user.employee.person.FName + " " + user.employee.person.LName
            //    //});

            //}
            cmbTagUsers.ItemsSource = Users;
            cmbCCUsers.ItemsSource = Users;
            cmbRecommentdationTagUsers.ItemsSource = Users;
            cmbRecomendationCCUsers.ItemsSource = Users;
            var lastComment = procurementRepo.getcommentlogAsc(TransactionId, transactionItemType);

            if (lastComment != null)
            {
                if(lastComment.TaggedRecomenndedList.Count==0)
                {
                    if(lastComment.CCRecomenndedList.Count==0)
                    {
                        btnApplyRecommendation.Content="Apply Suggestions";
                        lblTagRecommendation.Text="Tag Suggestions";
                        lblCCRecommendation.Text="CC Suggestions";
                        btnApplyRecommendation.Visibility = Visibility.Collapsed;
                    }
                }
            }
            if (lastComment != null)
            {

                if (lastComment.TaggedRecomenndedList != null && lastComment.TaggedRecomenndedList.Count > 0)
                {
                    var taggedRecommendationusersSource = cmbRecommentdationTagUsers.ItemsSource as List<User>;
                    foreach (User cmbitem in taggedRecommendationusersSource)
                    {
                        if (lastComment.TaggedRecomenndedList.Find(x => x.id == cmbitem.id) != null)
                        {
                            cmbRecommentdationTagUsers.SelectedItems.Add(cmbitem);
                            //break;
                        }
                    }

                }
                if (lastComment.CCRecomenndedList != null && lastComment.CCRecomenndedList.Count > 0)
                {
                    var CCRecomenndationUsersSource = cmbRecomendationCCUsers.ItemsSource as List<User>;
                    foreach (User cmbitem in CCRecomenndationUsersSource)
                    {
                        if (lastComment.CCRecomenndedList.Find(x => x.id == cmbitem.id) != null)
                        {
                            cmbRecomendationCCUsers.SelectedItems.Add(cmbitem);
                            //break;
                        }
                    }
                }
            }

        }

        private void BtnSaveTag_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTagUsers.SelectedItems.Count != 0)
            {
                if (cmbTagUsers.SelectedItem != null)
                {
                    var taggedUsers = new List<User>();
                    foreach (var user in cmbTagUsers.SelectedItems)
                    {
                        taggedUsers.Add(new User { id = (cmbTagUsers.SelectedItem as User).id, employeeId = (cmbTagUsers.SelectedItem as User).employeeId });
                    }
                    tagUsers = taggedUsers;

                }
            }
            else
            {
                tagUsers = new List<User>();
            }

            if (cmbCCUsers.SelectedItems.Count != 0)
            {
                if (cmbCCUsers.SelectedItems != null)
                {
                    var CCUsers = new List<User>();
                    foreach (User user in cmbCCUsers.SelectedItems)
                    {
                        CCUsers.Add(new User { id = user.id });

                    }
                    ccUsers = CCUsers;

                }
            }
            else
            {
                ccUsers = new List<User>();
            }
            if (cmbRecomendationCCUsers.SelectedItems.Count != 0)
            {
                if (cmbRecomendationCCUsers.SelectedItems != null)
                {
                    var CCUsers = new List<User>();
                    foreach (User user in cmbRecomendationCCUsers.SelectedItems)
                    {
                        CCUsers.Add(new User { id = user.id });

                    }
                    ccRecommendationUsers = CCUsers;
                }
            }
            else
            {
                ccRecommendationUsers = new List<User>();
            }
            if (cmbRecommentdationTagUsers.SelectedItems.Count != 0)
            {
                if (cmbRecommentdationTagUsers.SelectedItem != null)
                {
                    var taggedUsers = new List<User>();
                    foreach (var user in cmbRecommentdationTagUsers.SelectedItems)
                    {
                        taggedUsers.Add(new User { id = (cmbRecommentdationTagUsers.SelectedItem as User).id, employeeId = (cmbRecommentdationTagUsers.SelectedItem as User).employeeId });
                    }
                    tagRecommendationUsers = taggedUsers;
                }
            }
            else
            {
                tagRecommendationUsers = new List<User>();
            }
            this.Close();
        }

        private void BtnAvoidTag_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnApplyRecommendation_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRecommentdationTagUsers.SelectedItems != null)
            {
                var TagUsersSourceRec = cmbRecommentdationTagUsers.SelectedItems.ToList();
                var TagUsersSource = cmbTagUsers.ItemsSource as List<User>;
                foreach (User cmbitem in TagUsersSource)
                {
                    foreach (User cmbitem1 in TagUsersSourceRec)
                    {
                        if (cmbitem.id == cmbitem1.id)
                        {
                            cmbTagUsers.SelectedItems.Add(cmbitem);

                        }
                    }
                }
                
            }
            if (cmbRecomendationCCUsers.SelectedItems != null)
            {
                var CCUsersSource = cmbCCUsers.ItemsSource as List<User>;
                var CCUsersSourceRec = cmbRecomendationCCUsers.SelectedItems.ToList();
                foreach (User cmbitem in CCUsersSource)
                {
                    foreach (User cmbitem1 in CCUsersSourceRec)
                    {
                        if (cmbitem.id == cmbitem1.id)
                        {
                            cmbCCUsers.SelectedItems.Add(cmbitem);

                        }
                    }
                }
            }
        }

    }
}
