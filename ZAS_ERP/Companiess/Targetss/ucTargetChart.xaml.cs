using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ZAS_ERP.Companiess.Targetss
{
    /// <summary>
    /// Interaction logic for ucTargetChart.xaml
    /// </summary>
    public partial class ucTargetChart : Window
    {
        public ucTargetChart( )
        {
            InitializeComponent();
        }
        public ucTargetChart(List<AchivedTarget> targets)
        {
            InitializeComponent();
            TargetsViewModel obj = new TargetsViewModel(targets);
            DataContext = obj;
            //DataContext = obj;
        }
    }
    public class TargetsViewModel
    {
        public ObservableCollection<DataPoint> Data { get; private set; }
        public TargetsViewModel()
        {
        }
        public TargetsViewModel(List< AchivedTarget> targets)
        {
            this.Data = DataPoint.GetDataPoints(targets);
        }
    }
    public class DataPoint
    {
        public string Argument { get; set; }
        public double Value { get; set; }
        public static ObservableCollection<DataPoint> GetDataPoints(List<AchivedTarget> targets)
        {
            ObservableCollection<DataPoint> data = new ObservableCollection<DataPoint>();
            foreach (var target in targets)
            {
                DataPoint target1 = new DataPoint { Argument = target.target.ToString(), Value = target.achivedTarget };
                data.Add(target1);
            }
            return data;
        }
    }
}


