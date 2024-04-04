using BlApi;
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
    /// Interaction logic for ConnectEngineer.xaml
    /// </summary>
    public partial class ConnectEngineer : Window
    {
        readonly IBl s_bl = BlApi.Factory.Get();

        public ConnectEngineer(int id)
        {
            InitializeComponent();

            try
            {
                CurrentEngineer = s_bl.Engineer.GetEngineerDetails(id);

                if (CurrentEngineer == null || CurrentEngineer.Task == null)
                {
                    MessageBox.Show("No task assigned to this engineer.");
                    return; // Exit the function if no task assigned
                }

                CurrentTask = s_bl.Task.GetTaskDetails(CurrentEngineer.Task.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(ConnectEngineer), new PropertyMetadata(null));

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(ConnectEngineer), new PropertyMetadata(null));

        private void complete_task(object sender, RoutedEventArgs e)
        {
            // Add logic to complete the task
        }
    }
}
