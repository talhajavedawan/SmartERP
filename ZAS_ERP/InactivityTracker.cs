using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ZAS_ERP
{
   public  class InactivityTracker
    {
        public MainWindow myParent = null;

        private DispatcherTimer inactivityTimer;

        public bool logoutFromInactivity = false;
        public  bool winLoaded=false;

        public InactivityTracker(TimeSpan inactivityThreshold)
        {
            InitializeInactivityTimer(inactivityThreshold);
            WireUpActivityEvents();
        }

        private void InitializeInactivityTimer(TimeSpan inactivityThreshold)
        {
            inactivityTimer = new DispatcherTimer();
            inactivityTimer.Interval = inactivityThreshold;
            inactivityTimer.Tick += InactivityTimer_Tick;
            ResetInactivityTimer();
        }

        public void ResetInactivityTimer()
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            logoutFromInactivity = true;
            myParent.Close();
            // Perform logout actions here
            // Close all windows, clear session, etc.
            // For example:
            //Application.Current.Shutdown();
        }

        private void WireUpActivityEvents()
        {
            // Wire up events for user activity
            Application.Current.MainWindow.MouseMove += OnUserActivity;
            Application.Current.MainWindow.KeyDown += OnUserActivity;
            // Add more events as needed
        }

        private void OnUserActivity(object sender, EventArgs e)
        {
            ResetInactivityTimer();
        }
    }
}
