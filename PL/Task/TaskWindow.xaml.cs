using BlApi;
using PL.Director;
using PL.Engineer;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();
        public TaskWindow(int id = 0)
        {
            InitializeComponent();
            if (id != 0)
                try { CurrentTask = s_bl.Task.GetTaskDetails(id); }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }

            else
                CurrentTask = new BO.Task();
        }
        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));
       
        private void AddOrUpdate(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                string buttonText = button.Content.ToString();
                try
                {
                    if (buttonText == "Update")
                    {
                        s_bl.Task.Update(CurrentTask);

                    }
                    else if (buttonText == "Add")
                    {
                        s_bl.Task.Create(CurrentTask);

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            new AllTaskInListWindow().ShowDialog();
        }
    }
}