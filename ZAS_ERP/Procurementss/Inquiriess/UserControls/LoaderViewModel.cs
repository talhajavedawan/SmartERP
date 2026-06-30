using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Procurementss.Inquiriess.UserControls
{
    public partial class LoaderViewModel : INotifyPropertyChanged
    {
        public LoaderViewModel()
        {
            WaitIndicatorText = "Loading...";
        }
        bool _isWaitIndicatorVisible;
        string _waitIndicatorText;
        public bool IsWaitIndicatorVisible
        {
            get
            {
                return _isWaitIndicatorVisible;
            }

            set
            {
                _isWaitIndicatorVisible = value;
                RaisePropertyChanged("IsWaitIndicatorVisible");
            }
        }

        public string WaitIndicatorText
        {
            get
            {
                return _waitIndicatorText;
            }

            set
            {
                _waitIndicatorText = value;
                RaisePropertyChanged("WaitIndicatorText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
