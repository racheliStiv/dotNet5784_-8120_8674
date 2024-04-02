using BlApi;
using DO;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for Engineer.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();
        public EngineerWindow(int id = 0)
        {
            InitializeComponent();
            if (id != 0)
                CurrentEngineer = s_bl.Engineer.GetEngineerDetails(id);
            else
                CurrentEngineer = new BO.Engineer();
        }
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        
        
    }
}
