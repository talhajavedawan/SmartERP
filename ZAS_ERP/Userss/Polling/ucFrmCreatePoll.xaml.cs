using DevExpress.Xpf.Core;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using ERP_BL.User;
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

namespace ZAS_ERP.Userss.Polling
{
    /// <summary>
    /// Interaction logic for ucFrmCreatePoll.xaml
    /// </summary>
    public partial class ucFrmCreatePoll : UserControl
    {
        public int pollId = 0;
        Poll poll = new Poll();
        PollRepo pollRepo = new PollRepo();
        public bool EditFlag = false;

        public ucFrmCreatePoll()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPollingTypes();
            LoadGroups();
            if (EditFlag == false && pollId == 0)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtInitiatedBy.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
            }
            else if(EditFlag == true && pollId > 0)
            {
                poll = pollRepo.GetPoll(pollId);

                if(poll.CreationDate != null)
                    datCreationDate.DateTime = poll.CreationDate.Value;

                cmbPollingType.SelectedIndex = (int)poll.pollingType;

                if (poll.taskGroup != null)
                    lookupGroup.EditValue = poll.taskGroup.Id;

                if (poll.initiatedBy != null)
                    txtInitiatedBy.Text = poll.initiatedBy.employee.person.FName + " " + poll.initiatedBy.employee.person.LName;

                if (poll.ValidUntil != null)
                    datVaidUntilDate.EditValue = poll.ValidUntil.Value;

                if (!String.IsNullOrEmpty(poll.Title))
                    txtTitle.Text = poll.Title;
            }
            

            
        }

        private void LoadPollingTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.PollingType.Open_Polling; i++)
            {
                cmbPollingType.Items.Add(((ERP_BL.Enums.PollingType)i).ToString());
            }
        }

        private void LoadGroups()
        {
            ToDoTaskRepo taskRepo = new ToDoTaskRepo();
            lookupGroup.ItemsSource = taskRepo.GetAllBackgroundTaskGroups();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if( cmbPollingType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Polling Type!");
                cmbPollingType.Focus();
                return;
            }
            if (lookupGroup.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Group!");
                lookupGroup.Focus();
                return;
            }
            if (String.IsNullOrEmpty( txtTitle.Text))
            {
                DXMessageBox.Show("Please Enter Title!");
                txtTitle.Focus();
                return;
            }

            poll.CreationDate = datCreationDate.DateTime;
            poll.pollingType = (PollingType)cmbPollingType.SelectedIndex;
            poll.Title = txtTitle.Text;
            poll.taskGroupId = (lookupGroup.SelectedItem as TaskGroups).Id;
            
            poll.ValidUntil = datVaidUntilDate.DateTime;

            if (EditFlag == false)
            {
                poll.initiatedById = SYSTEM_STATIC.currentUser.id;
                pollRepo.AddPoll(poll);
                DXMessageBox.Show("Successfully Added!");
            }
            else
            {
                pollRepo.UpdatePoll(poll);
                DXMessageBox.Show("Successfully Updated!");
            }

            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (window1 != null) { window1.PollingsGlow(); }

            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
